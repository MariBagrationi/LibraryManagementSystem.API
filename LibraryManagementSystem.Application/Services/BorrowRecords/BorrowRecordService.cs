using LibraryManagementSystem.Application.Exceptions.BookExceptions;
using LibraryManagementSystem.Application.Exceptions.BorrowRecordExceptions;
using LibraryManagementSystem.Application.Exceptions.PatronExceptions;
using LibraryManagementSystem.Application.Models.Requests;
using LibraryManagementSystem.Application.Models.Responses;
using LibraryManagementSystem.Application.Repositories;
using LibraryManagementSystem.Domain.Interfaces;
using LibraryManagementSystem.Domain.Models;
using Mapster;
using Serilog;

namespace LibraryManagementSystem.Application.Services.BorrowRecords
{
    public class BorrowRecordService : IBorrowRecordService
    {
        private readonly IBorrowRecordRepository _borrowRecordRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IPatronRepository _patronRepository;
        private readonly IUnitOfWork _unitOfWork;
        public BorrowRecordService(IBorrowRecordRepository borrowRecordRepository,
                                  IBookRepository bookRepository,
                                  IPatronRepository patronRepository,
                                  IUnitOfWork unitOfWork)                       
        {
            _borrowRecordRepository = borrowRecordRepository;
            _bookRepository = bookRepository;
            _patronRepository = patronRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<BorrowRecordResponseModel> Get(int id, CancellationToken cancellationToken)
        {
            var borrowRecord = await _borrowRecordRepository.Get(id, cancellationToken).ConfigureAwait(false);

            if (borrowRecord == null)
                throw new BorrowRecordDoesNotExistEx("Borrow record with such id does not exist");

            return borrowRecord.Adapt<BorrowRecordResponseModel>();
        }

        public async Task<List<BorrowRecordResponseModel>> GetAll(int? PatronId, int? bookId, 
                                                                  Status? status, DateTime? borrowedAfter, 
                                                                  DateTime? borrowedBefore, 
                                                                  CancellationToken cancellationToken)
        {
            var borrowRecords = await _borrowRecordRepository
                .GetAll(PatronId, bookId, status, borrowedAfter, borrowedBefore, cancellationToken)
                .ConfigureAwait(false);

            return borrowRecords.Adapt<List<BorrowRecordResponseModel>>();
        }
        
        public async Task<BookResponseModel> Create(BorrowRecordRequestModel borrowRecord, CancellationToken cancellationToken)
        {
            if(borrowRecord == null) 
                throw new ArgumentNullException(nameof(borrowRecord));

            if (await _borrowRecordRepository.Get(borrowRecord.Id, cancellationToken).ConfigureAwait(false) != null)
                throw new BorrowRecordAlreadyExistsEx("Borrow record with such id already exists");

            var book = await _bookRepository.Get(borrowRecord.BookId, cancellationToken).ConfigureAwait(false);

            if (book == null)
                throw new BookDoesNotExistEx("A book with such Id does not exist");

            if (await _patronRepository.Get(borrowRecord.PatronId, cancellationToken).ConfigureAwait(false) == null)
                throw new PatronDoesNotExistEx("A Patron with such Id does not exist");

            if (book.RemainingQuantity <= 0)
                throw new BookIsNotAvailableEx("This book is not available in the library right now");

            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                borrowRecord.BorrowDate = DateTime.UtcNow;
                borrowRecord.status = Status.Borrowed;
                book.RemainingQuantity -= 1;

                await _borrowRecordRepository.Create(borrowRecord.Adapt<BorrowRecord>(), cancellationToken).ConfigureAwait(false);
                await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                Log.Error(ex, "Transaction failed while creating a borrow record.");
                throw new Exception("An error occurred while creating the borrow record.", ex);
            }

            return borrowRecord.Adapt<BookResponseModel>();
        }
       
        public async Task<bool> Delete(int id, CancellationToken cancellationToken)
        {
            var record = await _borrowRecordRepository.Get(id, cancellationToken).ConfigureAwait(false);
            
            if (record == null)
                throw new BorrowRecordDoesNotExistEx("Borrow record with such id does not exist");

            var book = await _bookRepository.Get(record.BookId, cancellationToken).ConfigureAwait(false);

            if(book == null)
                throw new BookDoesNotExistEx("Book with such Id does not exist");

            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                book.RemainingQuantity += 1;
                _bookRepository.Update(book, cancellationToken);
                await _borrowRecordRepository.Delete(id, cancellationToken).ConfigureAwait(false);
                await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                await transaction.CommitAsync(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                Log.Error(ex, "Transaction failed while deleting a borrow record.");
                throw;
            }
        }

        public async Task<List<BorrowRecordResponseModel>> GetOverdueBooks(CancellationToken cancellationToken)
        {
            var overdues = await _borrowRecordRepository.GetAll(cancellationToken).ConfigureAwait(false);
            return overdues.Adapt<List<BorrowRecordResponseModel>>();
        }

        public async Task<BookResponseModel> ReturnBook(int recordId, CancellationToken cancellationToken)
        {
            var borrowRecord = await _borrowRecordRepository.Get(recordId, cancellationToken).ConfigureAwait(false);
            if (borrowRecord == null)
                throw new BorrowRecordDoesNotExistEx("Borrow record with such id does not exist");

            if (borrowRecord.status == Status.Returned)
                throw new InvalidOperationException("This book has already been returned.");

            var book = await _bookRepository.Get(borrowRecord.BookId, cancellationToken);
            if(book == null)
                throw new BookDoesNotExistEx("Book with such Id does not exist");

            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                borrowRecord.status = Status.Returned;
                borrowRecord.ReturnDate = DateTime.UtcNow;

                book.RemainingQuantity += 1;

                _borrowRecordRepository.Update(borrowRecord, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                await transaction.CommitAsync(cancellationToken);
                return book.Adapt<BookResponseModel>();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                Log.Error(ex, "Transaction failed while returning a book.");
                throw;
            }
        }
    }
}

using LibraryManagementSystem.Application.Exceptions.BookExceptions;
using LibraryManagementSystem.Application.Models;
using LibraryManagementSystem.Application.Models.Requests;
using LibraryManagementSystem.Application.Models.Responses;
using LibraryManagementSystem.Application.Repositories;
using LibraryManagementSystem.Domain.Interfaces;
using LibraryManagementSystem.Domain.Models;
using Mapster;
using Serilog;

namespace LibraryManagementSystem.Application.Services.Books
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUnitOfWork _unitOfWork;
        public BookService(IBookRepository bookRepository, IUnitOfWork unitOfWork)
        {
            _bookRepository = bookRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<PagedResult<BookResponseModel>> GetAll(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var books = await _bookRepository
                .GetAll(pageNumber, pageSize, cancellationToken)
                .ConfigureAwait(false);

            return books.Adapt<PagedResult<BookResponseModel>>();
        }

        public async Task<BookResponseModel> Get(int id, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.Get(id, cancellationToken);
            if(book == null)
            {
                Log.Information($"Exception: {BookDoesNotExistEx.Code}");
                throw new BookDoesNotExistEx("Book with such id does not exist");
            }
            return book.Adapt<BookResponseModel>();
        }

        public async Task<BookResponseModel> Create(BookRequestModel book, CancellationToken cancellationToken)
        {
            if (book == null)
            {
                Log.Information($"Exception: {nameof(ArgumentNullException)}");
                throw new ArgumentNullException(nameof(book));
            }

            if(await _bookRepository.Get(book.Id, cancellationToken).ConfigureAwait(false) != null)
            {
                Log.Information($"Exception: {BookAlreadyExistsEx.Code}");
                throw new BookAlreadyExistsEx("the book already exists");
            }

            var entity = book.Adapt<Book>();
            entity.RemainingQuantity = book.Quantity;
            await _bookRepository.Create(book.Adapt<Book>(), cancellationToken).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return book.Adapt<BookResponseModel>();
        }

        public async Task Update(BookRequestModel book, CancellationToken cancellationToken)
        {
            if(book == null)
            {
                Log.Information($"Exception: {nameof(ArgumentNullException)}");
                throw new ArgumentNullException(nameof(book));
            }
            var existingBook = await _bookRepository.Get(book.Id, cancellationToken).ConfigureAwait(false);
            if (existingBook == null)
            {
                Log.Information($"Exception: {BookDoesNotExistEx.Code}");
                throw new BookDoesNotExistEx("the book does not exist");
            }

            book.Adapt(existingBook);
            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task Delete(int id, CancellationToken cancellationToken)
        { 
            if (await _bookRepository.Get(id, cancellationToken).ConfigureAwait(false) == null)
            {
                Log.Information($"Exception: {BookDoesNotExistEx.Code}");
                throw new BookDoesNotExistEx("the book does not exist");
            }

            await _bookRepository.Delete(id, cancellationToken).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        public async Task<List<BookResponseModel>> SearchBooks(string title, string author, CancellationToken cancellationToken)
        {
            var books =  await _bookRepository
                .SearchBooks(title, author, cancellationToken)
                .ConfigureAwait(false);

            return books.Adapt<List<BookResponseModel>>();
        }
        public async Task<bool> CheckAvailability(int id, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.Get(id, cancellationToken).ConfigureAwait(false);
            if (book == null)
            {
                Log.Information($"Exception: {BookDoesNotExistEx.Code}");
                throw new BookDoesNotExistEx("the book does not exist");
            }
            if(book.Quantity > 0)
                return true;

            return false;
        }
    }
}

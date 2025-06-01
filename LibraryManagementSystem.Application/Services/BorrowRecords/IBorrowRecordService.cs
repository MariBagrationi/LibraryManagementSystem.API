using LibraryManagementSystem.Application.Models.Requests;
using LibraryManagementSystem.Application.Models.Responses;
using LibraryManagementSystem.Domain.Models;

namespace LibraryManagementSystem.Application.Services.BorrowRecords
{
    public interface IBorrowRecordService
    {
        Task<BorrowRecordResponseModel> Get(int id, CancellationToken cancellationToken);
        Task<List<BorrowRecordResponseModel>> GetAll(int? PatronId, int? bookId,
                                                                  Status? status, DateTime? borrowedAfter,
                                                                  DateTime? borrowedBefore,
                                                                  CancellationToken cancellationToken);
        Task<List<BorrowRecordResponseModel>> GetOverdueBooks(CancellationToken cancellationToken);
        Task<BookResponseModel> Create(BorrowRecordRequestModel borrowRecord, CancellationToken cancellationToken);
        Task<bool> Delete(int id, CancellationToken cancellationToken);
        Task<BookResponseModel> ReturnBook(int recordId, CancellationToken cancellationToken);
    }
}

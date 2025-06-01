using LibraryManagementSystem.Application.Models.Requests;
using LibraryManagementSystem.Application.Models.Responses;
using LibraryManagementSystem.Domain.Models;
using LibraryManagementSystem.Domain.Interfaces;
using System.Linq.Expressions;

namespace LibraryManagementSystem.Application.Repositories
{
    public interface IBorrowRecordRepository : IBaseRepository<BorrowRecord>
    {
        Task<List<BorrowRecord>> GetAll(int? PatronId, int? bookId,
                                                                  Status? status, DateTime? borrowedAfter,
                                                                  DateTime? borrowedBefore,
                                                                  CancellationToken cancellationToken);
        Task<List<BorrowRecord>> GetAll(CancellationToken cancellationToken);
        Task<BorrowRecord?> Get(int id, CancellationToken cancellationToken);
        Task Create(BorrowRecord borrowRecord, CancellationToken cancellationToken);
        new void Update(BorrowRecord borrowRecord, CancellationToken cancellationToken);
        Task Delete(int id, CancellationToken cancellationToken);
        Task<List<BorrowRecord>> GetOverdues(CancellationToken cancellationToken);
        //Task<Book> ReturnBook(int recordId, CancellationToken cancellationToken);
        public void Attach(BorrowRecord entity);
        public void Detach(BorrowRecord entity);
    }
}

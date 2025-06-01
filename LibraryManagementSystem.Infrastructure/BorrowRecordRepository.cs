using LibraryManagementSystem.Application.Repositories;
using LibraryManagementSystem.Domain.Models;
using LibraryManagementSystem.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Infrastructure
{
    public class BorrowRecordRepository : BaseRepository<BorrowRecord>, IBorrowRecordRepository
    {
        public BorrowRecordRepository(LibraryManagementContext context) : base(context)
        {
        }

        public async Task Create(BorrowRecord borrowRecord, CancellationToken cancellationToken)
        {
            await base.CreateAsync(borrowRecord, cancellationToken).ConfigureAwait(false);
        }

        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            await base.DeleteAsync(new object[] { id }, cancellationToken).ConfigureAwait(false);
        }

        public async Task<BorrowRecord?> Get(int id, CancellationToken cancellationToken)
        {
            return await base.GetAsync(new object[] { id }, cancellationToken).ConfigureAwait(false);
        }

        public async Task<List<BorrowRecord>> GetAll(CancellationToken cancellationToken)
        {
            return await base.GetAllAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<List<BorrowRecord>> GetAll(int? PatronId, int? bookId, Status? status, 
                    DateTime? borrowedAfter, DateTime? borrowedBefore, CancellationToken cancellationToken)
        {
            var borrowRecords = base._dbSet.AsQueryable();
            if (PatronId.HasValue)
                borrowRecords = borrowRecords.Where(br => br.PatronId == PatronId.Value);
            if (bookId.HasValue)
                borrowRecords = borrowRecords.Where(br => br.BookId == bookId.Value);
            if (status.HasValue)
                borrowRecords = borrowRecords.Where(br => br.status == status.Value);
            if (borrowedAfter.HasValue)
                borrowRecords = borrowRecords.Where(br => br.BorrowDate >= borrowedAfter.Value);
            if (borrowedBefore.HasValue)
                borrowRecords = borrowRecords.Where(br => br.BorrowDate <= borrowedBefore.Value);

            return await borrowRecords.ToListAsync(cancellationToken);
        }

        public async Task<List<BorrowRecord>> GetOverdues(CancellationToken cancellationToken)
        {
            var overdues = base._dbSet
                .Where(b => b.DueDate < DateTime.UtcNow && b.status == Status.Overdue);

            return await overdues.ToListAsync();
        }

        public new void Update(BorrowRecord borrowRecord, CancellationToken cancellationToken)
        {
            base.Update(borrowRecord, cancellationToken);
        }
    }
}

using LibraryManagementSystem.Domain.Interfaces;
using LibraryManagementSystem.Persistance.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace LibraryManagementSystem.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryManagementContext _context;
        public UnitOfWork(LibraryManagementContext context)
        {
            _context = context;
        }
        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
        {
            return _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken)
        {
            return transaction.CommitAsync(cancellationToken);
        }

        public Task RollbackTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken)
        {
            return transaction.RollbackAsync(cancellationToken);
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}

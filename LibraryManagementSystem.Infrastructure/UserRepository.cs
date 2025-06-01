using LibraryManagementSystem.Application.Repositories;
using LibraryManagementSystem.Domain.Models;
using LibraryManagementSystem.Infrastructure;
using LibraryManagementSystem.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace PersonManagement.Infrastructure.Users
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(LibraryManagementContext context) : base(context)
        {
        }

        public async Task<User?> GetAsync(string username, CancellationToken cancellationToken)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Username == username, cancellationToken)
                .ConfigureAwait(false);
        }

        public async  Task<User?> GetByTokenAsync(string token, CancellationToken cancellationToken)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Token == token, cancellationToken)
                .ConfigureAwait(false);
        }

        public new async Task<string> CreateAsync(User user, CancellationToken cancellationToken)
        {
            await _dbSet.AddAsync(user, cancellationToken).ConfigureAwait(false);
            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return user.Username!;
        }

        public Task<bool> Exists(string username, CancellationToken cancellationToken)
        {
            return _dbSet.AnyAsync(u => u.Username == username, cancellationToken);
        }
    }

}

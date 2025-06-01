using LibraryManagementSystem.Domain.Models;

namespace LibraryManagementSystem.Application.Repositories
{
    public interface IUserRepository
    {
        Task<string> CreateAsync(User user, CancellationToken cancellationToken);
        Task<User?> GetAsync(string username, CancellationToken cancellationToken);
        Task<bool> Exists(string username, CancellationToken cancellationToken);
        Task<User?> GetByTokenAsync(string token, CancellationToken cancellationToken);
    }
}

using LibraryManagementSystem.Application.Models;

namespace LibraryManagementSystem.Domain.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<PagedResult<T>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<T?> GetAsync(object[] key, CancellationToken cancellationToken);
        Task CreateAsync(T entity, CancellationToken cancellationToken);
        void Update(T entity, CancellationToken cancellationToken);
        Task DeleteAsync(object[] key, CancellationToken cancellationToken);
        void Delete(T entity, CancellationToken cancellationToken);
    }
}

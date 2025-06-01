using LibraryManagementSystem.Application.Models;
using LibraryManagementSystem.Domain.Models;

namespace LibraryManagementSystem.Application.Repositories
{
    public interface IPatronRepository
    {
        Task<PagedResult<Patron>> GetAll(int number, int size, CancellationToken cancellationToken);
        Task<Patron?> Get(int id, CancellationToken cancellationToken);
        Task Create(Patron patron, CancellationToken cancellationToken);
        void Update(Patron patron, CancellationToken cancellationToken);
        Task Delete(int id, CancellationToken cancellationToken);
        public void Attach(Patron entity);
        public void Detach(Patron entity);
    }
}

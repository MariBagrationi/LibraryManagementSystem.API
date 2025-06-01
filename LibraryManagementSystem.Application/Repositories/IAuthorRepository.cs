using LibraryManagementSystem.Application.Models;
using LibraryManagementSystem.Application.Models.Responses;
using LibraryManagementSystem.Domain.Models;

namespace LibraryManagementSystem.Application.Repositories
{
    public interface IAuthorRepository
    {
        Task<PagedResult<Author>> GetAll(int firstPage, int lastPage, CancellationToken cancellationToken);
        Task<Author?> Get(int id, CancellationToken cancellationToken);
        Task<List<BookResponseModel>> GetBooksByAuthor(int id, CancellationToken cancellationToken);
        Task Create(Author book, CancellationToken cancellationToken);
        void Update(Author book, CancellationToken cancellationToken);
        Task Delete(int id, CancellationToken cancellationToken);
        public void Attach(Author entity);
        public void Detach(Author entity);
    }
}

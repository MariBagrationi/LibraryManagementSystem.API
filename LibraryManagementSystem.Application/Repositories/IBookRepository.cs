using LibraryManagementSystem.Application.Models;
using LibraryManagementSystem.Application.Models.Responses;
using LibraryManagementSystem.Domain.Interfaces;
using LibraryManagementSystem.Domain.Models;

namespace LibraryManagementSystem.Application.Repositories
{
    public interface IBookRepository : IBaseRepository<Book>
    {
        Task<PagedResult<Book>> GetAll(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<Book?> Get(int id, CancellationToken cancellationToken);
        Task<List<BookResponseModel>> GetBooksByAuthor(int id, CancellationToken cancellationToken);
        Task<List<Book>> SearchBooks(string title, string author,  CancellationToken cancellationToken);
        Task Create(Book book, CancellationToken cancellationToken);
        new void Update(Book book, CancellationToken cancellationToken);
        Task Delete(int id, CancellationToken cancellationToken);
        Task<List<Book>> GetAllBooksbyPatronId(int patronId, CancellationToken cancellationToken);
        public void Attach(Book entity);
        public void Detach(Book entity);
    }
}

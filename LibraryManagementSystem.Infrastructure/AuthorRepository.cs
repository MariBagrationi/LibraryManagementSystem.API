using LibraryManagementSystem.Application.Models;
using LibraryManagementSystem.Application.Models.Responses;
using LibraryManagementSystem.Application.Repositories;
using LibraryManagementSystem.Domain.Models;
using LibraryManagementSystem.Persistance.Context;

namespace LibraryManagementSystem.Infrastructure
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        IBookRepository _bookRepository;
        public AuthorRepository(LibraryManagementContext context, IBookRepository bookRepository) : base(context)
        {
            _bookRepository = bookRepository;
        }
        public async Task<PagedResult<Author>> GetAll(int firstPage, int lastPage, CancellationToken cancellationToken)
        {
            return await base.GetAllAsync(firstPage, lastPage, cancellationToken).ConfigureAwait(false);
        }
        public async Task Create(Author author, CancellationToken cancellationToken)
        {
            await base.CreateAsync(author, cancellationToken).ConfigureAwait(false);
        }

        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            await base.DeleteAsync(new object[] { id }, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Author?> Get(int id, CancellationToken cancellationToken)
        {
            return await base.GetAsync(new object[] { id }, cancellationToken).ConfigureAwait(false);
        }

        public new void Update(Author author, CancellationToken cancellationToken)
        {
            base.Update(author, cancellationToken);
        }

        public async Task<List<BookResponseModel>> GetBooksByAuthor(int id, CancellationToken cancellationToken)
        {
            return await _bookRepository.GetBooksByAuthor(id, cancellationToken).ConfigureAwait(false);
        }
    }
}

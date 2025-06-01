using LibraryManagementSystem.Application.Models;
using LibraryManagementSystem.Application.Repositories;
using LibraryManagementSystem.Domain.Models;
using LibraryManagementSystem.Persistance.Context;

namespace LibraryManagementSystem.Infrastructure
{
    public class PatronRepository : BaseRepository<Patron>, IPatronRepository
    {
        IBookRepository _bookRepository;
        public PatronRepository(LibraryManagementContext context, IBookRepository bookRepository) : base(context)
        {
            _bookRepository = bookRepository;
        }

        public async Task<Patron?> Get(int id, CancellationToken cancellationToken)
        {
            return await base.GetAsync(new object[] { id }, cancellationToken).ConfigureAwait(false);
        }

        public async Task<PagedResult<Patron>> GetAll(int number, int size, CancellationToken cancellationToken)
        {
            return await base.GetAllAsync(number, size, cancellationToken).ConfigureAwait(false);
        }

        public async Task Create(Patron patron, CancellationToken cancellationToken)
        {
            await base.CreateAsync(patron, cancellationToken).ConfigureAwait(false);
        }

        public async Task Delete(int id, CancellationToken cancellationToken)
        {
           await base.DeleteAsync(new object[] { id }, cancellationToken).ConfigureAwait(false);
        }

        public new void Update(Patron patron, CancellationToken cancellationToken)
        {
            base.Update(patron, cancellationToken);
        }

    }
}

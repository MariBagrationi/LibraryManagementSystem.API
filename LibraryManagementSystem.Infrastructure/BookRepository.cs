using LibraryManagementSystem.Application.Models;
using LibraryManagementSystem.Application.Models.Responses;
using LibraryManagementSystem.Application.Repositories;
using LibraryManagementSystem.Domain.Models;
using LibraryManagementSystem.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Infrastructure
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(LibraryManagementContext context) : base(context)
        {
        }

        public async Task Create(Book book, CancellationToken cancellationToken)
        {
            await base.CreateAsync(book, cancellationToken).ConfigureAwait(false);
        }

        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            await base.DeleteAsync(new object[] { id }, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Book?> Get(int id, CancellationToken cancellationToken)
        {
            return await base.GetAsync(new object[] { id }, cancellationToken).ConfigureAwait(false);
        }

        public async Task<List<Book>> GetAll(CancellationToken cancellationToken)
        {
            return await base.GetAllAsync(cancellationToken).ConfigureAwait(false);
        }

        public new void Update(Book book, CancellationToken cancellationToken)
        {
            base.Update(book, cancellationToken);
        }
       
        public async Task<List<BookResponseModel>> GetBooksByAuthor(int AuthorID, CancellationToken cancellationToken)
        {
            var books = base._dbSet
                .Where(b => b.AuthorId == AuthorID)
                .Select(b => new BookResponseModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    AuthorId = b.AuthorId,
                    Quantity = b.Quantity,
                    PublicationYear = b.PublicationYear,
                    CoverImageUrl = b.CoverImageUrl,
                    Description = b.Description,
                    ISBN = b.ISBN
                });

            return await books.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<PagedResult<Book>> GetAll(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            return await base.GetAllAsync(pageNumber, pageSize, cancellationToken).ConfigureAwait(false);
        }

        public async Task<List<Book>> SearchBooks(string title, string author, CancellationToken cancellationToken)
        {
            var books = base._dbSet
                .Include(b => b.Author)
                .Include(b => b.Title)
                .Where(b => (string.IsNullOrEmpty(title) || b.Title.Contains(title, StringComparison.OrdinalIgnoreCase)) &&
                            (string.IsNullOrEmpty(author) || b.Author.FirstName.Contains(author, StringComparison.OrdinalIgnoreCase)));

            return await books.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<List<Book>> GetAllBooksbyPatronId(int patronId, CancellationToken cancellationToken)
        {
            var books = base._dbSet
                .Where(p => p.Id == patronId)
                .AsNoTracking();

            return await books.ToListAsync().ConfigureAwait(false);
        }

    }
}

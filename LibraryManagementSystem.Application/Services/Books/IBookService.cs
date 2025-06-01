using LibraryManagementSystem.Application.Models;
using LibraryManagementSystem.Application.Models.Requests;
using LibraryManagementSystem.Application.Models.Responses;

namespace LibraryManagementSystem.Application.Services.Books
{
    public interface IBookService
    {
        Task<PagedResult<BookResponseModel>> GetAll(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<BookResponseModel> Get(int id, CancellationToken cancellationToken);
        Task<List<BookResponseModel>> SearchBooks(string title, string author, CancellationToken cancellationToken);
        Task<BookResponseModel> Create(BookRequestModel book, CancellationToken cancellationToken);
        Task Update(BookRequestModel book, CancellationToken cancellationToken);
        Task Delete(int id, CancellationToken cancellationToken);
        Task<bool> CheckAvailability(int bookId, CancellationToken cancellationToken);
    }
}

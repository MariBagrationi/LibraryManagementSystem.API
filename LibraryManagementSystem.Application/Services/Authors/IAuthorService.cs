using LibraryManagementSystem.Application.Models;
using LibraryManagementSystem.Application.Models.Requests;
using LibraryManagementSystem.Application.Models.Responses;

namespace LibraryManagementSystem.Application.Services.Authors
{
    public interface IAuthorService
    {
        Task<PagedResult<AuthorResponseModel>> GetAll(int page, int pageSize, CancellationToken cancellationToken);
        Task<List<BookResponseModel>> GetBooksByAuthor(int id, CancellationToken cancellationToken);
        Task<AuthorResponseModel> Get(int id, CancellationToken cancellationToken);
        Task<AuthorResponseModel> Create(AuthorRequestModel book, CancellationToken cancellationToken);
        Task Update(AuthorRequestModel book, CancellationToken cancellationToken);
        Task Delete(int id, CancellationToken cancellationToken);
    }
}

using LibraryManagementSystem.Application.Models;
using LibraryManagementSystem.Application.Models.Requests;
using LibraryManagementSystem.Application.Models.Responses;
using System.Linq.Expressions;

namespace LibraryManagementSystem.Application.Services.Patrons
{
    public interface IPatronService
    {
        Task<PagedResult<PatronResponseModel>> GetAll(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<PatronResponseModel> Get(int id, CancellationToken cancellationToken);
        Task<List<BookResponseModel>> GetAllBooksbyId(int patronId, CancellationToken cancellationToken);
        Task<PatronResponseModel> Create(PatronRequestModel patron, CancellationToken cancellationToken);
        Task Update(PatronRequestModel patron, CancellationToken cancellationToken);
        Task Delete(int id, CancellationToken cancellationToken);
    }
}

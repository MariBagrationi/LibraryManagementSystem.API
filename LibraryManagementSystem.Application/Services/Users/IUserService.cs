using LibraryManagementSystem.Application.Models.User;

namespace LibraryManagementSystem.Application.Services.Users
{
    public interface IUserService
    {
        Task<UserRegisterModel> AuthenticationAsync(string username, string password, CancellationToken cancellationToken);
        Task<UserRegisterModel> AuthenticationByTokenAsync(string token, CancellationToken cancellationToken);
        Task<string> CreateAsync(UserRegisterModel user, CancellationToken cancellationToken);      
    }
}

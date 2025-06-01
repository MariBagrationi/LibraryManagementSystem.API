using LibraryManagementSystem.Application.Exceptions.UserExceptions;
using LibraryManagementSystem.Application.Models.User;
using LibraryManagementSystem.Application.Repositories;
using LibraryManagementSystem.Domain.Models;
using Mapster;
using System.Security.Cryptography;
using System.Text;

namespace LibraryManagementSystem.Application.Services.Users
{
    public class UserService : IUserService
    {
        const string SECRET_KEY = "Tango_Is_A_Great_Dance";
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserRegisterModel> AuthenticationAsync(string username, string password, CancellationToken cancellationToken)
        {
            var result = await _repository.GetAsync(username, cancellationToken);
            if (result == null)
                throw new Exception("username or password is incorrect");

            var pass = GenerateHash(password);
            if (result.Password != pass)
                throw new Exception("password is incorrect");

            return result.Adapt<UserRegisterModel>();
        }

        public async Task<string> CreateAsync(UserRegisterModel userModel, CancellationToken cancellationToken)
        {
            var exists = await _repository.Exists(userModel.Username, cancellationToken );

            if (exists)
            {
                throw new  UserAreadyExistsEx("user already exists");
            }

            var user = userModel.Adapt<User>();
            user.Password = GenerateHash(user.Password!);
            var result = await _repository.CreateAsync(user, cancellationToken);

            return result;
        }

        private string GenerateHash(string input)
        {
            using (SHA512 sha = SHA512.Create())
            {
                byte[] bytes = Encoding.ASCII.GetBytes(input + SECRET_KEY);
                byte[] hashBytes = sha.ComputeHash(bytes);

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }

        public async Task<UserRegisterModel> AuthenticationByTokenAsync(string token, CancellationToken cancellationToken)
        {
            var result = await _repository.GetByTokenAsync(token, cancellationToken);

            if (result == null)
                throw new Exception("token has expired");

            return result.Adapt<UserRegisterModel>();
        }

    }
}

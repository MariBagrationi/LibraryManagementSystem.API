
namespace LibraryManagementSystem.Application.Exceptions.AuthorExceptions
{
    public class AuthorDoesNotExistEx : Exception
    {
        public static string Code { get; private set; } = "AuthorDoesNotExist";
        public AuthorDoesNotExistEx(string message) : base(message) { }
    }
}

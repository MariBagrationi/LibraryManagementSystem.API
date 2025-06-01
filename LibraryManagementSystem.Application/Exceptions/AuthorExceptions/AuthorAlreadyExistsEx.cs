
namespace LibraryManagementSystem.Application.Exceptions.AuthorExceptions
{
    public class AuthorAlreadyExistsEx : Exception
    {
        public static string Code { get; private set; } = "AuthorAlreadyExists";
        public AuthorAlreadyExistsEx(string message) : base(message) { }
    }
}

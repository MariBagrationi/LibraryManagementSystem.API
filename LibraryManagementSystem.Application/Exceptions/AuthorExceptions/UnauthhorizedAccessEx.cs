
namespace LibraryManagementSystem.Application.Exceptions.AuthorExceptions
{
    public class UnauthorizedAccessEx : Exception
    {
        public static string Code = "AnauthorizedAccess";
        public UnauthorizedAccessEx(string message) : base(message) { }
    }
}

namespace LibraryManagementSystem.Application.Models.User
{
    public class UserRegisterModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}


namespace LibraryManagementSystem.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public Role Role { get; set; }
        public string? Token { get; set; } 
        public DateTime TokenExpiration { get; set; } 
    }

    public enum Role
    {
        User,
        Admin
    }
}

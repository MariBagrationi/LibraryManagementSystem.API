namespace LibraryManagementSystem.Application.Models.Requests
{
    public class AuthorRequestModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        //public ICollection<BookRequestModel> books { get; set; }
    }
}

namespace LibraryManagementSystem.Application.Models.Requests
{
    public class PatronRequestModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime MembershipDate { get; set; }
    }
}

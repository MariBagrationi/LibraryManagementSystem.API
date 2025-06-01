
namespace LibraryManagementSystem.Domain.Models
{
    public class Patron
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime MembershipDate { get; set; }
        public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();

    }
}

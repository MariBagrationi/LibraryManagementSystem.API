namespace LibraryManagementSystem.Application.Models.Requests
{
    public class BookRequestModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public int PublicationYear { get; set; }
        public string Description { get; set; } = string.Empty;
        public string CoverImageUrl { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int AuthorId { get; set; }
    }
}

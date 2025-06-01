namespace LibraryManagementSystem.API.Infrastructure.Auth.JWT
{
    public class JWTConfig
    {
        public string? Secret { get; set; }
        public int ExpirationInMinutes { get; set; }
    }
}

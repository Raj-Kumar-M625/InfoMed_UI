namespace InfoMed.Models
{
    public class LoginResponse
    {
        public string? Token { get; set; }
        public DateTime? TokenExpiration { get; set; }
        public DateTime LoginTime { get; set; }
        public string? UserId { get; set; }
        public string? UserRole { get; set; }
        public string? UserEmail { get; set; }
        public string? UserName { get; set; }
    }
}

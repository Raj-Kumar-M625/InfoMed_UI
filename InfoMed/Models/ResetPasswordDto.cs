namespace InfoMed.Models
{
    public class ResetPasswordDto
    {
        public string Email { get; set; }
        public string? OldPassword { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}

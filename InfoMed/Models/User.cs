
namespace InfoMed.Models
{
    public class User
    {
        public int IdUser { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public string Role { get; set; }
        public bool Status { get; set; }
        public string? Password { get; set; }
    }
}

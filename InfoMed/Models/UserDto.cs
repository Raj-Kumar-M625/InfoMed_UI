using System.ComponentModel.DataAnnotations;

namespace InfoMed.Models
{
    public class UserDto
    {
        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

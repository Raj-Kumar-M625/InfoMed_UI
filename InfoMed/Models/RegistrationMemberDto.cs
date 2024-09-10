using System.ComponentModel.DataAnnotations.Schema;

namespace InfoMed.Models
{
    public class RegistrationMemberDto
    {
        public int? IdRegistrationMember { get; set; }
        public int? IdRegistration { get; set; }
        public string MemberName { get; set; }
        public string EmailID { get; set; }
        public string? MobileNumber { get; set; }
    }
}

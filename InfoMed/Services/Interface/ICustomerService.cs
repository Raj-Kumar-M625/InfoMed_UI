using InfoMed.Models;

namespace InfoMed.Services.Interface
{
    public interface ICustomerService
    {
        public Task<List<RegistrationMemberDto>> GetRegistrationMembers(int id);
        public Task<RegistrationDto> AddRegistrationMembers(RegistrationDto registrationDto);
        public Task<RegistrationDto> GetRegistrationMembersByEmail(string email, int idEvent);
    }
}

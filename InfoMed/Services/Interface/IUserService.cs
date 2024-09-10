using InfoMed.Models;
using Microsoft.AspNetCore.Mvc;

namespace InfoMed.Services.Interface
{
    public interface IUserService
    {
        public Task<bool> AddUser(User user);
        public Task<List<User>> GetUsers();
        public Task<User> GetUsersbyId(int userId);
        public Task<User> UpdateUser(User user);
        public Task<List<UserRole>> GetUserRoles();
    }
}

using InfoMed.Models;

namespace InfoMed.Services.Interface
{
    public interface IAccountService
    {
        Task<bool> Login(UserDto user, HttpContext httpContext);
        Task<APIResponse> ResetPassword(ResetPasswordDto resetPasswordDto);
    }
}

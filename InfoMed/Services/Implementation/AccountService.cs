using InfoMed.Models;
using InfoMed.Services.Interface;
using InfoMed.Utils;
using log4net;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

namespace InfoMed.Services.Implementation
{
    public class AccountService : IAccountService
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IApiService _apiService;
        private readonly Session _session;

        public AccountService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IApiService apiService, Session sessions)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _apiService = apiService;
            _session = sessions;
        }

        public async Task<bool> Login(UserDto user, HttpContext httpContext)
        {
            try
            {
                var userAgent = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString();
                var url = _configuration.GetValue<string>("BaseURL") + Constants.Login;
                using HttpClient client = new();
                client.DefaultRequestHeaders.Add("User-Agent", userAgent);
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, Constants.appJson);
                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var loginResponse = await response.Content.ReadAsStringAsync();
                    var tokenObject = JsonConvert.DeserializeObject<LoginResponse>(loginResponse);

                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = false,
                        Expires = tokenObject!.TokenExpiration,
                        Secure = false, 
                        SameSite = SameSiteMode.Strict
                    };
                    httpContext.Response.Cookies.Append("JWTToken", tokenObject.Token!, cookieOptions);
                    _session.SetUserDetails(tokenObject);
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                _log.Info("Inside Login() in AccountService.cs");
                _log.Info(ex.Message);
                return false;
            }
        }

        public async Task<APIResponse> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            try
            {
                var userAgent = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString();
                var url = _configuration.GetValue<string>("BaseURL") + Constants.ResetPassword;
                using HttpClient client = new();
                client.DefaultRequestHeaders.Add("User-Agent", userAgent);
                var json = JsonConvert.SerializeObject(resetPasswordDto);
                var content = new StringContent(json, Encoding.UTF8, Constants.appJson);
                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var loginResponse = await response.Content.ReadAsStringAsync();
                    var tokenObject = JsonConvert.DeserializeObject<APIResponse>(loginResponse);
                    return tokenObject;
                }
                return null;
            }
            catch (Exception ex)
            {
                _log.Info("Inside ResetPassword() in AccountService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }
    }
}

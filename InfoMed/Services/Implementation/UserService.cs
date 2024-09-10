using InfoMed.Models;
using InfoMed.Services.Interface;
using InfoMed.Utils;
using log4net;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

namespace InfoMed.Services.Implementation
{
    public class UserService : IUserService
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        private readonly IApiService _apiService;
        private readonly IConfiguration _configuration;

        public UserService(IApiService apiService, IConfiguration configuration)
        {
            _apiService = apiService;
            _configuration = configuration;
        }

        public async Task<bool> AddUser(User user)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.Register;
                var jsonString = JsonConvert.SerializeObject(user);
                var content = new StringContent(jsonString, Encoding.UTF8, Constants.appJson);
                var response = await _apiService.SendRequestAsync(HttpMethod.Post, url, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _log.Info("Inside AddUser() in UserService.cs");
                _log.Info(ex.Message);
                return false;
            }
        }

        public async Task<List<UserRole>> GetUserRoles()
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.GetUserRoles;
                var response = await _apiService.SendRequestAsync(HttpMethod.Get, url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<List<UserRole>>(result);
                }
                return null!;
            }
            catch (Exception ex)
            {
                _log.Info("Inside GetUserRoles() in UserService.cs");
                _log.Info(ex.Message);
                return null!;
            }
        }

        public async Task<List<User>> GetUsers()
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.GetUsers;
                var response = await _apiService.SendRequestAsync(HttpMethod.Get, url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<List<User>>(result);
                }
                return null;
            }
            catch (Exception ex)
            {
                _log.Info("Inside GetUsers() in UserService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }

        public async Task<User> GetUsersbyId(int userId)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.GetUsersbyId + $@"?userId={userId}";
                var response = await _apiService.SendRequestAsync(HttpMethod.Get, url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<User>(result)!;
                }
                return null!;
            }
            catch (Exception ex)
            {
                _log.Info("Inside GetUsersbyId() in UserService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }

        public async Task<User> UpdateUser(User user)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.UpdateUser;
                var jsonString = JsonConvert.SerializeObject(user);
                var content = new StringContent(jsonString, Encoding.UTF8, Constants.appJson);
                var response = await _apiService.SendRequestAsync(HttpMethod.Post, url, content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<User>(result);
                }
                return null;
            }
            catch (Exception ex)
            {
                _log.Info("Inside UpdateUser() in UserService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }
    }
}

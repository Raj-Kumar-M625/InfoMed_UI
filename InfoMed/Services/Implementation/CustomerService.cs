using InfoMed.Models;
using InfoMed.Services.Interface;
using InfoMed.Utils;
using log4net;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Text;

namespace InfoMed.Services.Implementation
{
    public class CustomerService : ICustomerService
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        private readonly IApiService _apiService;
        private readonly IConfiguration _configuration;

        public CustomerService(IApiService apiService, IConfiguration configuration)
        {
            _apiService = apiService;
            _configuration = configuration;
        }

        public async Task<RegistrationDto> AddRegistrationMembers(RegistrationDto registrationDto)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.AddRegistrationMembers;
                var jsonString = JsonConvert.SerializeObject(registrationDto);
                var content = new StringContent(jsonString, Encoding.UTF8, Constants.appJson);
                var response = await _apiService.SendRequestAsync(HttpMethod.Post, url, content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<RegistrationDto>(result);
                }
                return null;
            }
            catch (Exception ex)
            {
                _log.Info("Inside AddRegistrationMembers() in CustomerService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }

        public async Task<List<RegistrationMemberDto>> GetRegistrationMembers(int id)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.GetRegistrationMembers + $@"?id={id}";
                var response = await _apiService.SendRequestAsync(HttpMethod.Get, url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<List<RegistrationMemberDto>>(result)!;
                }
                return null!;
            }
            catch (Exception ex)
            {
                _log.Info("Inside GetRegistrationMembers() in CustomerService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }

        public async Task<RegistrationDto> GetRegistrationMembersByEmail(string email,int idEvent)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.GetRegistrationMembersByEmail + $@"?email={email}&&idEvent={idEvent}";
                var response = await _apiService.SendRequestAsync(HttpMethod.Get, url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<RegistrationDto>(result)!;
                }
                return null!;
            }
            catch (Exception ex)
            {
                _log.Info("Inside GetRegistrationMembersByEmail() in CustomerService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }
    }
}

using InfoMed.Models;
using InfoMed.Services.Interface;
using InfoMed.Utils;
using log4net;
using Newtonsoft.Json;
using System.Reflection;

namespace InfoMed.Services.Implementation
{
    public class CommonService : ICommonService
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        private readonly IApiService _apiService;
        private readonly IConfiguration _configuration;

        public CommonService(IApiService apiService, IConfiguration configuration)
        {
            _apiService = apiService;
            _configuration = configuration;
        }

        public async Task<List<Country>> GetCountries()
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.GetCountries;
                var response = await _apiService.SendRequestAsync(HttpMethod.Get, url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<List<Country>>(result);
                }
                return null;
            }
            catch (Exception ex)
            {
                _log.Info("Inside GetCountries() in CommonService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }
    }
}

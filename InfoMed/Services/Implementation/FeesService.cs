using InfoMed.Models;
using InfoMed.Services.Interface;
using InfoMed.Utils;
using log4net;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

namespace InfoMed.Services.Implementation
{
    public class FeesService : IFeesService
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        private readonly IApiService _apiService;
        private readonly IConfiguration _configuration;

        public FeesService(IApiService apiService, IConfiguration configuration)
        {
            _apiService = apiService;
            _configuration = configuration;
        }
      
       
        public async Task<List<ConferenceFeeDto>> GetConferenceFeesList(int id, int idVersion)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.GetConferenceFeesList + $"?id={id}&idVersion={idVersion}";                
                var response = await _apiService.SendRequestAsync(HttpMethod.Get, url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<List<ConferenceFeeDto>>(result)!;
                }
                return null!;
            }
            catch (Exception ex)
            {
                _log.Info("Inside GetConferenceFeesList() in TextContentAreasService.cs");
                _log.Info(ex.Message);
                return null!;
            }
        }


        public async Task<ConferenceFeeDto> AddFees(ConferenceFeeDto feesMasterDto)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.AddFees;
                var jsonString = JsonConvert.SerializeObject(feesMasterDto);
                var content = new StringContent(jsonString, Encoding.UTF8, Constants.appJson);
                var response = await _apiService.SendRequestAsync(HttpMethod.Post, url, content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<ConferenceFeeDto>(result);
                }
                return null;
            }
            catch (Exception ex)
            {
                _log.Info("Inside AddFees() in FeesService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }

        public async Task<ConferenceFeeDto> GetFeesById(int idFeesMaster)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.GetFeesById + $@"?idFeesMaster={idFeesMaster}";
                var response = await _apiService.SendRequestAsync(HttpMethod.Get, url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<ConferenceFeeDto>(result)!;
                }
                return null!;
            }
            catch (Exception ex)
            {
                _log.Info("Inside GetFeesById() in FeesService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }


        public async Task<ConferenceFeeDto> UpdateFees(ConferenceFeeDto feesMasterDto)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.UpdateFees;
                var jsonString = JsonConvert.SerializeObject(feesMasterDto);
                var content = new StringContent(jsonString, Encoding.UTF8, Constants.appJson);
                var response = await _apiService.SendRequestAsync(HttpMethod.Post, url, content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<ConferenceFeeDto>(result);
                }
                return null;
            }
            catch (Exception ex)
            {
                _log.Info("Inside UpdateFees() in FeesService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }


    }
}

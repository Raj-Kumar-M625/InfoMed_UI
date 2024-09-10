using InfoMed.Models;
using InfoMed.Services.Interface;
using InfoMed.Utils;
using log4net;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

namespace InfoMed.Services.Implementation
{
    public class SchedulerService : ISchedulerService
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        private readonly IApiService _apiService;
        private readonly IConfiguration _configuration;

        public SchedulerService(IApiService apiService, IConfiguration configuration)
        {
            _apiService = apiService;
            _configuration = configuration;
        }

        public async Task<ScheduleDetailsDto> AddScheduleDetails(ScheduleDetailsDto ScheduleDetailsDto)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.AddScheduleDetails;
                var jsonString = JsonConvert.SerializeObject(ScheduleDetailsDto);
                var content = new StringContent(jsonString, Encoding.UTF8, Constants.appJson);
                var response = await _apiService.SendRequestAsync(HttpMethod.Post, url, content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<ScheduleDetailsDto>(result);
                }
                return null;
            }
            catch (Exception ex)
            {
                _log.Info("Inside AddScheduleDetails() in SchedulerService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }

        public async Task<ScheduleMasterDto> AddScheduleMaster(ScheduleMasterDto ScheduleMasterDto)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.AddScheduleMaster;
                var jsonString = JsonConvert.SerializeObject(ScheduleMasterDto);
                var content = new StringContent(jsonString, Encoding.UTF8, Constants.appJson);
                var response = await _apiService.SendRequestAsync(HttpMethod.Post, url, content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<ScheduleMasterDto>(result);
                }
                return null;
            }
            catch (Exception ex)
            {
                _log.Info("Inside AddScheduleDetails() in SchedulerService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }

        public async Task<ScheduleDetailsDto> GetScheduleDetailById(int idScheduleDetail)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.GetScheduleDetailById + $@"?idScheduleDetails={idScheduleDetail}";
                var response = await _apiService.SendRequestAsync(HttpMethod.Get, url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<ScheduleDetailsDto>(result)!;
                }
                return null!;
            }
            catch (Exception ex)
            {
                _log.Info("Inside GetScheduleDetailById() in SchedulerService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }

        public async Task<ScheduleMasterDto> GetScheduleMasterById(int idScheduleMaster)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.GetScheduleMasterById + $@"?idScheduleMaster={idScheduleMaster}";
                var response = await _apiService.SendRequestAsync(HttpMethod.Get, url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<ScheduleMasterDto>(result)!;
                }
                return null!;
            }
            catch (Exception ex)
            {
                _log.Info("Inside GetScheduleMasterById() in SchedulerService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }

        public async Task<List<ScheduleDetailsDto>> GetSchedulesDetails(int IdScheduleMaster)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.GetSchedulesDetails + $@"?IdScheduleMaster={IdScheduleMaster}";
                var response = await _apiService.SendRequestAsync(HttpMethod.Get, url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<List<ScheduleDetailsDto>>(result);
                }
                return null;
            }
            catch (Exception ex)
            {
                _log.Info("Inside GetSchedulesDetails() in SchedulerService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }

        public async Task<List<ScheduleMasterDto>> GetSchedulesMaster(int id, int idVersion)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.GetSchedulesMaster + $"?id={id}&idVersion={idVersion}";                
                var response = await _apiService.SendRequestAsync(HttpMethod.Get, url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<List<ScheduleMasterDto>>(result);
                }
                return null;
            }
            catch (Exception ex)
            {
                _log.Info("Inside GetEvents() in SchedulerService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }

        public async Task<ScheduleDetailsDto> UpdateScheduleDetails(ScheduleDetailsDto scheduleDetailsDto)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.UpdateScheduleDetails;
                var jsonString = JsonConvert.SerializeObject(scheduleDetailsDto);
                var content = new StringContent(jsonString, Encoding.UTF8, Constants.appJson);
                var response = await _apiService.SendRequestAsync(HttpMethod.Post, url, content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<ScheduleDetailsDto>(result);
                }
                return null;
            }
            catch (Exception ex)
            {
                _log.Info("Inside UpdateScheduleDetails() in SchedulerService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }

        public async Task<ScheduleMasterDto> UpdateScheduleMaster(ScheduleMasterDto scheduleMasterDto)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.UpdateScheduleMaster;
                var jsonString = JsonConvert.SerializeObject(scheduleMasterDto);
                var content = new StringContent(jsonString, Encoding.UTF8, Constants.appJson);
                var response = await _apiService.SendRequestAsync(HttpMethod.Post, url, content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<ScheduleMasterDto>(result);
                }
                return null!;
            }
            catch (Exception ex)
            {
                _log.Info("Inside UpdateTextContent() in TextContentAreasService.cs");
                _log.Info(ex.Message);
                return null!;
            }
        }
    }
}

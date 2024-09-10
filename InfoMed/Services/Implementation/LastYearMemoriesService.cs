using InfoMed.Models;
using InfoMed.Services.Interface;
using InfoMed.Utils;
using log4net;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

namespace InfoMed.Services.Implementation
{
    public class LastYearMemoriesService : ILastYearMemoriesService
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        private readonly IApiService _apiService;
        private readonly IConfiguration _configuration;

        public LastYearMemoriesService(IApiService apiService, IConfiguration configuration)
        {
            _apiService = apiService;
            _configuration = configuration;
        }
       

        public async Task<LastYearMemoryDto> AddLastYearMemories(LastYearMemoryDto lastYeatMemoryDto)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.AddLastYearMemories;
                var jsonString = JsonConvert.SerializeObject(lastYeatMemoryDto);
                var content = new StringContent(jsonString, Encoding.UTF8, Constants.appJson);
                var response = await _apiService.SendRequestAsync(HttpMethod.Post, url, content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<LastYearMemoryDto>(result);
                }
                return null;
            }
            catch (Exception ex)
            {
                _log.Info("Inside AddLastYearMemories() in LastYearMemoriesService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }

        public async Task<List<LastYearMemoryDto>> GetLastYearMemoriesList(int id, int idVersion)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.GetLastYearMemoriesList + $"?id={id}&idVersion={idVersion}";               
                var response = await _apiService.SendRequestAsync(HttpMethod.Get, url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<List<LastYearMemoryDto>>(result)!;
                }
                return null!;
            }
            catch (Exception ex)
            {
                _log.Info("Inside GetLastYearMemoriesList() in LastYearMemoriesService.cs");
                _log.Info(ex.Message);
                return null!;
            }
        }

        public async Task<LastYearMemoryDto> GetLastYearMemoriesById(int idLastYearMemory)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.GetLastYearMemoriesById + $@"?idLastYearMemory={idLastYearMemory}";
                var response = await _apiService.SendRequestAsync(HttpMethod.Get, url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<LastYearMemoryDto>(result)!;
                }
                return null!;
            }
            catch (Exception ex)
            {
                _log.Info("Inside GetLastYearMemoriesById() in LastYearMemoriesService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }

        public async Task<LastYearMemoryDto> UpdateLastYearMemories(LastYearMemoryDto lastYeatMemoryDto)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.UpdateLastYearMemories;
                var jsonString = JsonConvert.SerializeObject(lastYeatMemoryDto);
                var content = new StringContent(jsonString, Encoding.UTF8, Constants.appJson);
                var response = await _apiService.SendRequestAsync(HttpMethod.Post, url, content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<LastYearMemoryDto>(result);
                }
                return null;
            }
            catch (Exception ex)
            {
                _log.Info("Inside UpdateLastYearMemories() in LastYearMemoriesService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }
    }
}

using InfoMed.Models;
using InfoMed.Services.Interface;
using InfoMed.Utils;
using log4net;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

namespace InfoMed.Services.Implementation
{
    public class TextContentAreasService : ITextContentAreasService
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        private readonly IApiService _apiService;
        private readonly IConfiguration _configuration;

        public TextContentAreasService(IApiService apiService, IConfiguration configuration)
        {
            _apiService = apiService;
            _configuration = configuration;
        }

        public async Task<TextContentAreasDto> AddTextContent(TextContentAreasDto textContent)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.AddTextContent;
                var jsonString = JsonConvert.SerializeObject(textContent);
                var content = new StringContent(jsonString, Encoding.UTF8, Constants.appJson);
                var response = await _apiService.SendRequestAsync(HttpMethod.Post, url, content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<TextContentAreasDto>(result)!;
                }
                return null!;
            }
            catch (Exception ex)
            {
                _log.Info("Inside AddTextContent() in TextContentAreasService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }

        public async Task<List<TextContentAreasDto>> GetTextContentByEventVersionId(int id, int idVersion)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.GetTextContentByEventVersionId + $"?id={id}&idVersion={idVersion}";
                 var response = await _apiService.SendRequestAsync(HttpMethod.Get, url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<List<TextContentAreasDto>>(result)!;
                }
                return null!;
            }
            catch (Exception ex)
            {
                _log.Info("Inside GetTextContentByEventVersionId() in TextContentAreasService.cs");
                _log.Info(ex.Message);
                return null!;
            }
        }

        public async Task<TextContentAreasDto> GetTextContentById(int id)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.GetTextContentById + $@"?id={id}";
                var response = await _apiService.SendRequestAsync(HttpMethod.Get, url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<TextContentAreasDto>(result)!;
                }
                return null!;
            }
            catch (Exception ex)
            {
                _log.Info("Inside GetTextContentById() in TextContentAreasService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }

        public async Task<List<TextContentAreasDto>> GetTextContents()
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.GetTextContents;
                var response = await _apiService.SendRequestAsync(HttpMethod.Get, url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<List<TextContentAreasDto>>(result);
                }
                return null;
            }
            catch (Exception ex)
            {
                _log.Info("Inside GetTextContents() in TextContentAreasService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }

        public async Task<TextContentAreasDto> UpdateTextContent(TextContentAreasDto textContent)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.UpdateTextContent;
                var jsonString = JsonConvert.SerializeObject(textContent);
                var content = new StringContent(jsonString, Encoding.UTF8, Constants.appJson);
                var response = await _apiService.SendRequestAsync(HttpMethod.Post, url, content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<TextContentAreasDto>(result);
                }
                return null;
            }
            catch (Exception ex)
            {
                _log.Info("Inside UpdateTextContent() in TextContentAreasService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }
    }
}

using InfoMed.Models;
using InfoMed.Services.Interface;
using InfoMed.Utils;
using log4net;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

namespace InfoMed.Services.Implementation
{
    public class EventService : IEventService
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        private readonly IApiService _apiService;
        private readonly IConfiguration _configuration;

        public EventService(IApiService apiService, IConfiguration configuration)
        {
            _apiService = apiService;
            _configuration = configuration;
        }

        public async Task<EventVersionDto> AddEvent(EventVersionDto _event)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.AddEvent;
                var jsonString = JsonConvert.SerializeObject(_event);
                var content = new StringContent(jsonString, Encoding.UTF8, Constants.appJson);
                var response = await _apiService.SendRequestAsync(HttpMethod.Post, url, content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<EventVersionDto>(result);
                }
                return null;
            }
            catch (Exception ex)
            {
                _log.Info("Inside AddEvent() in EventService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }

        public async Task<EventVersionDto> UpdateEvent(EventVersionDto _event)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.UpdateEvent;
                var jsonString = JsonConvert.SerializeObject(_event);
                var content = new StringContent(jsonString, Encoding.UTF8, Constants.appJson);
                var response = await _apiService.SendRequestAsync(HttpMethod.Post, url, content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<EventVersionDto>(result);
                }
                return null;
            }
            catch (Exception ex)
            {
                _log.Info("Inside UpdateEvent() in EventService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }

        public async Task<List<EventVersionDto>> GetEvents(string version)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.GetEvents + version;
                var response = await _apiService.SendRequestAsync(HttpMethod.Get, url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<List<EventVersionDto>>(result);
                }
                return null;
            }
            catch (Exception ex)
            {
                _log.Info("Inside GetEvents() in EventService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }

        public async Task<EventVersionDto> GetEventById(int id, int idVersion)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.GetEventById + $"?id={id}&idVersion={idVersion}";
                var response = await _apiService.SendRequestAsync(HttpMethod.Get, url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<EventVersionDto>(result)!;
                }
                return null!;
            }
            catch (Exception ex)
            {
                _log.Info("Inside GetEventById() in EventService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }

        public async Task<List<EventTypeDto>> GetEventTypes()
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.GetEventTypes;
                var response = await _apiService.SendRequestAsync(HttpMethod.Get, url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<List<EventTypeDto>>(result)!;
                }
                return null!;
            }
            catch (Exception ex)
            {
                _log.Info("Inside GetEventTypes() in EventService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }

        public async Task<List<SponserTypeDto>> GetSponserTypes()
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.GetSponserTypes;
                var response = await _apiService.SendRequestAsync(HttpMethod.Get, url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<List<SponserTypeDto>>(result)!;
                }
                return null!;
            }
            catch (Exception ex)
            {
                _log.Info("Inside GetSponserTypes() in EventService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }


        public async Task<List<SponserDto>> GetSponserList(int id, int idVersion)
        {
            try
            {
                var baseUrl = _configuration.GetValue<string>("BaseURL");
                var endpoint = $"{Constants.GetSponser}?id={id}&idVersion={idVersion}";
                var url = $"{baseUrl}{endpoint}";
                var response = await _apiService.SendRequestAsync(HttpMethod.Get, url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<List<SponserDto>>(result);
                }
                return null;
            }
            catch (Exception ex)
            {
                _log.Info("Inside GetSponserList() in EventService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }

        public async Task<List<SpeakersDto>> GetSpeakerList(int id, int idVersion)
        {
            try
            {
                var baseUrl = _configuration.GetValue<string>("BaseURL");
                var endpoint = $"{Constants.GetSpeakers}?id={id}&idVersion={idVersion}";                
                var url = $"{baseUrl}{endpoint}";
                var response = await _apiService.SendRequestAsync(HttpMethod.Get, url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<List<SpeakersDto>>(result);
                }
                return null;
            }
            catch (Exception ex)
            {
                _log.Info("Inside GetSponserList() in EventService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }
        public async Task<bool> Addsponser(SponserDto _sponser)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.AddSponserPost;
                var jsonString = JsonConvert.SerializeObject(_sponser);
                var content = new StringContent(jsonString, Encoding.UTF8, Constants.appJson);
                var response = await _apiService.SendRequestAsync(HttpMethod.Post, url, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _log.Info("Inside Addsponser() in EventService.cs");
                _log.Info(ex.Message);
                return false;
            }
        }

        public async Task<bool> Updatesponser(SponserDto _sponser)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.UpdateSponserPost;
                var jsonString = JsonConvert.SerializeObject(_sponser);
                var content = new StringContent(jsonString, Encoding.UTF8, Constants.appJson);
                var response = await _apiService.SendRequestAsync(HttpMethod.Post, url, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _log.Info("Inside Updatesponser() in EventService.cs");
                _log.Info(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteSponsor(int Id)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.DeleteSponsor + $@"?id={Id}";
                var response = await _apiService.SendRequestAsync(HttpMethod.Get, url);
                return response.IsSuccessStatusCode;

            }
            catch (Exception ex)
            {
                _log.Info("Inside DeleteSponsor() in EventService.cs");
                _log.Info(ex.Message);
                return false;
            }
        }

        public async Task<SponserDto> GetSponserById(int id)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.GetSponserById + $@"?id={id}";
                var response = await _apiService.SendRequestAsync(HttpMethod.Get, url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<SponserDto>(result)!;
                }
                return null!;
            }
            catch (Exception ex)
            {
                _log.Info("Inside GetSponserById() in EventService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }

        public async Task<bool> AddSpeaker(SpeakersDto speakers)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.AddSpeakerPost;
                var jsonString = JsonConvert.SerializeObject(speakers);
                var content = new StringContent(jsonString, Encoding.UTF8, Constants.appJson);
                var response = await _apiService.SendRequestAsync(HttpMethod.Post, url, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _log.Info("Inside AddSpeaker() in EventService.cs");
                _log.Info(ex.Message);
                return false;
            }
        }

        public async Task<SpeakersDto> GetSpeakerById(int id)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.GetSpeakerById + $@"?id={id}";
                var response = await _apiService.SendRequestAsync(HttpMethod.Get, url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<SpeakersDto>(result)!;
                }
                return null!;
            }
            catch (Exception ex)
            {
                _log.Info("Inside GetSpeakerById() in EventService.cs");
                _log.Info(ex.Message);
                return null;
            }
        }

        public async Task<bool> Updatespeaker(SpeakersDto _speaker)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.UpdateSpeakerPost;
                var jsonString = JsonConvert.SerializeObject(_speaker);
                var content = new StringContent(jsonString, Encoding.UTF8, Constants.appJson);
                var response = await _apiService.SendRequestAsync(HttpMethod.Post, url, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _log.Info("Inside Updatespeaker() in EventService.cs");
                _log.Info(ex.Message);
                return false;
            }
        }

        public async Task<int> EventVersionCreate(int id)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.EventVersionCreate + $@"?id={id}";
                var response = await _apiService.SendRequestAsync(HttpMethod.Post, url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<int>(result)!;
                }
                return 0!;
            }
            catch (Exception ex)
            {
                _log.Info("Inside EventVersionCreate() in EventService.cs");
                _log.Info(ex.Message);
                return 0;
            }
        } 

        public async Task<bool> IsEventNameUnique(EventVersionDto _event)
        {
            var events = await GetEvents(Constants.AllVersion);
            var isUnique = events.FirstOrDefault(e => e.EventName.Replace(" ", "").ToLower().Trim() == _event.EventName.Replace(" ", "").ToLower().Trim() && _event.IdEvent != e.IdEvent);
            return isUnique == null ? true : false;
        }

        public async Task<EventVersionDto> GetEventByWebPageName(string webPageName)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.GetEventByWebPageName + $@"?webPageName={webPageName}";
                var response = await _apiService.SendRequestAsync(HttpMethod.Get, url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<EventVersionDto>(result)!;
                }
                return null!;
            }
            catch (Exception ex)
            {
                _log.Info("Inside GetEventByWebPageName() in EventService.cs");
                _log.Info(ex.Message);
                return null!;
            }
        }

        public async Task<EventViewModel> GetEventDetails(string webPageName)
        {
            try
            {
                var url = _configuration.GetValue<string>("BaseURL") + Constants.GetEventDetails + $@"?webPageName={webPageName}";
                var response = await _apiService.SendRequestAsync(HttpMethod.Get, url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<EventViewModel>(result);
                }
                return null!;
            }
            catch (Exception ex)
            {
                _log.Info("Inside GetEventDetails() in EventService.cs");
                _log.Info(ex.Message);
                return null!;
            }
        }
    }
}

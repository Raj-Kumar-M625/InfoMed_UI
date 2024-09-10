using InfoMed.Models;

namespace InfoMed.Services.Interface
{
    public interface IEventService
    {
        public Task<List<EventVersionDto>> GetEvents(string version);
        public Task<EventViewModel> GetEventDetails(string webPageName);
        public Task<EventVersionDto> AddEvent(EventVersionDto _event);
        public Task<EventVersionDto> UpdateEvent(EventVersionDto _event);
        public Task<EventVersionDto> GetEventById(int id, int idVersion);
        public Task<EventVersionDto> GetEventByWebPageName(string webPageName);
        Task<List<EventTypeDto>> GetEventTypes();
        Task<List<SponserTypeDto>> GetSponserTypes();
        public Task<List<SponserDto>> GetSponserList(int id, int idVersion);
        public Task<List<SpeakersDto>> GetSpeakerList(int id, int idVersion);
        public Task<bool> Addsponser(SponserDto _sponser);
        public Task<bool> Updatesponser(SponserDto _sponser);
        public Task<SponserDto> GetSponserById(int id);
        public Task<bool> DeleteSponsor(int id);
        public Task<bool> IsEventNameUnique(EventVersionDto _event);
        public Task<bool> AddSpeaker(SpeakersDto _sponser);
        public Task<SpeakersDto> GetSpeakerById(int id);
        public Task<bool> Updatespeaker(SpeakersDto _sponser);
        public Task<int> EventVersionCreate(int id);

    }
}

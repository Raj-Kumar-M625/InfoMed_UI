using InfoMed.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace InfoMed.Utils
{
    public class Session
    {
        private readonly ISession _session;
        const string SessionUser = "SessionUser";
        const string IdEvent = "IdEvent";
        const string EventVersion = "EventVersion";
        const string EventStatus = "EventStatus";
        const string OTP = "OTP";
        const string Version = "Version";
        const string Email = "Email";
        const string Event = "Event";

        public Session(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext!.Session;
        }

        public void SetUserDetails(LoginResponse userData)
        {
            _session.SetString(SessionUser, JsonConvert.SerializeObject(userData));
        }

        public LoginResponse GetUserDetails()
        {
            if (!string.IsNullOrEmpty(_session.GetString(SessionUser)))
                return JsonConvert.DeserializeObject<LoginResponse>(_session.GetString(SessionUser)!)!;
            return null!;
        }

        public void RemoveUserData()
        {
            _session.Remove(SessionUser);
        }

        public void SetEventId(int eventId)
        {
            _session.SetString(IdEvent, eventId.ToString());
        }

        public int GetEventId()
        {
            if (!string.IsNullOrEmpty(_session.GetString(IdEvent)))
                return int.Parse(_session.GetString(IdEvent)!);
            return 0;
        }

        public void SetEventVersion(int version)
        {
            _session.SetString(EventVersion, version.ToString());
        }

        public int GetEventVersion()
        {
            if (!string.IsNullOrEmpty(_session.GetString(EventVersion)))
                return int.Parse(_session.GetString(EventVersion)!);
            return 0;
        }

        public void SetVersion(int version)
        {
            _session.SetString(Version, version.ToString());
        }

        public int GetVersion()
        {
            if (!string.IsNullOrEmpty(_session.GetString(Version)))
                return int.Parse(_session.GetString(Version)!);
            return 0;
        }

        public void SetEventStatus(string eventStatus)
        {
            _session.SetString(EventStatus, eventStatus.ToString());
        }

        public string GetEventStatus()
        {
            if (!string.IsNullOrEmpty(_session.GetString(EventStatus)))
                return (_session.GetString(EventStatus)!);
            return null;
        }

        public void SetOTP(int otp)
        {
            _session.SetString(OTP, otp.ToString());
        }

        public int GetOTP()
        {
            if (!string.IsNullOrEmpty(_session.GetString(OTP)))
                return int.Parse(_session.GetString(OTP)!);
            return 0;
        }

        public void RemoveOTP()
        {
            _session.Remove(OTP);
        }
        public void SetEmail(string email)
        {
            _session.SetString(Email, email.ToString());
        }

        public string GetEmail()
        {
            if (!string.IsNullOrEmpty(_session.GetString(Email)))
                return (_session.GetString(Email)!);
            return null;
        }

        public void SetEvent(EventVersionDto eventDto)
        {
            _session.SetString(Event, JsonConvert.SerializeObject(eventDto));
        }

        public EventVersionDto GetEvent()
        {
            if (!string.IsNullOrEmpty(_session.GetString(Event)))
                return JsonConvert.DeserializeObject<EventVersionDto>(_session.GetString(Event)!)!;
            return null!;
        }
    }
}

namespace InfoMed.Utils
{
    public class Constants
    {
        #region Account
        public const string Login = "api/Account/login";
        public const string ResetPassword = "api/Account/ResetPassword";
        #endregion

        #region Event
        public const string GetEvents = "api/Event/GetEvents?version=";
        public const string AddEvent = "api/Event/AddEvent";
        public const string UpdateEvent = "api/Event/UpdateEvent";
        public const string GetEventById = "api/Event/GetEventById"; 
        public const string GetEventTypes = "api/Event/GetEventTypes";
        public const string GetEventByWebPageName = "api/Event/GetEventByWebPageName";
        public const string GetEventDetails = "api/Event/GetEventDetails";
        public const string EventVersionCreate = "api/Event/EventVersionCreate";
        public static string LatestVersion = "LatestVersion";
        public static string AllVersion = "AllVersion";
        #endregion

        #region Tabs
        public const string EventDetails = "EventDetails";
        public const string TextContent = "TextContent";
        public const string Sponsors = "Sponsors";
        public const string Speakers = "Speakers";
        public const string Media = "Media";
        public const string Schedules = "Schedules";
        public const string Fees = "Fees";
        public const string LastYearMemories = "LastYearMemories";
        public const string Payment = "Payment";
        #endregion

        #region Common
        public const string appJson = "application/json";
        public const string Submitted = "Submitted";
        public const string Home = "Home";
        #endregion

        #region SweetAlertIcon
        public const string Success = "success";
        #endregion

        #region TextContentAreas
        public const string GetTextContents = "api/TextContentArea/GetTextContents";
        public const string AddTextContent = "api/TextContentArea/AddTextContent";
        public const string UpdateTextContent = "api/TextContentArea/UpdateTextContent";
        public const string GetTextContentById = "api/TextContentArea/GetTextContentById";
        public const string GetTextContentByEventVersionId = "api/TextContentArea/GetTextContentByEventVersionId";
        #endregion

        #region Sponser
        public const string GetSponser = "api/Event/GetSponser";
        public const string AddSponserPost = "api/Event/AddSponser";
        public const string UpdateSponserPost = "api/Event/UpdateSponser";
        public const string GetSponserById = "api/Event/GetSponserById";
        public const string DeleteSponsor = "api/Event/DeleteSponsor";
        public const string sponserList = "../Shared/_sponserList";
        public const string AddSponser = "../Shared/_AddSponser";
        public const string EditSponser = "../Shared/_EditSponser";
        public const string GetSponserTypes = "api/Event/GetSponserTypes";
        public const string PlatinumSponcers = "PLATINUM SPONSORS";
        public const string GoldSponcers = "GOLD SPONSORS";
        public const string SilverSponcers = "SILVER SPONSORS";
        #endregion

        #region Schedule
        public const string GetSchedulesMaster = "api/Schedule/GetSchedulesMaster";
        public const string GetSchedulesDetails = "api/Schedule/GetSchedulesDetails";
        public const string AddScheduleMaster = "api/Schedule/AddScheduleMaster";
        public const string AddScheduleDetails = "api/Schedule/AddScheduleDetails";
        public const string UpdateScheduleMaster = "api/Schedule/UpdateScheduleMaster";
        public const string UpdateScheduleDetails = "api/Schedule/UpdateScheduleDetails";
        public const string GetScheduleDetailById = "api/Schedule/GetScheduleDetailById";
        public const string GetScheduleMasterById = "api/Schedule/GetScheduleMasterById";
        #endregion

        #region Speaker
        public const string GetSpeakers = "api/Event/GetSpeakers";
        public const string AddSpeaker = "../Shared/_AddSpeaker";
        public const string AddSpeakerPost = "api/Event/AddSpeaker";
        public const string GetSpeakerById = "api/Event/GetSpeakerById";
        public const string EditSpeaker = "../Shared/_EditSpeaker";
        public const string UpdateSpeakerPost = "api/Event/UpdateSpeaker";
        #endregion

        #region Fees
        public const string GetConferenceFeesList = "api/Fees/GetConferenceFeesList";
        public const string AddFees = "api/Fees/AddFees";
        public const string GetFeesById = "api/Fees/GetFeesById"; 
        public const string UpdateFees = "api/Fees/UpdateFees";
        #endregion

        #region User
        public const string GetUsers = "api/User/GetUsers";
        public const string GetUsersbyId = "api/User/GetUsersbyId";
        public const string UpdateUser = "api/User/UpdateUser";
        public const string GetUserRoles = "api/User/GetUserRoles";
        public const string Register = "api/Account/register";
        #endregion

        #region LastYearMemory
        public const string AddLastYearMemories = "api/LastYearMemories/AddLastYearMemories";
        public const string GetLastYearMemoriesList = "api/LastYearMemories/GetLastYearMemoriesList";
        public const string GetLastYearMemoriesById = "api/LastYearMemories/GetLastYearMemoriesById";
        public const string UpdateLastYearMemories = "api/LastYearMemories/UpdateLastYearMemories";
        #endregion

        #region Payment     
        public const string AddPaymentDetails = "api/Payment/AddPaymentDetails";
        public const string GetPaymentDetails = "api/Payment/GetPaymentDetails";
        public const string UpdatePaymentDetails = "api/Payment/UpdatePaymentDetails";
        #endregion

        #region Customer 
        public const string AddRegistrationMembers = "api/Customer/AddRegistrationMembers"; 
        public const string GetRegistrationMembers = "api/Customer/GetRegistrationMembers";
        public const string GetRegistrationMembersByEmail = "api/Customer/GetRegistrationMembersByEmail";
        #endregion

        #region Common
        public const string GetCountries = "api/Common/GetCountries";
        #endregion
    }
}

using InfoMed.Models;
using InfoMed.Services.Implementation;
using InfoMed.Services.Interface;
using InfoMed.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace InfoMed.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly ITextContentAreasService _textContentAreasService;
        private readonly ISchedulerService _schedulerService;
        private readonly IFeesService _feesService;
        private readonly ILastYearMemoriesService _lastYearMemories;
        private readonly IPaymentService _payment;
        private readonly Session _session;

        public EventController(IEventService eventService, Session session, ITextContentAreasService textContentAreasService, ISchedulerService schedulerService, IFeesService feesService, ILastYearMemoriesService lastYearMemories, IPaymentService payment)
        {
            _eventService = eventService;
            _session = session;
            _textContentAreasService = textContentAreasService;
            _schedulerService = schedulerService;
            _feesService = feesService;
            _lastYearMemories = lastYearMemories;
            _payment = payment;
        }

        [HttpGet]
        public async Task<IActionResult> Home(string e)
        {
            _session.RemoveUserData();
            var eventVersion = await _eventService.GetEventDetails(e);
            if (eventVersion is null) throw new Exception("Page not found!");
            _session.SetEvent(eventVersion.EventVersion);
            return View("EventHomePage", eventVersion);
        }

        [HttpGet]
        public IActionResult PrivacyPolicy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TermsConditions()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Event(string? view)
        {
            ViewBag.sessionUser = _session.GetUserDetails();
            ViewBag.EventTypes = await _eventService.GetEventTypes();
            _session.SetEventId(0);
            _session.SetEventVersion(0);
            switch (view)
            {
                case Constants.EventDetails:
                    string eventDetails = Utils.Utils.RenderRazorViewToString(this, "../../Views/Event/_AddEventPartial");
                    return Json(new { html = eventDetails });
                case Constants.TextContent:
                    var content = new List<TextContentAreasDto>();
                    string textContent = Utils.Utils.RenderRazorViewToString(this, "../../Views/TextContentAreas/TextContentAreas", content);
                    return Json(new { html = textContent });
                case Constants.Sponsors:
                    var sponser = new List<SponserDto>();
                    string sponserDto = Utils.Utils.RenderRazorViewToString(this, "../../Views/Event/_sponserList", sponser);
                    return Json(new { html = sponserDto });
                case "Speakers":
                    var speaker = new List<SpeakersDto>();
                    string speakerDto = Utils.Utils.RenderRazorViewToString(this, "../../Views/Event/_speakerList", speaker);
                    return Json(new { html = speakerDto });
                case "Media":
                    return Json(new { html = "<h1 class='fs-1 text-center fw-bold'>Media Comming Soon</h1>" });
                case Constants.Schedules:
                    var schedules = new List<ScheduleMasterDto>();
                    string schedulePartialView = Utils.Utils.RenderRazorViewToString(this, "../../Views/Schedule/Schedules", schedules);
                    return Json(new { html = schedulePartialView });
                case Constants.Fees:
                    var fees = new List<ConferenceFeeDto>();
                    ViewBag.sessionUser = _session.GetUserDetails();
                    string feesPartialView = Utils.Utils.RenderRazorViewToString(this, "../../Views/Fees/_FeesList", fees);
                    return Json(new { html = feesPartialView });
                case Constants.Payment:
                    ViewBag.sessionUser = _session.GetUserDetails();
                    var payment = new PaymentDetailsDto();
                    string paymentDetails = Utils.Utils.RenderRazorViewToString(this, "../../Views/Payment/_AddPaymentPartial", payment);
                    return Json(new { html = paymentDetails });
                case Constants.LastYearMemories:
                    var lastYearMemories = new List<LastYearMemoryDto>();
                    string lastYearMemoriesPartialView = Utils.Utils.RenderRazorViewToString(this, "../../Views/LastYearMemories/_LastYearMemoriesList", lastYearMemories);
                    return Json(new { html = lastYearMemoriesPartialView });
            }
            _session.SetEventStatus(string.Empty);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEvent(EventVersionDto eventVersion)
        {
            ViewBag.sessionUser = _session.GetUserDetails();
            ViewBag.EventTypes = await _eventService.GetEventTypes();

            if (ModelState.IsValid)
            {
                var isUnique = await _eventService.IsEventNameUnique(eventVersion);
                if (isUnique)
                {
                    if (eventVersion.EventBackgroundLogoMap != null && eventVersion.EventBackgroundLogoMap.Length > 0)
                    {
                        var imagePath = await SaveImageToFileSystem(eventVersion.EventBackgroundLogoMap,"EventMaster");
                        eventVersion.EventBackgroundImage = imagePath;                       
                    }
                    var _event = await _eventService.AddEvent(eventVersion);
                    _session.SetEventId(_event.IdEvent);
                    _session.SetEventVersion(_event.IdEventVersion);
                    string eventDetails = Utils.Utils.RenderRazorViewToString(this, "../../Views/Event/_AddEventPartial", _event);
                    return Json(new { html = eventDetails, success = true });
                }
                else
                {
                    string eventDetails = Utils.Utils.RenderRazorViewToString(this, "../../Views/Event/_AddEventPartial", eventVersion);
                    return Json(new { html = eventDetails, success = false });
                }
            }
            return View(eventVersion);
        }

        public async Task<string> SaveImageToFileSystem(IFormFile file, string moduleName)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", moduleName);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            //var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName.Trim();
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Regex.Replace(file.FileName.Trim(), @"[^a-zA-Z0-9_.-]", "");
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/uploads/{moduleName}/{uniqueFileName}";
        }

        [HttpGet]
        public async Task<IActionResult> ViewEventImage(string filePath)
        {                     
            
                string _textContent = Utils.Utils.RenderRazorViewToString(this, "../../Views/Event/EventContentPartial", filePath);
                return Json(new { html = _textContent });          
            
        }

        [HttpGet]
        public async Task<IActionResult> EditEvent(int id, string? view,int idEventVersion)
        {
            ViewBag.sessionUser = _session.GetUserDetails();
            ViewBag.EventTypes = await _eventService.GetEventTypes();

            if (id == 0)
            {
                id = _session.GetEventId();
                idEventVersion = _session.GetEventVersion();
            }
            else
            {
                _session.SetEventId(id);
                _session.SetEventVersion(idEventVersion);
            }
            var _event = await _eventService.GetEventById(id, idEventVersion);
            _session.SetEventId(_event?.IdEvent??0);
            _session.SetEventVersion(_event?.IdEventVersion ?? 0);
            _session.SetEventStatus(_event?.VersionStatus??string.Empty);
            _session.SetEvent(_event);

            switch (view)
            {
                case Constants.EventDetails:
                    string eventDetails = Utils.Utils.RenderRazorViewToString(this, "../../Views/Event/_AddEventPartial", _event);
                    return Json(new { html = eventDetails });
                case Constants.TextContent:
                    var content = await _textContentAreasService.GetTextContentByEventVersionId(id, idEventVersion);
                    string textContent = Utils.Utils.RenderRazorViewToString(this, "../../Views/TextContentAreas/TextContentAreas", content);
                    return Json(new { html = textContent });
                case Constants.Sponsors:                    
                    var _sponserList = await _eventService.GetSponserList(id, idEventVersion);                    
                    string sponser = Utils.Utils.RenderRazorViewToString(this, "../../Views/Event/_sponserList", _sponserList);
                    return Json(new { html = sponser });
                case Constants.Speakers:                    
                    var _speakerList = await _eventService.GetSpeakerList(id, idEventVersion);                   
                    string speaker = Utils.Utils.RenderRazorViewToString(this, "../../Views/Event/_speakerList", _speakerList);
                    return Json(new { html = speaker });
                case Constants.Media:
                    return Json(new { html = "<h1 class='fs-1 text-center fw-bold'>Media Comming Soon</h1>" });
                case Constants.Schedules:
                    var schedules = await _schedulerService.GetSchedulesMaster(id, idEventVersion);
                    string schedulePartialView = Utils.Utils.RenderRazorViewToString(this, "../../Views/Schedule/Schedules", schedules);
                    return Json(new { html = schedulePartialView });
                case Constants.Fees:
                    var fees = await _feesService.GetConferenceFeesList(id, idEventVersion);
                    string feesPartialView = Utils.Utils.RenderRazorViewToString(this, "../../Views/Fees/_FeesList", fees);
                    return Json(new { html = feesPartialView });
                case Constants.Payment:
                    var payment = await _payment.GetPaymentDetails(id, idEventVersion);
                    string paymentDetails = Utils.Utils.RenderRazorViewToString(this, "../../Views/Payment/_AddPaymentPartial", payment);
                    return Json(new { html = paymentDetails });
                case Constants.LastYearMemories:
                    var lastYearMemories = await _lastYearMemories.GetLastYearMemoriesList(id, idEventVersion);
                    string lastYearMemoriesPartialView = Utils.Utils.RenderRazorViewToString(this, "../../Views/LastYearMemories/_LastYearMemoriesList", lastYearMemories);
                    return Json(new { html = lastYearMemoriesPartialView });
            }
            return View("Event", _event);
        }

        public async Task<IActionResult> EventVersionCreate(int id)
        {
            if (id > 0)
            {
                var _event = await _eventService.EventVersionCreate(id);

                return await EditEvent(id, null, _event);
            }
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public async Task<IActionResult> EditEvent(EventVersionDto eventVersion)
        {
            ViewBag.sessionUser = _session.GetUserDetails();
            ViewBag.EventTypes = await _eventService.GetEventTypes();

            if (ModelState.IsValid)
            {
                var isUnique = await _eventService.IsEventNameUnique(eventVersion);
                if (isUnique)
                {
                    if (eventVersion.EventBackgroundLogoMap != null && eventVersion.EventBackgroundLogoMap.Length > 0)
                    {
                        var imagePath = await SaveImageToFileSystem(eventVersion.EventBackgroundLogoMap, "EventMaster");
                        eventVersion.EventBackgroundImage = imagePath;
                    }
                    else
                    {
                        var eventimage = await _eventService.GetEventById(eventVersion.IdEvent, eventVersion.IdEventVersion);
                        eventVersion.EventBackgroundImage = eventimage.EventBackgroundImage;
                    }
                    var _event = await _eventService.UpdateEvent(eventVersion);
                    _session.SetEventId(_event.IdEvent);
                    _session.SetEventVersion(_event.IdEventVersion);
                    string eventDetails = Utils.Utils.RenderRazorViewToString(this, "../../Views/Event/_AddEventPartial", _event);
                    return Json(new { html = eventDetails, success = true });
                }
                else
                {
                    string eventDetails = Utils.Utils.RenderRazorViewToString(this, "../../Views/Event/_AddEventPartial", eventVersion);
                    return Json(new { html = eventDetails, success = false });
                }
            }
            return View("Event", eventVersion);
        }




        [HttpPost]
        public async Task<IActionResult> SubmitEvent(int idEventId, int idEventVersion)
        {
            try
            {
                if (idEventId > 0)
                {
                    var _event = await _eventService.GetEventById(idEventId, idEventVersion);
                    _event.VersionStatus = "Submitted";
                    var eventVersion = await _eventService.UpdateEvent(_event);
                    return Json(new { success = true });
                }
                else
                {
                    var eventId = _session.GetEventId();
                    var eventversion = _session.GetEventVersion();
                    var _event = await _eventService.GetEventById(eventId, eventversion);
                    _event.VersionStatus = "Submitted";
                    var eventVersion = await _eventService.UpdateEvent(_event);
                    return Json(new { success = true });

                }
                return Json(new { success = false });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> ApproveEvent(int idEventId,int idEventVersion)
        {
            try
            {
                if (idEventId > 0)
                {
                    var _event = await _eventService.GetEventById(idEventId,idEventVersion);
                    _event.VersionStatus = "Approved";
                    var eventVersion = await _eventService.UpdateEvent(_event);
                    return Json(new { success = true });
                }
                else
                {
                    var eventId = _session.GetEventId();
                    var eventversion = _session.GetEventVersion();
                    var _event = await _eventService.GetEventById(eventId, eventversion);
                    _event.VersionStatus = "Approved";
                    var eventVersion = await _eventService.UpdateEvent(_event);
                    return Json(new { success = true });
                }
                return Json(new { success = false });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> RejectEvent(int idEventId, int idEventVersion)
        {
            try
            {
                if (idEventId > 0)
                {
                    var _event = await _eventService.GetEventById(idEventId, idEventVersion);
                    _event.VersionStatus = "Draft";
                    var eventVersion = await _eventService.UpdateEvent(_event);
                    return Json(new { success = true });
                }
                else
                {
                    var eventId = _session.GetEventId();
                    var eventversion = _session.GetEventVersion();
                    var _event = await _eventService.GetEventById(eventId, eventversion);
                    _event.VersionStatus = "Draft";
                    var eventVersion = await _eventService.UpdateEvent(_event);
                    return Json(new { success = true });
                }
                return Json(new { success = false });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }



        [HttpGet]
        public async Task<IActionResult> AddSponser()
        {
            SponserDto sponserDto = new SponserDto();
            ViewBag.SponserTypes = await _eventService.GetSponserTypes();
            string html = Utils.Utils.RenderRazorViewToString(this, Constants.AddSponser, sponserDto);
            return Json(new { data = html });
        }

        [HttpPost]
        public async Task<IActionResult> AddSponser(SponserDto sponser)
        {
            try
            {
                var eventId = _session.GetEventId();
                var eventVersion = _session.GetEventVersion();
                if (eventId <= 0)
                {
                    eventId = sponser.IdEvent;
                    eventVersion = sponser.IdEventVersion;
                    if (eventId <= 0)
                    {
                        return Json(new { success = false });
                    }
                }
                if (sponser.SponserLogoMap != null && sponser.SponserLogoMap.Length > 0)
                {
                    var imagePath = await SaveImageToFileSystem(sponser.SponserLogoMap, "Sponser");
                    sponser.SponsorLogo = imagePath;
                    sponser.IdEvent = eventId;
                    sponser.Status = true;
                    sponser.IdEventVersion = eventVersion;
                }              
                await _eventService.Addsponser(sponser);

                return Json(new { success = true, IdEvent = sponser.IdEvent, IdEventVersion = sponser.IdEventVersion });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditSponser(int id)
        {
            var _sponser = await _eventService.GetSponserById(id);          
            var sponserTypes = await _eventService.GetSponserTypes();
            ViewBag.SponserTypes = sponserTypes;
            string html = Utils.Utils.RenderRazorViewToString(this, Constants.EditSponser, _sponser);
            return Json(new { data = html });
        }


        [HttpPost]
        public async Task<IActionResult> EditSponser(SponserDto sponser)
        {
            try
            {
                if (sponser.SponserLogoMap != null && sponser.SponserLogoMap.Length > 0)
                {

                    var imagePath = await SaveImageToFileSystem(sponser.SponserLogoMap, "Sponser");
                    sponser.SponsorLogo = imagePath;                    
                    sponser.Status = true;
                }
                else
                {
                    var _sponser = await _eventService.GetSponserById(sponser.IdEventSponsor);
                    sponser.SponsorLogo = _sponser.SponsorLogo;
                    sponser.Status = true;
                }             
                await _eventService.Updatesponser(sponser);
                return Json(new { success = true, IdEvent = sponser.IdEvent , IdEventVersion = sponser.IdEventVersion });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpGet]
        public async Task<IActionResult> DeleteSponsor(int id)
        {
            var _textContent = await _eventService.DeleteSponsor(id);
            int sessionid = _session.GetEventId();
            int eventVersion = _session.GetEventVersion();
            var _sponserList = await _eventService.GetSponserList(sessionid, eventVersion);          
            ViewBag.sessionUser = _session.GetUserDetails();
            string sponser = Utils.Utils.RenderRazorViewToString(this, "../../Views/Event/_sponserList", _sponserList);
            return Json(new { html = sponser });
        }

        [HttpGet]
        public async Task<IActionResult> AddSpeaker()
        {
            SpeakersDto speakersDto = new SpeakersDto();
            string html = Utils.Utils.RenderRazorViewToString(this, Constants.AddSpeaker, speakersDto);
            return Json(new { data = html });
        }


        [HttpPost]
        public async Task<IActionResult> AddSpeaker(SpeakersDto speaker)
        {
            try
            {
                var eventId = _session.GetEventId();
                var eventVersion = _session.GetEventVersion();

                if (eventId <= 0)
                {
                    if (eventId <= 0)
                    {
                        eventId = speaker.IdEvent;
                        eventVersion = speaker.IdEventVersion;
                        if (eventId <= 0)
                        {
                            return Json(new { success = false });
                        }
                    }
                }
                if (speaker.SpeakerLogoMap != null && speaker.SpeakerLogoMap.Length > 0)
                {
                    var imagePath = await SaveImageToFileSystem(speaker.SpeakerLogoMap, "Speaker");
                    speaker.SpeakerImage = imagePath;
                    speaker.IdEvent = eventId;
                    speaker.Status = true;
                    speaker.IdEventVersion = eventVersion;                  
                }
                await _eventService.AddSpeaker(speaker);

                return Json(new { success = true, IdEvent = speaker.IdEvent, IdEventVersion = speaker.IdEventVersion });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpGet]
        public async Task<IActionResult> EditSpeaker(int id)
        {
            var _speaker = await _eventService.GetSpeakerById(id);
            string html = Utils.Utils.RenderRazorViewToString(this, Constants.EditSpeaker, _speaker);
            return Json(new { data = html });
        }

        [HttpPost]
        public async Task<IActionResult> EditSpeaker(SpeakersDto speaker)
        {
            try
            {
                if (speaker.SpeakerLogoMap != null && speaker.SpeakerLogoMap.Length > 0)
                {
                    var imagePath = await SaveImageToFileSystem(speaker.SpeakerLogoMap, "Sponser");
                    speaker.SpeakerImage = imagePath;
                    speaker.Status = true;
                }
                else
                {
                    var _speaker = await _eventService.GetSpeakerById(speaker.IdSpeaker);
                    speaker.SpeakerImage = _speaker.SpeakerImage;
                    speaker.Status = true;
                }

                await _eventService.Updatespeaker(speaker);
                return Json(new { success = true, IdEvent = speaker.IdEvent, IdEventVersion = speaker.IdEventVersion });                
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpGet]
        public async Task<IActionResult> DeleteSpeaker(int id)
        {
            var _textContent = await _eventService.DeleteSponsor(id);
            var _speaker = await _eventService.GetSpeakerById(id);
            _speaker.Status = false;
            await _eventService.Updatespeaker(_speaker);
            int sessionid = _session.GetEventId();
            int eventVersion = _session.GetEventVersion();
            var _speakerList = await _eventService.GetSpeakerList(sessionid, eventVersion);          
            ViewBag.sessionUser = _session.GetUserDetails();
            string speaker = Utils.Utils.RenderRazorViewToString(this, "../../Views/Event/_speakerList", _speakerList);
            return Json(new { html = speaker });
        }

    }
}

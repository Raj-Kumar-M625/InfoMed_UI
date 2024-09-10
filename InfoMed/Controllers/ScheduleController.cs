using InfoMed.Models;
using InfoMed.Services.Interface;
using InfoMed.Utils;
using Microsoft.AspNetCore.Mvc;

namespace InfoMed.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly ISchedulerService _schedulerService;
        private readonly Session _session;

        public ScheduleController(ISchedulerService schedulerService, Session session)
        {
            _schedulerService = schedulerService;
            _session = session;
        }

        [HttpGet]
        public IActionResult AddSchedule()
        {
            string textContent = Utils.Utils.RenderRazorViewToString(this, "../../Views/Schedule/UpsertSchedule");
            return Json(new { html = textContent });
        }

        [HttpPost]
        public async Task<IActionResult> AddSchedule(ScheduleMasterDto scheduleMasterDto)
        {
            ViewBag.sessionUser = _session.GetUserDetails();
            if (ModelState.IsValid)
            {
                await _schedulerService.AddScheduleMaster(scheduleMasterDto);
            }
            int id = _session.GetEventId();
            int eventVersion = _session.GetEventVersion();
            var content = await _schedulerService.GetSchedulesMaster(id, eventVersion);
            string schedulePartialView = Utils.Utils.RenderRazorViewToString(this, "../../Views/Schedule/Schedules", content);
            return Json(new { html = schedulePartialView });
        }

        [HttpGet]
        public async Task<IActionResult> EditSchedule(int idScheduleMaster)
        {
            var content = await _schedulerService.GetScheduleMasterById(idScheduleMaster);
            string upsertScheduleView = Utils.Utils.RenderRazorViewToString(this, "../../Views/Schedule/UpsertSchedule", content);
            return Json(new { html = upsertScheduleView });
        }

        [HttpPost]
        public async Task<IActionResult> EditSchedule(ScheduleMasterDto scheduleMasterDto)
        {
            ViewBag.sessionUser = _session.GetUserDetails();
            if (ModelState.IsValid)
            {
                await _schedulerService.UpdateScheduleMaster(scheduleMasterDto);
            }
            int id = _session.GetEventId();
            int eventVersion = _session.GetEventVersion();
            var content = await _schedulerService.GetSchedulesMaster(id, eventVersion);
            string upsertScheduleView = Utils.Utils.RenderRazorViewToString(this, "../../Views/Schedule/Schedules", content);
            return Json(new { html = upsertScheduleView });
        }

        [HttpGet]
        public async Task<IActionResult> ScheduleDetails(int idScheduleMaster)
        {
            ViewBag.IdScheduleMaster = idScheduleMaster;
            ViewBag.sessionUser = _session.GetUserDetails();
            var scheduleDetails = await _schedulerService.GetSchedulesDetails(idScheduleMaster);
            string scheduleDetailsPartialView = Utils.Utils.RenderRazorViewToString(this, "../../Views/Schedule/ScheduleDetails", scheduleDetails);
            return Json(new { html = scheduleDetailsPartialView });
        }

        [HttpGet]
        public IActionResult AddScheduleDetails(int idScheduleMaster)
        {
            ViewBag.IdScheduleMaster = idScheduleMaster;
            ViewBag.sessionUser = _session.GetUserDetails();
            string scheduleDetailsPartialView = Utils.Utils.RenderRazorViewToString(this, "../../Views/Schedule/UpsertScheduleDetails");
            return Json(new { html = scheduleDetailsPartialView });
        }

        [HttpPost]
        public async Task<IActionResult> AddScheduleDetails(ScheduleDetailsDto scheduleDetailsDto)
        {
            ViewBag.sessionUser = _session.GetUserDetails();
            if (ModelState.IsValid)
            {
                await _schedulerService.AddScheduleDetails(scheduleDetailsDto);
            }
            ViewBag.IdScheduleMaster = scheduleDetailsDto.IdScheduleMaster;
            var content = await _schedulerService.GetSchedulesDetails(scheduleDetailsDto.IdScheduleMaster);
            string scheduleDetailsPartialView = Utils.Utils.RenderRazorViewToString(this, "../../Views/Schedule/ScheduleDetails", content);
            return Json(new { html = scheduleDetailsPartialView });
        }

        [HttpGet]
        public async Task<IActionResult> EditScheduleDetails(int idScheduleDetails)
        {
           var content = await _schedulerService.GetScheduleDetailById(idScheduleDetails);
            ViewBag.IdScheduleMaster = content.IdScheduleMaster;
            string upsertScheduleDetailsView = Utils.Utils.RenderRazorViewToString(this, "../../Views/Schedule/UpsertScheduleDetails", content);
            return Json(new { html = upsertScheduleDetailsView });
        }

        [HttpPost]
        public async Task<IActionResult> EditScheduleDetails(ScheduleDetailsDto scheduleDetailsDto)
        {
            ViewBag.sessionUser = _session.GetUserDetails();
            if (ModelState.IsValid)
            {
                await _schedulerService.UpdateScheduleDetails(scheduleDetailsDto);
            }
            ViewBag.IdScheduleMaster = scheduleDetailsDto.IdScheduleMaster;
            var content = await _schedulerService.GetSchedulesDetails(scheduleDetailsDto.IdScheduleMaster);
            string scheduleDetailsPartialView = Utils.Utils.RenderRazorViewToString(this, "../../Views/Schedule/ScheduleDetails", content);
            return Json(new { html = scheduleDetailsPartialView });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteScheduleMaster(int idScheduleMaster)
        {
            var scheduleMaster = await _schedulerService.GetScheduleMasterById(idScheduleMaster);
            scheduleMaster.IsActive = false;
            await _schedulerService.UpdateScheduleMaster(scheduleMaster);
            int id = _session.GetEventId();
            ViewBag.sessionUser = _session.GetUserDetails();
            int eventVersion = _session.GetEventVersion();
            var content = await _schedulerService.GetSchedulesMaster(id, eventVersion);
            string schedulePartialView = Utils.Utils.RenderRazorViewToString(this, "../../Views/Schedule/Schedules", content);
            return Json(new { html = schedulePartialView });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteScheduleDetail(int idScheduleDetail)
        {
            var scheduleDetail = await _schedulerService.GetScheduleDetailById(idScheduleDetail);
            scheduleDetail.IsActive = false;
            await _schedulerService.UpdateScheduleDetails(scheduleDetail);
            ViewBag.sessionUser = _session.GetUserDetails();
            ViewBag.IdScheduleMaster = scheduleDetail.IdScheduleMaster;
            var content = await _schedulerService.GetSchedulesDetails(scheduleDetail.IdScheduleMaster);
            string scheduleDetailsPartialView = Utils.Utils.RenderRazorViewToString(this, "../../Views/Schedule/ScheduleDetails", content);
            return Json(new { html = scheduleDetailsPartialView });
        }
    }
}

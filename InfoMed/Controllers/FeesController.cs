using InfoMed.Models;
using InfoMed.Services.Interface;
using InfoMed.Utils;
using Microsoft.AspNetCore.Mvc;

namespace InfoMed.Controllers
{
    public class FeesController : Controller
    {
        private readonly IFeesService _feesService;
        private readonly Session _session;
        private readonly IEventService _eventService;
        public FeesController(IFeesService feesService, Session session, IEventService eventService)
        {
            _feesService = feesService;
            _session = session;
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> AddFees()
        {

            var id = _session.GetEventId();
            var idEventVersion = _session.GetEventVersion();
            var _event = await _eventService.GetEventById(id, idEventVersion);            
            ViewBag.EventDetials = _event;
            string textContent = Utils.Utils.RenderRazorViewToString(this, "../../Views/Fees/UpsertFees");
            return Json(new { html = textContent });
        }

        [HttpPost]
        public async Task<IActionResult> AddFees(ConferenceFeeDto feesMasterDto)
        {
            ViewBag.sessionUser = _session.GetUserDetails();
            if (ModelState.IsValid)
            {
                await _feesService.AddFees(feesMasterDto);
            }
            int id = _session.GetEventId();
            int eventVersion = _session.GetEventVersion();
            var content = await _feesService.GetConferenceFeesList(id, eventVersion);
            string feesPartialView = Utils.Utils.RenderRazorViewToString(this, "../../Views/Fees/_FeesList", content);
            return Json(new { html = feesPartialView });
        }

        
         [HttpGet]
        public async Task<IActionResult> editFees(int idFeesMaster)
        {
            var content = await _feesService.GetFeesById(idFeesMaster);
            string upsertFeesView = Utils.Utils.RenderRazorViewToString(this, "../../Views/Fees/UpsertFees", content);
            return Json(new { html = upsertFeesView });
        }

        [HttpPost]
        public async Task<IActionResult> EditSchedule(ConferenceFeeDto feesMasterDto)
        {
            ViewBag.sessionUser = _session.GetUserDetails();
            if (ModelState.IsValid)
            {
                await _feesService.UpdateFees(feesMasterDto);
            }
            int id = _session.GetEventId();
            int eventVersion = _session.GetEventVersion();
            var content = await _feesService.GetConferenceFeesList(id, eventVersion);
            string upsertScheduleView = Utils.Utils.RenderRazorViewToString(this, "../../Views/Fees/_FeesList", content);
            return Json(new { html = upsertScheduleView });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteFeesMaster(int idFeesMaster)
        {
            var scheduleMaster = await _feesService.GetFeesById(idFeesMaster);
            scheduleMaster.IsActive = false;
            await _feesService.UpdateFees(scheduleMaster);
            int id = _session.GetEventId();
            int eventVersion = _session.GetEventVersion();
            ViewBag.sessionUser = _session.GetUserDetails();
            var content = await _feesService.GetConferenceFeesList(id, eventVersion);
            string schedulePartialView = Utils.Utils.RenderRazorViewToString(this, "../../Views/Fees/_FeesList", content);
            return Json(new { html = schedulePartialView });
        }

    }
}

using InfoMed.Models;
using InfoMed.Services.Implementation;
using InfoMed.Services.Interface;
using InfoMed.Utils;
using Microsoft.AspNetCore.Mvc;

namespace InfoMed.Controllers
{
    public class TextContentAreaController : Controller
    {
        private readonly ITextContentAreasService _textContentAreas;
        private readonly ISchedulerService _scheduleService;
        private readonly Session _session;

        public TextContentAreaController(ITextContentAreasService textContentAreas, Session session, ISchedulerService scheduleService)
        {
            _textContentAreas = textContentAreas;
            _session = session;
            _scheduleService = scheduleService;
        }

        [HttpGet]
        public IActionResult AddTextContent()
        {
            string textContent = Utils.Utils.RenderRazorViewToString(this, "../../Views/TextContentAreas/UpsertTextContent");
            return Json(new { html = textContent });
        }

        [HttpPost]
        public async Task<IActionResult> AddTextContent(TextContentAreasDto textContentAreaDto)
        {
            ViewBag.sessionUser = _session.GetUserDetails();
            if (ModelState.IsValid)
            {
                await _textContentAreas.AddTextContent(textContentAreaDto);
            }
            int id = _session.GetEventId();
            int eventVersion = _session.GetEventVersion();
            var content = await _textContentAreas.GetTextContentByEventVersionId(id,eventVersion);
            string textContent = Utils.Utils.RenderRazorViewToString(this, "../../Views/TextContentAreas/TextContentAreas", content);
            return Json(new { html = textContent });
        }

        [HttpGet]
        public async Task<IActionResult> EditTextContent(int idTextContentArea)
        {
            var content = await _textContentAreas.GetTextContentById(idTextContentArea);
            string textContent = Utils.Utils.RenderRazorViewToString(this, "../../Views/TextContentAreas/UpsertTextContent", content);
            return Json(new { html = textContent });
        }

        [HttpPost]
        public async Task<IActionResult> EditTextContent(TextContentAreasDto textContentAreaDto)
        {
            ViewBag.sessionUser = _session.GetUserDetails();
            if (ModelState.IsValid)
            {
                await _textContentAreas.UpdateTextContent(textContentAreaDto);
            }
            int id = _session.GetEventId();
            int eventVersion = _session.GetEventVersion();
            var content = await _textContentAreas.GetTextContentByEventVersionId(id, eventVersion);
            string textContent = Utils.Utils.RenderRazorViewToString(this, "../../Views/TextContentAreas/TextContentAreas", content);
            return Json(new { html = textContent });
        }

        [HttpGet]
        public async Task<IActionResult> ViewTextContent(int? IdTextContentArea,int? IdScheduleDetails)
        {
            string data = "";
            if(IdTextContentArea != null)
            {
                var textContentArea = await _textContentAreas.GetTextContentById((int)IdTextContentArea);
                data = textContentArea.ContentText;
            }

            if (IdScheduleDetails != null)
            {
                var scheduleDetaiils = await _scheduleService.GetScheduleDetailById((int)IdScheduleDetails);
                data = scheduleDetaiils.Topic;
            }

            string _textContent = Utils.Utils.RenderRazorViewToString(this, "../../Views/TextContentAreas/TextContentPartial", data!);
            return Json(new { html = _textContent });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteTextContent(int idTextContentArea)
        {
            var _textContent = await _textContentAreas.GetTextContentById(idTextContentArea);
            _textContent.Status = false;
            await _textContentAreas.UpdateTextContent(_textContent);
            int id = _session.GetEventId();
            int eventVersion = _session.GetEventVersion();
            ViewBag.sessionUser = _session.GetUserDetails();
            var content = await _textContentAreas.GetTextContentByEventVersionId(id, eventVersion);
            string textContent = Utils.Utils.RenderRazorViewToString(this, "../../Views/TextContentAreas/TextContentAreas", content);
            return Json(new { html = textContent });
        }
    }
}

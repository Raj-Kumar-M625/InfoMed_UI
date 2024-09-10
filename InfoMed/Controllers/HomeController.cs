using InfoMed.Models;
using InfoMed.Services.Interface;
using InfoMed.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace InfoMed.Controllers
{
    public class HomeController : Controller
    {
        private readonly Session _session;
        private readonly IEventService _eventService;
        private readonly IUserService _userService;

        public HomeController(Session session, IEventService eventService, IUserService userService)
        {
            _session = session;
            _eventService = eventService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string version)
        {
            ViewBag.sessionUser = _session.GetUserDetails();
            if (_session.GetUserDetails() == null) return RedirectToAction("Login", "Account");

            ViewBag.Users = await _userService.GetUsers();
            ViewBag.Events = await _eventService.GetEvents(version ?? "LatestVersion");

            if(version != null)
            {
                string html = Utils.Utils.RenderRazorViewToString(this, "../../Views/Home/_EventListPartial");
                return Json(new { data = html });
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard(string version)
        {
            ViewBag.sessionUser = _session.GetUserDetails();
            if (_session.GetUserDetails() == null) return RedirectToAction("Login", "Account");

            var submittedEvents = await _eventService.GetEvents(version ?? "LatestVersion");
            ViewBag.Events = submittedEvents.Where(x => x.VersionStatus == Constants.Submitted).ToList();
            ViewBag.Route = Constants.Home;

            if (version != null)
            {
                string html = Utils.Utils.RenderRazorViewToString(this, "../../Views/Home/_EventListPartial");
                return Json(new { data = html });
            }

            return View();
        }

        [HttpPost]
        public IActionResult UploadFile(IFormFile file)
        {
            return Json(new { data = true });
        }

        [Route("Error/{statusCode}")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error404",new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
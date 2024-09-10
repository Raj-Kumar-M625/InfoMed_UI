using InfoMed.Models;
using InfoMed.Services.Implementation;
using InfoMed.Services.Interface;
using InfoMed.Utils;
using Microsoft.AspNetCore.Mvc;

namespace InfoMed.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly Session _session;
        public UserController(IUserService userService,Session session)
        {
            _userService = userService;
            _session = session;
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var sessionData = _session.GetUserDetails();
            if (sessionData == null) return RedirectToAction("Login", "Account");

            var users = await _userService.GetUsers();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> AddUser()
        {
            ViewBag.UserRoles = await _userService.GetUserRoles();
            string textContent = Utils.Utils.RenderRazorViewToString(this, "../../Views/User/UpsertUser");
            return Json(new { html = textContent });
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            if (ModelState.IsValid)
            {
                await _userService.AddUser(user);
            }
            var users = await _userService.GetUsers();
            string textContent = Utils.Utils.RenderRazorViewToString(this, "../../Views/User/_UserListPartial", users);
            return Json(new { html = textContent });
        }


        [HttpGet]
        public async Task<IActionResult> EditUser(int IdUser)
        {
            var user = await _userService.GetUsersbyId(IdUser);
            ViewBag.UserRoles = await _userService.GetUserRoles();
            string textContent = Utils.Utils.RenderRazorViewToString(this, "../../Views/User/UpsertUser", user);
            return Json(new { html = textContent });
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                await _userService.UpdateUser(user);
            }
            var users = await _userService.GetUsers();
            string textContent = Utils.Utils.RenderRazorViewToString(this, "../../Views/User/_UserListPartial", users);
            return Json(new { html = textContent });
        }
    }
}

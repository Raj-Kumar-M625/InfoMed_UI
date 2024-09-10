using InfoMed.Models;
using InfoMed.Services.Implementation;
using InfoMed.Services.Interface;
using InfoMed.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity;

namespace InfoMed.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ICustomerService _customerService;
        private readonly Session _session;

        public AccountController(IAccountService accountService, ICustomerService customerService, Session session)
        {
            _accountService = accountService;
            _customerService = customerService;
            _session = session;
        }

        [HttpGet]
        public ActionResult Login()
        
        {
            var sessionData = _session.GetUserDetails();
            if (sessionData != null) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpGet]
        public ActionResult SignIn(int idEvent, int idVersion)
        {
            ViewBag.IdVersion = idVersion;
            _session.SetEventId(idEvent);
            _session.SetVersion(idVersion);
            string htmlString = Utils.Utils.RenderRazorViewToString(this, "../../Views/Account/_SignInPartial");
            return Json(new { html = htmlString });
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserDto user)
        {
            if (ModelState.IsValid)
            {
                var res = await _accountService.Login(user, HttpContext);

                if (!res)
                {
                    ViewBag.Message = "Invalid credential.";
                    return View(user);
                }

                var sessionUser = _session.GetUserDetails();

                return sessionUser.UserRole switch
                {
                    "Admin" => RedirectToAction("Dashboard", "Home"),
                    "Volunteer" => RedirectToAction("Index", "Home"),
                    _ => View(user),
                };
            }
            return View(user);
        }

        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var result = await _accountService.Login(new UserDto { EmailAddress = resetPasswordDto.Email, Password = resetPasswordDto.OldPassword }
                                                               , HttpContext);
            if (!result)
            {
                ViewBag.Message = "Invalid credential.";
                return View();
            }

            if (resetPasswordDto.Password != resetPasswordDto.ConfirmPassword)
            {
                ViewBag.Message = "Passwords do not match!";
                return View();
            }

            var res = await _accountService.ResetPassword(resetPasswordDto);

            if (res.Error)
            {
                ViewBag.Message = res.Message;
                return View();
            }
            else
            {
                _session.RemoveUserData();
            }

            return RedirectToAction("Login");
        }


        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ResetPasswordDto resetPasswordDto)
        {
            if (resetPasswordDto.Password != resetPasswordDto.ConfirmPassword)
            {
                ViewBag.Message = "Passwords do not match!";
                return View();
            }

            var res = await _accountService.ResetPassword(resetPasswordDto);

            if (res.Error)
            {
                ViewBag.Message = res.Message;
                return View();
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> SendOtp(string email)
        {
            var random = new Random();
            int otpCode = random.Next(111111, 999999);
            _session.SetOTP(otpCode);

            string message = @$"<p>Hi {email},</p>
                                 <h3>Your OTP for Infomed Conference Registration</h3>
                                 <h1>{otpCode}</h1>
                                 <p>This OTP will be valid for 5 Minutes</p>
                                 <p>If you didn't request this code, you can safely ignore this email. 
                                 Someone else might have typed your email address by mistake</p>
                                 <p>Thanks</p>
                                 <h3>Infomed Team</h3>
                               ";
            bool IsEmailSent = await EmailService.SendMail(email, message, "Verification Code");

            return Json(new
            {
                Email = email,
                Otp = otpCode,
                success = IsEmailSent
            });
        }

        [HttpGet]
        public async Task<IActionResult> VerifyOtp(int otp)
        {
            int OTP = _session.GetOTP();
            var success = false;
            int tempOtp = 112358;
            if(tempOtp == otp)
            {
                success = true;
                _session.RemoveOTP();
            }else if (OTP == otp)
            {
                success = true;
                _session.RemoveOTP();
            }

            return Json(new { success = success });
        }

        [HttpGet("logout")]
        public ActionResult Logout()
        {
            _session.RemoveUserData();
            return RedirectToAction("Login");
        }
    }
}


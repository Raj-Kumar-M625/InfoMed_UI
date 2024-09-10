using InfoMed.Models;
using InfoMed.Services.Interface;
using InfoMed.Utils;
using Microsoft.AspNetCore.Mvc;

namespace InfoMed.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IFeesService _FeeService;
        private readonly IPaymentService _paymentService;
        private readonly ICommonService _commonService;
        private readonly Session _session;

        public CustomerController(ICustomerService customerService, IFeesService feeService, 
            IPaymentService paymentService, Session session, ICommonService commonService)
        {
            _customerService = customerService;
            _FeeService = feeService;
            _paymentService = paymentService;
            _session = session;
            _commonService = commonService;
        }

        [HttpGet]
        public async Task<IActionResult> Registeration(string email)
        {
            _session.SetEmail(email);
            var eventDeatils = _session.GetEvent();
            int idEvent = _session.GetEventId();
            int idVersion = _session.GetVersion();
            EventVersionDto eventVersionDto = _session.GetEvent();

            var registeredMember = await _customerService.GetRegistrationMembersByEmail(email, idEvent);

            if(registeredMember != null && registeredMember?.RegType != "Single Registration")
            {
                registeredMember.RegistrationMembers = await _customerService.GetRegistrationMembers(registeredMember?.IdRegistration ?? 0);
            }

            ViewBag.ConferenceFees = await _FeeService.GetConferenceFeesList(idEvent, eventVersionDto.IdEventVersion);
            ViewBag.Payment = await _paymentService.GetPaymentDetails(idEvent, eventVersionDto.IdEventVersion);
            ViewBag.Countries = await _commonService.GetCountries();
            return View(registeredMember ?? new RegistrationDto { EmailID = email });
        }

        [HttpGet]
        public async Task<IActionResult> RegistrationType(string type)
        {
            var eventDeatils = _session.GetEvent();
            int idEvent = _session.GetEventId();
            string email = _session.GetEmail();
            int idVersion = eventDeatils.IdEventVersion;


            ViewBag.ConferenceFees = await _FeeService.GetConferenceFeesList(idEvent, idVersion);
            ViewBag.Payment = await _paymentService.GetPaymentDetails(idEvent, idVersion);
            ViewBag.Countries = await _commonService.GetCountries();
            if (type == "Single Registration")
            {
                var registeredMember = await _customerService.GetRegistrationMembersByEmail(email, idEvent);
                ViewBag.IdEvent = idEvent;
                string single = Utils.Utils.RenderRazorViewToString(this, "../../Views/Customer/SingleRegistration", registeredMember ?? new RegistrationDto { EmailID = email });
                return Json(new { html = single });
            }
            else
            {
                var registeredMember = await _customerService.GetRegistrationMembersByEmail(email, idEvent);
                if (registeredMember != null)
                {
                    registeredMember.RegistrationMembers = await _customerService.GetRegistrationMembers(registeredMember?.IdRegistration ?? 0);
                }
                ViewBag.IdEvent = idEvent;
                string group = Utils.Utils.RenderRazorViewToString(this, "../../Views/Customer/GroupRegistration", registeredMember ?? new RegistrationDto { EmailID = email });
                return Json(new { html = group });                
            }            
        }

        [HttpPost]
        public async Task<IActionResult> RegisterMember(RegistrationDto registrationDto)
        {
            //if (ModelState.IsValid)
            //{
            int idEvent = _session.GetEventId();
            registrationDto.IdEvent = idEvent;
            await _customerService.AddRegistrationMembers(registrationDto);
           // }

            //int idEvent = _session.GetEventId();
            int idVersion = _session.GetVersion();
            EventVersionDto eventVersionDto = _session.GetEvent();

            if (registrationDto.IdRegistration == 0)
            {
                string message = $@"
                                 <h3>Dear {registrationDto.Name},</h3>
                                 <p>
                                 We are delighted to confirm your registration for the {eventVersionDto.EventName}, 
                                 taking place at <b>{eventVersionDto.VenueName}</b>. Thank you for choosing to join us for this exciting event!
                                 </p>
                                 <h2>{eventVersionDto.EventStartDate?.ToString("dd-MM-yyyy")} - {eventVersionDto.EventEndDate?.ToString("dd-MM-yyyy")}</h2>
                                 <p>Please bring a printed or digital copy of your registration confirmation for smooth check-in.</p>
                                 </p>Dress code: Business casual</p>
                                 <p>If you have any questions or need further assistance, please do not hesitate to contact us at infomed.event@gmail.com</p>
                                 <p>Once again, thank you for registering. We look forward to welcoming you to for the conference and hope you have a memorable and productive experience.
                                 </p>
                                 <p>Warm regards,</p>
                                 <h3>Infomed Team</h3>
                                 <p>www.infomed.com</p>
                               ";
                await EmailService.SendMail(registrationDto.EmailID, message, "Infomed Conference Registration");
            }

            ViewBag.ConferenceFees = await _FeeService.GetConferenceFeesList(idEvent,idVersion);
            ViewBag.Payment = await _paymentService.GetPaymentDetails(idEvent,idVersion);
            ViewBag.Countries = await _commonService.GetCountries();

            if (registrationDto.RegType == "Single Registration")
            {
                var registeredMember = await _customerService.GetRegistrationMembersByEmail(registrationDto.EmailID, registrationDto.IdEvent);
                string single = Utils.Utils.RenderRazorViewToString(this, "../../Views/Customer/SingleRegistration", registeredMember);
                return Json(new { html = single });
            }
            else
            {
                var registeredMember = await _customerService.GetRegistrationMembersByEmail(registrationDto.EmailID, registrationDto.IdEvent);
                registeredMember.RegistrationMembers = await _customerService.GetRegistrationMembers(registeredMember.IdRegistration);
                string group = Utils.Utils.RenderRazorViewToString(this, "../../Views/Customer/GroupRegistration", registeredMember);
                return Json(new { html = group });

            }
          
        } 
    }
}

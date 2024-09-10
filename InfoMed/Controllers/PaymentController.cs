using InfoMed.Models;
using InfoMed.Services.Interface;
using InfoMed.Utils;
using Microsoft.AspNetCore.Mvc;

namespace InfoMed.Controllers
{
    public class PaymentController : Controller
    {
        private readonly Session _session;
        private readonly IPaymentService _payment;
        public PaymentController(Session session, IPaymentService payment)
        {

            _session = session;
            _payment = payment;
        }

        [HttpPost]
        public async Task<IActionResult> AddPaymentDetails(PaymentDetailsDto paymentDetailsDto)
        {
            var res = _session.GetUserDetails();
            if (ModelState.IsValid)
            {
                var eventId = _session.GetEventId();
                var eventVersion = _session.GetEventVersion();
                if (eventId <= 0)
                {
                    string eventDetails = Utils.Utils.RenderRazorViewToString(this, "../../Views/Payment/_AddPaymentPartial", paymentDetailsDto);
                    return Json(new { html = eventDetails, success = false });

                }
                else
                {
                    paymentDetailsDto.IdEvent = eventId;
                    paymentDetailsDto.IdEventVersion = eventVersion;
                }
                if (paymentDetailsDto.IdEventVersion > 0)
                {

                    if (paymentDetailsDto.PaymentMap != null && paymentDetailsDto.PaymentMap.Length > 0)
                    {
                        var imagePath = await SaveImageToFileSystem(paymentDetailsDto.PaymentMap, "Payment");
                        paymentDetailsDto.QRCodeImage = imagePath;
                    }
                    var payment = await _payment.AddPaymentDetails(paymentDetailsDto);
                    string paymentDetails = Utils.Utils.RenderRazorViewToString(this, "../../Views/Payment/_AddPaymentPartial", payment);
                    return Json(new { html = paymentDetails, success = true });
                }
                else
                {
                    {
                        string eventDetails = Utils.Utils.RenderRazorViewToString(this, "../../Views/Payment/_AddPaymentPartial", paymentDetailsDto);
                        return Json(new { html = eventDetails, success = false });
                    }

                }

            }
            else
            {
                string eventDetails = Utils.Utils.RenderRazorViewToString(this, "../../Views/Payment/_AddPaymentPartial", paymentDetailsDto);
                return Json(new { html = eventDetails, success = false });
            }

        }

        public async Task<string> SaveImageToFileSystem(IFormFile file, string moduleName)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", moduleName);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/uploads/{moduleName}/{uniqueFileName}";
        }

        public async Task<IActionResult> UpdatePaymentDetails(PaymentDetailsDto paymentDetailsDto)
        {
            ViewBag.sessionUser = _session.GetUserDetails();

            if (ModelState.IsValid)
            {
                if (paymentDetailsDto.PaymentMap != null && paymentDetailsDto.PaymentMap.Length > 0)
                {

                    var imagePath = await SaveImageToFileSystem(paymentDetailsDto.PaymentMap, "Payment");
                    paymentDetailsDto.QRCodeImage = imagePath;


                }
                else
                {
                    var paymenteGet = await _payment.GetPaymentDetails((int)paymentDetailsDto.IdEvent, paymentDetailsDto.IdEventVersion);
                    paymentDetailsDto.QRCodeImage = paymenteGet.QRCodeImage;
                }


                if (paymentDetailsDto.PaymentMap != null && paymentDetailsDto.PaymentMap.Length > 0)
                {
                    var imagePath = await SaveImageToFileSystem(paymentDetailsDto.PaymentMap, "Payment");
                    paymentDetailsDto.QRCodeImage = imagePath;
                }
                var payment = await _payment.UpdatePaymentDetails(paymentDetailsDto);
                string paymentDetails = Utils.Utils.RenderRazorViewToString(this, "../../Views/Payment/_AddPaymentPartial", payment);
                return Json(new { html = paymentDetails, success = true });
            }
            else
            {
                string eventDetails = Utils.Utils.RenderRazorViewToString(this, "../../Views/Event/_AddEventPartial", paymentDetailsDto);
                return Json(new { html = eventDetails, success = false });
            }
            //return View("Event", eventVersion);
        }

    }
}

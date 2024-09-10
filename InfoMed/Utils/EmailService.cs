using System.Net.Mail;
using System.Net;

namespace InfoMed.Utils
{
    public static class EmailService
    {
        public static async Task<bool> SendMail(string email, string message, string subject)
        {
            try
            {
                using (var client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential("infomed.event@gmail.com", "ihcunwntltqndnod");

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("infomed.event@gmail.com", "InfoMed"),
                        Subject = subject,
                        Body = message,
                        IsBodyHtml = true,
                    };

                    mailMessage.To.Add(email);
                    await client.SendMailAsync(mailMessage);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

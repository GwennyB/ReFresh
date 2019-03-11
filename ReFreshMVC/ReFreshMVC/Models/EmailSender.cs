using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace ReFreshMVC.Models
{
    public class EmailSender : IEmailSender
    {
        private IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// assembles and sends an email
        /// </summary>
        /// <param name="email"> recipient email address </param>
        /// <param name="subject"> email subject </param>
        /// <param name="message">email contents </param>
        /// <returns> task completed </returns>
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            SendGridClient client = new SendGridClient(_configuration.GetConnectionString("SendGridAPIKey"));
            SendGridMessage msg = new SendGridMessage();
            msg.SetFrom("refreshfoods401@gmail.com", "ReFresh Foods");

            msg.AddTo(email);
            msg.SetSubject(subject);
            msg.AddContent(MimeType.Html, message);

            var status = await client.SendEmailAsync(msg);

        }
    }
}

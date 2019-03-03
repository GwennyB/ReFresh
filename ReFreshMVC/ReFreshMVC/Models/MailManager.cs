using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReFreshMVC.Models
{
    public class MailManager : IEmailSender
    {
        private IConfiguration _configuration;

        public MailManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            SendGridClient client = new SendGridClient(_configuration["SendGridAPIKey"]);
            SendGridMessage msg = new SendGridMessage();
            msg.SetFrom("refreshfoods401@gmail.com", "ReFresh Foods");

            msg.AddTo(email);
            msg.SetSubject(subject);
            msg.AddContent(MimeType.Html, message);

            await client.SendEmailAsync(msg);

        }
    }
}

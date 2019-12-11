using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class EmailSettings
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string Address { get; set; }
        public string DisplayName { get; set; }
        public string TargetName { get; set; }
    }
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var smtp = new SmtpClient(_emailSettings.Host, _emailSettings.Port)
                {
                    EnableSsl = _emailSettings.EnableSsl,
                    UseDefaultCredentials = false,
                    Timeout = 10000,
                    Credentials = new NetworkCredential(_emailSettings.UserName, _emailSettings.Password),
                    TargetName = _emailSettings.TargetName,
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                smtp.ServicePoint.MaxIdleTime = 1;

                string address = _emailSettings.Address;
                string displayName = _emailSettings.DisplayName;

                var from = new MailAddress(address, displayName, Encoding.UTF8);
                var to = new MailAddress(email);

                var message = new MailMessage(from, to)
                {
                    Subject = subject,
                    SubjectEncoding = Encoding.UTF8,
                    IsBodyHtml = true,
                    BodyEncoding = Encoding.UTF8,
                    Body = htmlMessage
                };

                message.Bcc.Add("info@d3v.mx");

                //message.To.Add(to);

                await smtp.SendMailAsync(message);
                message.Dispose();
            }
            catch (Exception ex)
            {
                // TODO: handle exception
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}

using healthspanmd.core.Services.Messaging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.email
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSenderOptions _options;
        private readonly ILogger _logger;

        public EmailSender(
            IOptions<EmailSenderOptions> optionsAccessor,
            ILogger<EmailSender> logger
            )
        {
            _options = optionsAccessor.Value;
            _logger = logger;
        }


        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailTask = SendEmailAsync(email, subject, htmlMessage, _options, true);
            await emailTask;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage, EmailSenderOptions senderOptions)
        {
            var emailTask = SendEmailAsync(email, subject, htmlMessage, senderOptions, true);
            await emailTask;
        }

        //public async Task SendEmailAsync(string email, string subject, string htmlMessage, EmailSenderOptions senderOptions, bool isHtml, ICollection<Attachment> attachments)
        //{
        //    var emailTask = SendEmailAsync(email, subject, htmlMessage, senderOptions, true, attachments);
        //    await emailTask;
        //}

        public async Task SendEmailAsync(string email, string subject, AlternateView htmlView, ICollection<Attachment> attachments = null)
        {
            var emailTask = SendEmailAsync(email, subject, null, null, true, htmlView, attachments);
            await emailTask;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage, EmailSenderOptions senderOptions, bool isHtml, AlternateView htmlView = null, ICollection<Attachment> attachments = null)
        {
            try
            {
                if (senderOptions == null)
                {
                    senderOptions = _options;
                }

                SmtpClient client = new SmtpClient(senderOptions.SMTPServer);
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderOptions.SMTPUsername, senderOptions.SMTPPassword);
                client.Port = senderOptions.SMTPPort;
                client.EnableSsl = senderOptions.SMTPEnableSSL;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage mailMessage = new MailMessage()
                {
                    From = new MailAddress(senderOptions.SMTPFrom),
                    Body = htmlMessage,
                    Subject = subject,
                    IsBodyHtml = isHtml
                };
                mailMessage.AlternateViews.Add(htmlView);
                if (attachments != null)
                {
                    foreach (var attachment in attachments)
                    {
                        mailMessage.Attachments.Add(attachment);
                    }

                }
                mailMessage.To.Add(email);
                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            
        }




    }
}

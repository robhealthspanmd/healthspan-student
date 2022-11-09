using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using healthspanmd.core.Services.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.gmail
{
    public class GmailSender : IEmailSender
    {

        private readonly IConfiguration _config;
        private readonly ILogger<GmailSender> _logger;
        private readonly ILoggerFactory _loggerFactory;

        public GmailSender(
            IConfiguration config,
            ILoggerFactory loggerFactory
            )
        {
            _config = config;
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<GmailSender>();
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            await Task.Run(() => SendEmail(email, subject, htmlMessage, new EmailSenderOptions(), false));
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage, EmailSenderOptions senderOptions)
        {
            await Task.Run(() => SendEmail(email, subject, htmlMessage, senderOptions, false));
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage, EmailSenderOptions senderOptions, bool isHtml)
        {
            await Task.Run(() => SendEmail(email, subject, htmlMessage, senderOptions, isHtml));
        }


        public async Task SendEmailAsync(string email, string subject, System.Net.Mail.AlternateView htmlView, ICollection<System.Net.Mail.Attachment> attachments = null)
        {
            await Task.Run(() => SendEmail(email, subject, null, new EmailSenderOptions(), true, htmlView, attachments));
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage, EmailSenderOptions senderOptions, bool isHtml, System.Net.Mail.AlternateView htmlView = null, ICollection<System.Net.Mail.Attachment> attachments = null)
        {
            await Task.Run(() => SendEmail(email, subject, htmlMessage, senderOptions, isHtml, htmlView, attachments));
        }

        private void SendEmail(string email, string subject, string htmlMessage, EmailSenderOptions senderOptions, bool isHtml, System.Net.Mail.AlternateView htmlView = null, ICollection<System.Net.Mail.Attachment> attachments = null)
        {
            // prep messsage
            var mailMessage = new System.Net.Mail.MailMessage();
            mailMessage.From = new System.Net.Mail.MailAddress(_config.GetSection("GoogleApiSetting:FromEmail").Value);
            mailMessage.To.Add(email);
            mailMessage.Subject = subject;
            mailMessage.Body = htmlMessage;
            mailMessage.IsBodyHtml = isHtml;

            mailMessage.AlternateViews.Add(htmlView);
            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    mailMessage.Attachments.Add(attachment);
                }

            }

            var mimeMessage = MimeMessage.CreateFromMailMessage(mailMessage);

            var gmailMessage = new Google.Apis.Gmail.v1.Data.Message
            {
                Raw = EncodeMimeMessageToBase64(mimeMessage)
            };

            // get service
            var service = GetGmailService();

            // send message
            UsersResource.MessagesResource.SendRequest request = service.Users.Messages.Send(gmailMessage, _config.GetSection("GoogleApiSetting:FromEmail").Value);
            var result = request.Execute();

        }

        private GmailService GetGmailService()
        {
            var credPath = _config.GetSection("GoogleApiSetting:PathToKeyFile").Value;
            var appName = _config.GetSection("GoogleApiSetting:ApplicationName").Value;
            GoogleCredential credential;

            using (var stream = new FileStream(credPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential
                    .FromStream(stream)
                    .CreateScoped(GmailService.Scope.MailGoogleCom)
                    .CreateWithUser(_config.GetSection("GoogleApiSetting:FromEmail").Value);
            }


            // Create Gmail API service.
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = appName,
            });

            return service;
        }

        private string EncodeMimeMessageToBase64(MimeMessage mimeMessage)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                mimeMessage.WriteTo(ms);
                return Convert.ToBase64String(ms.GetBuffer())
                    .TrimEnd('=')
                    .Replace('+', '-')
                    .Replace('/', '_');
            }
        }

    }
}

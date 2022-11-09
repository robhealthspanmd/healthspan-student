using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using infrastructure.notifications.Models;
using Razor.Templating.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using healthspanmd.core.Services.Messaging;

namespace infrastructure.notifications
{
    public class HtmlEmailSender : IHtmlEmailSender
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<HtmlEmailSender> _logger;

        public HtmlEmailSender(
            IEmailSender emailSender,
            ILogger<HtmlEmailSender> logger
            )
        {
            _emailSender = emailSender;         
            _logger = logger;
        }


        public async Task SendGenericEmailMessageAsync(string recipientEmail, string subject, string messageHtml)
        {
            var emailHtml = await RazorTemplateEngine.RenderAsync("~/Views/GenericEmailMessage.cshtml",
                                new GenericEmailMessageViewModel
                                {
                                    EmailMessageHtml = messageHtml
                                });
            var response = ImageProcessor.EmbedImages(emailHtml);
            _logger.LogInformation($"queue up confirmation email for {recipientEmail}");
            var emailTask = _emailSender.SendEmailAsync(recipientEmail, subject, response.htmlView);
        }

        
        
    }
}

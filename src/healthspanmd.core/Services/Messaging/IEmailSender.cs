using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.Services.Messaging
{ 
   public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
        Task SendEmailAsync(string email, string subject, string htmlMessage, EmailSenderOptions senderOptions);
        Task SendEmailAsync(string email, string subject, AlternateView htmlView, ICollection<Attachment> attachments = null);
        Task SendEmailAsync(string email, string subject, string htmlMessage, EmailSenderOptions senderOptions, bool isHtml, AlternateView htmlView = null, ICollection<Attachment> attachments = null);
    }
}

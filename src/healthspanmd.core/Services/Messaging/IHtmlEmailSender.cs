using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.Services.Messaging
{
    public interface IHtmlEmailSender
    {
        Task SendGenericEmailMessageAsync(string recipientEmail, string subject, string messageHtml);
    }
}

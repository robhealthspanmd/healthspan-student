using healthspanmd.core.Services.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.mailerlite
{
    public class MailerliteSender : ITrackedEmailSender
    {
        public TrackedEmailResult sendTrackedEmail(TrackedEmailRequest request)
        {
            //do actual work
            throw new NotImplementedException();
        }
    }
}

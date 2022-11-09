using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.Notifications
{
    public class NotificationTemplateModel
    {
        public int NotificationTemplateId { get; set; }
        public NotificationType NotificationType { get; set; }
        public string Template { get; set; }
    }
}

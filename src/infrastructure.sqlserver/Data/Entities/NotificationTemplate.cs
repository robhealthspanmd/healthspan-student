using healthspanmd.core.CQRS.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data.Entities
{
    public class NotificationTemplate
    {
        public int NotificationTemplateId { get; set; }
        public NotificationType NotificationType { get; set; }
        public string Template { get; set; }
    }
}

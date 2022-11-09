using healthspanmd.core.CQRS.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data.Entities
{
    public class Notification
    {
        public long NotificationId { get; set; }
        public long UserId { get; set; }
        public DateTime SentDateTimeUtc { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}

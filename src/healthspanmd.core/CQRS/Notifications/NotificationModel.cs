using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.Notifications
{
    public class NotificationModel
    {
        public long NotificationId { get; set; }
        public long UserId { get; set; }
        public DateTime SentDateTimeUtc { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}

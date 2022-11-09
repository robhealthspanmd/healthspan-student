using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.Notifications
{
    public class GetNotificationListQueryFilter
    {
        public long? UserId { get; set; }
        public DateTime? SentOnOrAfterDateTimeUtc { get; set; }
        public NotificationType? NotificationType { get; set; }
    }
}

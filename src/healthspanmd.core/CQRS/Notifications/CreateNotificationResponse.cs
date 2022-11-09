using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.Notifications
{
    public class CreateNotificationResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public NotificationModel Notification { get; set; }
    }
}

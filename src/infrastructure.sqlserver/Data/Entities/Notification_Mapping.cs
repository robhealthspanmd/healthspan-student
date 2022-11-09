using healthspanmd.core.CQRS.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data.Entities
{
    public static class Notification_Mapping
    {
        public static NotificationModel ToNotificationModel(this Notification n)
        {
            return new NotificationModel
            {
                NotificationId = n.NotificationId,
                NotificationType = n.NotificationType,
                SentDateTimeUtc = n.SentDateTimeUtc,
                UserId = n.UserId,
            };
        }

        public static Notification ToNotification(this NotificationModel n)
        {
            return new Notification
            {
                NotificationId = n.NotificationId,
                NotificationType = n.NotificationType,
                SentDateTimeUtc = n.SentDateTimeUtc,
                UserId = n.UserId
            };
        }
    }
}

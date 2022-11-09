using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.Notifications
{
    public interface INotificationQueries
    {
        ICollection<NotificationModel> GetList(GetNotificationListQueryFilter filter);
        NotificationTemplateModel GetNotificationTemplate(NotificationType notificationType);
        NotificationModel GetNotification(long notificationId);
    }
}

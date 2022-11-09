using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.Notifications
{
    public interface INotificationCommands
    {
        CreateNotificationResponse CreateNotification(NotificationModel model);
    }
}

using healthspanmd.core.CQRS.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data.Entities
{
    public static class NotificationTemplate_Mapping
    {
        public static NotificationTemplateModel ToNotificationTemplateModel(this NotificationTemplate t)
        {
            return new NotificationTemplateModel
            {
                NotificationTemplateId = t.NotificationTemplateId,
                NotificationType = t.NotificationType,
                Template = t.Template,
            };
        }

        public static NotificationTemplate ToNotificationTemplate(this NotificationTemplateModel t)
        {
            return new NotificationTemplate
            {
                NotificationTemplateId = t.NotificationTemplateId,
                NotificationType = t.NotificationType,
                Template = t.Template,
            };
        }
    }
}

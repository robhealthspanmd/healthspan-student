using healthspanmd.core.CQRS.Notifications;
using healthspanmd.core.Services.ExceptionReporting;
using infrastructure.sqlserver.Data;
using infrastructure.sqlserver.Data.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.CQRS.Notifications
{
    public class NotificationQueries : INotificationQueries
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public NotificationQueries(
            IServiceScopeFactory scopeFactory
            )
        {
            _scopeFactory = scopeFactory;
        }

        public ICollection<NotificationModel> GetList(GetNotificationListQueryFilter filter)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
                var query = dbContext.Notifications.AsQueryable();


                if (filter.UserId.HasValue)
                    query = query.Where(n => n.UserId == filter.UserId.Value);

                if (filter.NotificationType.HasValue)
                    query = query.Where(n => n.NotificationType == filter.NotificationType.Value);

                if (filter.SentOnOrAfterDateTimeUtc.HasValue)
                    query = query.Where(n => n.SentDateTimeUtc >= filter.SentOnOrAfterDateTimeUtc.Value);

                return query.Select(n => n.ToNotificationModel()).ToList();
                    

            }    
        }

        public NotificationModel GetNotification(long notificationId)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
                var result = dbContext.Notifications.Find(notificationId);


                return result.ToNotificationModel();
            }
        }

        public NotificationTemplateModel GetNotificationTemplate(NotificationType notificationType)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
                var result = dbContext.NotificationTemplates
                    .Where(t => t.NotificationType == notificationType)
                    .FirstOrDefault();


                return result.ToNotificationTemplateModel();


            }
        }
    }
}

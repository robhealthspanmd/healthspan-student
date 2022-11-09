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
    public class NotificationCommands : INotificationCommands
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IExceptionReporter _exceptionReporter;
        public NotificationCommands(
            IServiceScopeFactory scopeFactory,
            IExceptionReporter exceptionReporter
            )
        {
            _scopeFactory = scopeFactory;
            _exceptionReporter = exceptionReporter;
        }

        public CreateNotificationResponse CreateNotification(NotificationModel model)
        {
            var response = new CreateNotificationResponse();
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();

                    var notification = model.ToNotification();
                    dbContext.Notifications.Add(notification);
                    dbContext.SaveChanges();

                    response.Success = true;
                    response.Notification = notification.ToNotificationModel();

                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                _exceptionReporter.Execute(ex, this.GetType());
            }

            return response;
        }
    }
}

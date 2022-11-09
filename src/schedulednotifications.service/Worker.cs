using healthspanmd.core.CQRS.Notifications;
using healthspanmd.core.CQRS.Users;
using healthspanmd.core.Services.Messaging;
using healthspanmd.core.Services.SMS;

namespace schedulednotifications.service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ISMSProvider _smsProvider;
        private readonly IUserQueries _userQueries;
        private readonly INotificationQueries _notificationQueries;
        private readonly INotificationCommands _notificationCommands;
        private readonly IEmailSender _emailSender;

        public Worker(
            ILogger<Worker> logger,
            ISMSProvider smsProvider,
            IUserQueries userQueries,
            INotificationQueries notificationQueries,
            INotificationCommands notificationCommands,
            IEmailSender emailSender
            )
        {
            _logger = logger;
            _smsProvider = smsProvider;
            _userQueries = userQueries;
            _notificationQueries = notificationQueries;
            _notificationCommands = notificationCommands;
            _emailSender = emailSender;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var launchMsg = _smsProvider.Send(new SendRequest
            {
                Message = "The app is running",
                    PhoneNumber = "+14805290732"
            });
            Console.WriteLine("Worker has started.");
            while (!stoppingToken.IsCancellationRequested)
            {
                if (DateTime.UtcNow.Minute == 0)
                {
                    Console.WriteLine($"Worker running at: {DateTime.UtcNow.ToString()}");
                }
                try
                {
                    // get notification template
                    var notificationTemplate = _notificationQueries.GetNotificationTemplate(NotificationType.DaillyTracker);
                    //Console.WriteLine($"notificationTemplate: {notificationTemplate}");

                    // go through each client that is within the program active window
                    // check for daily tracker notifications to be sent
                    var users = _userQueries.GetUserList(new GetUserListQueryFilter
                    {
                        IsActive = true,
                        IsInProgramWindow = true,
                    });


                    foreach (var user in users)
                    {
                        Console.WriteLine($"Considering user {user.FullName}");
                        bool shouldSendNotification = false;

                        
                        // does user have a daily active metric?
                        if (user.ActiveMetrics.Any(am => am.Frequency == healthspanmd.core.CQRS.Metrics.MetricFrequencyType.Daily))
                        {
                            Console.WriteLine($"{user.FullName} is eligible for a daily notification");
                            // get current notification DateTime
                            // we can define today (Date) using server time

                            // get User's time zone
                            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(user.TimeZoneId);

                            // what is current time in user's timezone
                            var currentTimeInUsersTimeZone = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, userTimeZone);
                            Console.WriteLine($"Current Time in users timezone is {currentTimeInUsersTimeZone.ToString()}");

                            // when should user recieve a notification on this day?
                            var notificationDateTime = DateTime.SpecifyKind(currentTimeInUsersTimeZone.Date + user.NotificationTime.TimeOfDay, DateTimeKind.Unspecified);
                            Console.WriteLine($"Users notification datetime is {notificationDateTime.ToString()}");

                            var notificationDateTimeUtc = TimeZoneInfo.ConvertTimeToUtc(notificationDateTime, userTimeZone);
                            Console.WriteLine($"{user.FullName}'s notificationDateTimeUtc = {notificationDateTimeUtc.ToString()}");
                            

                            if (DateTime.UtcNow >= notificationDateTimeUtc)
                            {
                                // check for a notification to this user that has already been sent on or after the notification dateTime
                                // existing notification: 4/6/2022 5:46:00 PM
                                var existingNotification = user.Notifications
                                    .Where(n => n.NotificationType == NotificationType.DaillyTracker
                                                && n.SentDateTimeUtc >= notificationDateTimeUtc).FirstOrDefault();

                                if (existingNotification == null)
                                {
                                    // we are cleared to send the notification
                                    shouldSendNotification = true;
                                }
                                else
                                {
                                    Console.WriteLine($"Already found a notification {existingNotification.NotificationId} that was sent on {existingNotification.SentDateTimeUtc.ToString()} after {user.FullName}'s notificationDateTimeUtc");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Not yet time for a notification to {user.FullName}");
                            }
                        }
                        else if (user.ActiveMetrics.Any(am => am.Frequency == healthspanmd.core.CQRS.Metrics.MetricFrequencyType.Weekly))
                        {
                            //Console.WriteLine($"{user.FullName} is eligible for a weekly notification");
                            var todaysActiveMetric = user.ActiveMetrics
                                .Where(am => am.Frequency == healthspanmd.core.CQRS.Metrics.MetricFrequencyType.Weekly
                                            && am.DayOfWeek == DateTime.Today.DayOfWeek)
                                .FirstOrDefault();
                            if (todaysActiveMetric != null)
                            {
                                // get current notification DateTime
                                // we can define today (Date) using server time
                                var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(user.TimeZoneId);
                                var notificationDateTime = DateTime.SpecifyKind(DateTime.Today + user.NotificationTime.TimeOfDay, DateTimeKind.Unspecified);
                                var notificationDateTimeUtc = TimeZoneInfo.ConvertTimeToUtc(notificationDateTime, userTimeZone);

                                if (DateTime.UtcNow >= notificationDateTimeUtc)
                                {
                                    // check for a notification to this user that has already been sent on or after the notification dateTime
                                    var existingNotification = user.Notifications
                                        .Where(n => n.NotificationType == NotificationType.DaillyTracker
                                                    && n.SentDateTimeUtc >= notificationDateTimeUtc).FirstOrDefault();

                                    if (existingNotification == null)
                                    {
                                        // we are cleared to send the notification
                                        shouldSendNotification = true;
                                    }
                                }
                            }
                            
                        }
                        else if (user.ActiveMetrics.Any(am => am.Frequency == healthspanmd.core.CQRS.Metrics.MetricFrequencyType.Monthly))
                        {
                            // TODO: do we need to implement a monthly frequency notification?
                            Console.WriteLine($"{user.FullName} is NOT eligible for a notification");
                        }

                        if (shouldSendNotification)
                        {
                            // we are cleared to send the notification
                            Console.WriteLine($"Time to send message to {user.FullName}");
                            var msg = notificationTemplate.Template
                                .Replace("{firstname}", user.FirstName);

                            SendResponse result = null;

                            // check for SMS notification
                            if (user.NotificationBySMS)
                            {
                                result = _smsProvider.Send(new SendRequest
                                {
                                    Message = msg,
                                    PhoneNumber = user.PhoneNumberAs10DigitLongCode
                                });
                            }

                            // could also be by email (secondary)
                            if (user.NotificationByEmail)
                            {
                                var task = _emailSender.SendEmailAsync(user.Email, "Notification from HealthspanMD", msg);
                                if (result == null)
                                {
                                    result = new SendResponse
                                    {
                                        Success = true,
                                    };
                                }
                            }


                            if (result.Success)
                            {
                                _notificationCommands.CreateNotification(new NotificationModel
                                {
                                    NotificationType = NotificationType.DaillyTracker,
                                    SentDateTimeUtc = DateTime.UtcNow,
                                    UserId = user.UserId
                                });
                            }
                            else
                            {
                                Console.WriteLine($"Unable to send message to {user.FullName}");
                                Console.WriteLine($"SMS Sender response: {result.Message}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    var exMsg = _smsProvider.Send(new SendRequest
                    {
                        Message = ex.Message,
                        PhoneNumber = "+14805290732"
                    });
                }

                await Task.Delay(60000, stoppingToken);
            }
        }
    }
}
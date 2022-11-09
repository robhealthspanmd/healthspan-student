using healthspanmd.core.CQRS.Content;
using healthspanmd.core.CQRS.Notifications;
using healthspanmd.core.CQRS.Users;
using healthspanmd.core.Services.SMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.Services.Messaging
{
    public class NotificationSender : INotificationSender
    {
        private readonly ISMSProvider _smsProvider;
        private readonly INotificationCommands _notificationCommands;
        private readonly IUserCommands _userCommands;
        private readonly IEmailSender _emailSender;
        private readonly IHtmlEmailSender _htmlEmailSender;

        public NotificationSender(
            ISMSProvider smsProvider,
            INotificationCommands notificationCommands,
            IUserCommands userCommands,
            IEmailSender emailSender,
            IHtmlEmailSender htmlEmailSender
            )
        {
            _smsProvider = smsProvider;
            _notificationCommands = notificationCommands;
            _userCommands = userCommands;
            _emailSender = emailSender;
            _htmlEmailSender = htmlEmailSender;
        }


        public SendNotificationResult SendContentNotification(UserModel targetUser, ContentCardModel contentCard)
        {
            SendNotificationResult result = new SendNotificationResult();
            try
            {
                var existingAssignment = targetUser.ContentCardAssignments
                    .Where(a => a.ContentCardId == contentCard.ContentCardId)
                    .FirstOrDefault();

                // resolve message (default or personalized)
                var message = contentCard.NotificationMessage;
                if (existingAssignment.NotificationMessage != null)
                    message= existingAssignment.NotificationMessage;


                // send the message (SMS and/or Email)
                SendResponse sendResponse = null;
                if (targetUser.NotificationBySMS)
                {
                    sendResponse = _smsProvider.Send(new SendRequest
                    {
                        Message = GetNewContentSMSMessage(targetUser, contentCard, message),
                        PhoneNumber = targetUser.PhoneNumberAs10DigitLongCode
                    });
                }
                
                if (targetUser.NotificationByEmail)
                {
                    var task = _htmlEmailSender.SendGenericEmailMessageAsync(
                        targetUser.Email, 
                        "New content at HealthspanMD", 
                        GetNewContentEmailHtmlMessage(targetUser, contentCard, message)
                    );


                    // we are not going to wait for the email, we will assume it worked
                    if (sendResponse == null)
                        sendResponse = new SendResponse { Success = true };
                }

                if (sendResponse.Success)
                {
                    // audit tracking of the notification
                    var notificationResponse = _notificationCommands.CreateNotification(new NotificationModel
                    {
                        NotificationType = NotificationType.NewContent,
                        SentDateTimeUtc = DateTime.UtcNow,
                        UserId = targetUser.UserId
                    });

                    // mark that the notification has been sent on the assignment
                    _userCommands.MarkContentCardAssignmentNotification(existingAssignment.ContentCardAssignmentId, notificationResponse.Notification.NotificationId);

                    result.Success = true;
                }
                else
                {
                    result.Success = false;
                    result.Message = sendResponse.Message;
                }

                
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }

        private string GetNewContentSMSMessage(UserModel targetUser, ContentCardModel contentCard, string message)
        {
            var link = $"https://platform.healthspanmd.com/Content/{contentCard.ContentCardId}";
            var messageBody = message.Replace("{firstname}", targetUser.FirstName);
            return messageBody
                + Environment.NewLine + Environment.NewLine
                + link;
        }

        private string GetNewContentEmailHtmlMessage(UserModel targetUser, ContentCardModel contentCard, string message)
        {
            var link = $"<a href=\"https://platform.healthspanmd.com/Content/{contentCard.ContentCardId}\">{contentCard.Name}</a>";
            var messageBody = message
                .Replace(Environment.NewLine, "<br />")
                .Replace("{firstname}", targetUser.FirstName);
            return messageBody
                + "<br /><br />"
                + link;
        }
    }
}

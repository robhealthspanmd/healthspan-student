using healthspanmd.core.CQRS.Content;
using healthspanmd.core.CQRS.Notifications;
using healthspanmd.core.CQRS.Users;

namespace healthspanmd.web.Models.Account
{
    public class PersonalizedContentNotificationDialogViewModel
    {
        public ContentCardAssignmentModel ContentCardAssignment { get; set; }
        public NotificationModel Notification { get; set; }
        public ContentCardModel ContentCard { get; set; }
        public UserModel TargetUser { get; set; }
    }
}

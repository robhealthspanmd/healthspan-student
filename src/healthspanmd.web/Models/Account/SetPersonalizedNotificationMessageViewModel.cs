namespace healthspanmd.web.Models.Account
{
    public class SetPersonalizedNotificationMessageViewModel
    {
        public long UserId { get; set; }
        public int ContentCardId { get; set; }
        public string NotificationMessage { get; set; }
    }
}

using healthspanmd.core.CQRS.Content;
using healthspanmd.core.CQRS.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.Services.Messaging
{
    public interface INotificationSender
    {
        SendNotificationResult SendContentNotification(UserModel targetUser, ContentCardModel contentCard);
    }
}

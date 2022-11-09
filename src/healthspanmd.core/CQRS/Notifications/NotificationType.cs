using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.Notifications
{
    public enum NotificationType
    {
        DaillyTracker = 0,
        ProgramIsEndingWarning = 1,
        InvitationToRegister = 2,
        NewContent = 3
    }
}

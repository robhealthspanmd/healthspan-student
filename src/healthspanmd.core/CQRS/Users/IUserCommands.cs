using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.Users
{
    public interface IUserCommands
    {
        UpdateUserResponse CreateOrUpdateUser(UserModel model, UserValidator validator);
        UpdateUserResponse DeactivateUser(long userId);
        UpdateUserResponse DeleteUser(long userId);
        UpdateUserResponse ActivateUser(long userId);
        UpdateUserResponse AddAuthorizedRole(long userId, int userRoleId);
        UpdateUserResponse AddActiveMetric(long userId, int metricId);
        UpdateUserResponse RemoveActiveMetric(long userId, int metricId);
        UpdateUserResponse CreateContentCardAssignment(UserModel model, int contentCardId, int assignedByUserId);
        UpdateUserResponse RemoveContentCardAssignment(long userId, int contentCardAssignmentId);
        UpdateUserResponse UpdateContentCardAssignments(UserModel model);
        void MarkContentCardAssignmentAsComplete(int assignmentId);
        void MarkContentCardAssignmentAsComplete(long userId, int contentCardId);
        void UpdatePersonalizedContentNotificationMessage(int assignmentId, string notificationMessage);
        void MarkContentCardAssignmentNotification(int assignmentId, long notificationId);
    }
}

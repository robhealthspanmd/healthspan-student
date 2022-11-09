using healthspanmd.core.CQRS.Users;
using healthspanmd.shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data.Entities
{
    public static class User_Mapping
    {
        public static UserModel ToUserModel(this User u)
        {
            if (u == null)
                return null;

            return new UserModel
            {
                UserId = u.UserId,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                PhoneCountryCode = u.PhoneCountryCode,
                Phone = u.Phone,
                IdentityUserId = u.IdentityUserId,
                IsActive = u.IsActive,
                NotificationByEmail = u.NotificationByEmail,
                NotificationBySMS = u.NotificationBySMS,
                ProgramEndDate = u.ProgramEndDate,
                ProgramStartDate = u.ProgramStartDate,
                TimeZoneId = u.TimeZoneId,
                NotificationTime = u.NotificationTime,
                ActiveMetrics = u.ActiveMetrics != null ? u.ActiveMetrics.Select(am => am.ToActiveMetricModel()).ToList() : null,
                AuthorizedRoles = u.AuthorizedRoles != null ? u.AuthorizedRoles.Select(ar => ar.ToAuthorizedRoleModel()).ToList() : null,
                MetricTrackingEntries = u.MetricTrackingEntries != null ? u.MetricTrackingEntries.Select(mte => mte.ToMetricTrackingEntryModel()).ToList() : null,
                Notifications = u.Notifications != null ? u.Notifications.Select(n => n.ToNotificationModel()).ToList() : null,
                ContentCardAssignments = u.ContentCardAssignments != null ? u.ContentCardAssignments.Select(a => a.ToContentCardAssignmentModel()).ToList() : null,
            };
        }


        public static User ToUser(this UserModel m, bool includeRelatedEntities)
        {
            if (m == null)
                return null; 

            var result = new User
            {
                UserId = m.UserId,
                FirstName = m.FirstName,
                LastName = m.LastName,
                Email = m.Email,
                PhoneCountryCode = m.PhoneCountryCode,
                Phone = m.Phone,
                IdentityUserId = m.IdentityUserId,
                IsActive = m.IsActive,
                NotificationByEmail = m.NotificationByEmail,
                NotificationBySMS = m.NotificationBySMS,
                ProgramEndDate = m.ProgramEndDate,
                ProgramStartDate = m.ProgramStartDate,
                TimeZoneId = m.TimeZoneId,
                NotificationTime = m.NotificationTime,
            };

            if (includeRelatedEntities)
            {
                result.ActiveMetrics = m.ActiveMetrics != null ? m.ActiveMetrics.Select(am => am.ToActiveMetric()).ToList() : null;
                result.AuthorizedRoles = m.AuthorizedRoles != null ? m.AuthorizedRoles.Select(ur => ur.ToAuthorizedRole()).ToList() : null;
                result.MetricTrackingEntries = m.MetricTrackingEntries != null ? m.MetricTrackingEntries.Select(mte => mte.ToMetricTrackingEntry()).ToList() : null;
                result.Notifications = m.Notifications != null ? m.Notifications.Select(n => n.ToNotification()).ToList() : null;
                result.ContentCardAssignments = m.ContentCardAssignments != null ? m.ContentCardAssignments.Select(a => a.ToContentCardAssignment()).ToList() : null;
            }

            return result;
        }
    }
}

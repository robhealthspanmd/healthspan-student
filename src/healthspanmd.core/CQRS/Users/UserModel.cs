using healthspanmd.core.CQRS.ActiveMetrics;
using healthspanmd.core.CQRS.AuthorizedRoles;
using healthspanmd.core.CQRS.Content;
using healthspanmd.core.CQRS.MetricTrackingEntries;
using healthspanmd.core.CQRS.Notifications;
using healthspanmd.core.CQRS.UserRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.Users
{
    public class UserModel
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneCountryCode { get; set; }
        public string Phone { get; set; }
        public string IdentityUserId { get; set; }
        public DateTime ProgramStartDate { get; set; }
        public DateTime ProgramEndDate { get; set; }
        public bool IsActive { get; set; }
        public bool NotificationBySMS { get; set; }
        public bool NotificationByEmail { get; set; }
        public string TimeZoneId { get; set; }
        public DateTime NotificationTime { get; set; }
        public ICollection<AuthorizedRoleModel> AuthorizedRoles { get; set; }
        public ICollection<ActiveMetricModel> ActiveMetrics { get; set; }
        public ICollection<MetricTrackingEntryModel> MetricTrackingEntries { get; set; }
        public ICollection<NotificationModel> Notifications { get; set; }
        public ICollection<ContentCardAssignmentModel> ContentCardAssignments { get; set; }

        public ICollection<ContentCardAssignmentModel> ActiveContentCardAssignments
        {
            get
            {
                return ContentCardAssignments
                    .Where(a => !a.CompletedUtc.HasValue)
                    .ToList();
            }
        }
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        public string AuthorizedRoleList
        {
            get
            {
                return string.Join(", ", AuthorizedRoles.Select(r => r.UserRole.Name));
            }
        }

        public string PhoneNumberAs10DigitLongCode
        {
            get
            {
                string phoneNumber = PhoneCountryCode + Phone;
                if (!phoneNumber.StartsWith("+"))
                    phoneNumber = "+" + phoneNumber;
                return phoneNumber
                    .Replace("-", "")
                    .Replace(".", "")
                    .Replace(" ", "")
                    .Replace("(", "")
                    .Replace(")", "");
            }
        }
    }
}

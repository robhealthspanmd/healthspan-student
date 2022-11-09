using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data.Entities
{
    public class User
    {
        public long UserId { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(150)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string Phone { get; set; }

        [MaxLength(10)]
        public string PhoneCountryCode { get; set; }

        [MaxLength(100)]
        public string IdentityUserId { get; set; }
        public ICollection<AuthorizedRole> AuthorizedRoles { get; set; }
        public ICollection<ActiveMetric> ActiveMetrics { get; set; }
        public ICollection<MetricTrackingEntry> MetricTrackingEntries { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<ContentCardAssignment> ContentCardAssignments { get; set; }

        public DateTime ProgramStartDate { get; set; }
        public DateTime ProgramEndDate { get; set; }
        public bool IsActive { get; set; }
        public bool NotificationBySMS { get; set; }
        public bool NotificationByEmail { get; set; }

        [MaxLength(150)]
        public string TimeZoneId { get; set; }
        public DateTime NotificationTime { get; set; }

    }
}

using System;
using System.Collections.Generic;

namespace healthspanmd.web.Models.Account
{
    public class UserViewModel
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneCountryCode { get; set; }
        public string Phone { get; set; }
        public DateTime ProgramStartDate { get; set; }
        public DateTime ProgramEndDate { get; set; }
        public bool IsActive { get; set; }
        public bool NotificationBySMS { get; set; }
        public bool NotificationByEmail { get; set; }
        public string TimeZoneId { get; set; }
        public DateTime NotificationTime { get; set; }
        public List<SelectedMetricViewModel> SelectedMetrics { get; set; }
    }
}

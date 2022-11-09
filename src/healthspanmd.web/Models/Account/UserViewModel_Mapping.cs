using healthspanmd.core.CQRS.ActiveMetrics;
using healthspanmd.core.CQRS.AuthorizedRoles;
using healthspanmd.core.CQRS.Metrics;
using healthspanmd.core.CQRS.Users;
using System.Collections.Generic;
using System.Linq;

namespace healthspanmd.web.Models.Account
{
    public static class UserViewModel_Mapping
    {
        public static UserViewModel ToUserViewModel(this UserModel u)
        {
            return new UserViewModel
            {
                UserId = u.UserId,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneCountryCode = u.PhoneCountryCode,
                Phone = u.Phone,
                IsActive = u.IsActive,
                NotificationBySMS = u.NotificationBySMS,
                NotificationByEmail = u.NotificationByEmail,
                ProgramStartDate = u.ProgramStartDate,
                ProgramEndDate = u.ProgramEndDate,
                TimeZoneId = u.TimeZoneId,
                NotificationTime = u.NotificationTime,
                SelectedMetrics = u.ActiveMetrics.Select(m => new SelectedMetricViewModel
                {
                    MetricId = m.MetricId,
                    Goal = m.Goal,
                    Goal2 = m.Goal2,
                    Frequency = (int)m.Frequency,
                    DayOfWeek = (int)m.DayOfWeek,
                }).ToList(),
            };
        }

        public static UserModel ToUserModel(this UserViewModel u, ICollection<ActiveMetricModel> existingActiveMetrics)
        {
            var model = new UserModel
            {
                UserId = u.UserId,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneCountryCode = u.PhoneCountryCode,
                Phone = u.Phone,
                IsActive = u.IsActive,
                NotificationBySMS=u.NotificationBySMS,
                NotificationByEmail=u.NotificationByEmail,
                ProgramStartDate = u.ProgramStartDate,
                ProgramEndDate = u.ProgramEndDate,
                TimeZoneId = u.TimeZoneId,
                NotificationTime = u.NotificationTime
            };

            // if new user is being created, give them Client role
            if (u.UserId == 0)
                model.AuthorizedRoles = new List<AuthorizedRoleModel> { new AuthorizedRoleModel { UserRoleId = 1 } };
            
                

            model.ActiveMetrics = new List<ActiveMetricModel>();
            foreach (var thisMetric in u.SelectedMetrics)
            {
                var existingMetric = existingActiveMetrics.Where(m => m.MetricId == thisMetric.MetricId).FirstOrDefault();
                if (existingMetric != null)
                {
                    existingMetric.Frequency = (MetricFrequencyType)thisMetric.Frequency;
                    existingMetric.DayOfWeek = (System.DayOfWeek)thisMetric.DayOfWeek;
                    existingMetric.Goal = thisMetric.Goal;
                    existingMetric.Goal2 = thisMetric.Goal2;
                    model.ActiveMetrics.Add(existingMetric);
                }
                else
                {
                    // create new Active Metric
                    model.ActiveMetrics.Add(new ActiveMetricModel
                    {
                        ActiveMetricId = 0,
                        MetricId = thisMetric.MetricId,
                        UserId = u.UserId,
                        Frequency = (MetricFrequencyType)thisMetric.Frequency,
                        Goal = thisMetric.Goal,
                        Goal2 = thisMetric.Goal2,
                        DayOfWeek = (System.DayOfWeek)thisMetric.DayOfWeek
                    });
                }    
            }

            return model;
        }
    }
}

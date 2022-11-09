using healthspanmd.core.CQRS.ActiveMetrics;
using healthspanmd.core.CQRS.Users;
using healthspanmd.core.Services.ExceptionReporting;
using healthspanmd.shared.Extensions;
using infrastructure.sqlserver.Data;
using infrastructure.sqlserver.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.CQRS.Users
{
    public class UserQueries : IUserQueries
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IExceptionReporter _exceptionReporter;
        public UserQueries(
            IServiceScopeFactory scopeFactory,
            IExceptionReporter exceptionReporter
            )
        {
            _scopeFactory = scopeFactory;
            _exceptionReporter = exceptionReporter;
        }

        public UserModel GetUserDetailModel(string email, bool includeMetricTrackingEntries)
        {
            if (string.IsNullOrEmpty(email))
                return null;

            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
               
                var user = dbContext.Users
                    .Where(u => u.Email == email);

                return GetDetailModel(dbContext, user, includeMetricTrackingEntries);
            }
        }

        public UserModel GetUserDetailModel(long userId, bool includeMetricTrackingEntries)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();

                var user = dbContext.Users
                    .Where(u => u.UserId == userId);

                return GetDetailModel(dbContext, user, includeMetricTrackingEntries);
            }
        }

        private UserModel GetDetailModel(HealthSpanMdDbContext dbContext, IQueryable<Data.Entities.User> userQuery, bool includeMetricTrackingEntries)
        {
            userQuery = userQuery
                .Include(u => u.ActiveMetrics).ThenInclude(am => am.Metric).ThenInclude(m => m.SelectItems)
                .Include(u => u.AuthorizedRoles).ThenInclude(ar => ar.UserRole)
                .Include(u => u.Notifications)
                .Include(u => u.ContentCardAssignments).ThenInclude(cca => cca.ContentCard);
            

            //if (includeMetricTrackingEntries)
            //    userQuery = userQuery.Include(u => u.MetricTrackingEntries).ThenInclude(mte => mte.EntryValues);  

            var user = userQuery.FirstOrDefault();

            if (includeMetricTrackingEntries)
            {
                user.MetricTrackingEntries = dbContext.MetricTrackingEntries.Where(e => e.UserId == user.UserId).Include(m => m.EntryValues).ToList();
            }



            return user.ToUserModel();
        }

        public ICollection<UserModel> GetUserList(GetUserListQueryFilter filter)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
                var users = dbContext.Users.AsQueryable();
                    

                if (!string.IsNullOrEmpty(filter.Phone))
                    users = users.Where(u => u.Phone == filter.Phone);

                if (filter.IsInProgramWindow.HasValue)
                {
                    if (filter.IsInProgramWindow.Value)
                        users = users.Where(u => u.ProgramStartDate <= DateTime.Today && u.ProgramEndDate >= DateTime.Today);
                    else
                        users = users.Where(u => u.ProgramStartDate > DateTime.Today || u.ProgramEndDate < DateTime.Today);
                }
                    
                if (filter.IsActive.HasValue)
                    users = users.Where(u => u.IsActive == filter.IsActive.Value);


                users = users
                    .Include(u => u.ActiveMetrics)
                    .Include(u => u.AuthorizedRoles)
                        .ThenInclude(ar => ar.UserRole)
                    .Include(u => u.Notifications);

                var result = users.Select(u => u.ToUserModel()).ToList();

                return result;
            }
        }
    }
}

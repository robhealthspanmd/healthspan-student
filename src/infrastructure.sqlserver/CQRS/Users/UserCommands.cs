using healthspanmd.core.CQRS.UserRoles;
using healthspanmd.core.CQRS.Users;
using healthspanmd.core.Services.ExceptionReporting;
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
    public class UserCommands : IUserCommands
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IUserQueries _userQueries;
        private readonly IExceptionReporter _exceptionReporter;
        public UserCommands(
            IServiceScopeFactory scopeFactory,
            IUserQueries userQueries,
            IExceptionReporter exceptionReporter
            )
        {
            _scopeFactory = scopeFactory;
            _userQueries = userQueries;
            _exceptionReporter = exceptionReporter;
        }


        public void MarkContentCardAssignmentAsComplete(int assignmentId)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
                var assignment = dbContext.ContentCardsAssignments.Find(assignmentId);
                assignment.CompletedUtc = DateTime.UtcNow;
                dbContext.ContentCardsAssignments.Update(assignment);
                dbContext.SaveChanges();
            }
        }

        public void MarkContentCardAssignmentAsComplete(long userId, int contentCardId)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
                var query = from a in dbContext.ContentCardsAssignments
                            where a.ContentCardId == contentCardId && a.UserId == userId
                            select a;
                var assignments = query.ToList();
                foreach (var assignment in assignments)
                {
                    MarkContentCardAssignmentAsComplete(assignment.ContentCardAssignmentId);
                }
            }
        }

        public UpdateUserResponse CreateContentCardAssignment(UserModel model, int contentCardId, int assignedByUserId)
        {
            var response = new UpdateUserResponse();
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
                    var contentCardAssignment = new ContentCardAssignment
                    {
                        ContentCardId = contentCardId,
                        UserId = model.UserId,
                        AssignedBy = assignedByUserId,
                        CreatedUtc = DateTime.UtcNow,
                        SortOrder = model.ContentCardAssignments.Max(a => a.SortOrder) + 1
                    };
                    dbContext.ContentCardsAssignments.Add(contentCardAssignment);
                    dbContext.SaveChanges();

                    response.User = _userQueries.GetUserDetailModel(model.UserId, false);
                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Encountered Exception: " + ex.Message;
            }
            return response;
        }

        public UpdateUserResponse RemoveContentCardAssignment(long userId, int contentCardAssignmentId)
        {
            var response = new UpdateUserResponse();
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
                    var assignmentToRemove = dbContext.ContentCardsAssignments.Find(contentCardAssignmentId);
                    dbContext.ContentCardsAssignments.Remove(assignmentToRemove);
                    dbContext.SaveChanges();

                    response.User = _userQueries.GetUserDetailModel(userId, false);
                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Encountered Exception: " + ex.Message;
            }
            return response;
        }

        public UpdateUserResponse UpdateContentCardAssignments(UserModel model)
        {
            var response = new UpdateUserResponse();
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();

                    var user = dbContext.Users
                        .Include(u => u.ContentCardAssignments)
                        .Where(u => u.UserId == model.UserId)
                        .FirstOrDefault();

                    if (user.ContentCardAssignments == null)
                        user.ContentCardAssignments = new List<ContentCardAssignment>();

                    var assignmentIdsToDelete = new List<int>();
                    foreach (var existingAssignment in user.ContentCardAssignments)
                    {
                        var assignment = model.ContentCardAssignments.Where(a => a.ContentCardAssignmentId == existingAssignment.ContentCardAssignmentId).FirstOrDefault();
                        if (assignment == null)
                        {
                            // this assignment no longer exists
                            assignmentIdsToDelete.Add(existingAssignment.ContentCardAssignmentId);
                        }
                        else
                        {
                            // assignment still exists, update the sort order
                            existingAssignment.SortOrder = assignment.SortOrder;
                        }
                    }
                    foreach (var assignmentId in assignmentIdsToDelete)
                    {
                        user.ContentCardAssignments.Remove(user.ContentCardAssignments.Where(a => a.ContentCardAssignmentId == assignmentId).FirstOrDefault());
                    }

                    foreach (var assignment in model.ContentCardAssignments.Where(a => a.ContentCardAssignmentId == 0))
                    {
                        // this is a new assignment
                        user.ContentCardAssignments.Add(new ContentCardAssignment
                        {
                            ContentCardId = assignment.ContentCardId,
                            AssignedBy = assignment.AssignedBy,
                            UserId = assignment.UserId,
                            CreatedUtc = assignment.CreatedUtc,
                            SortOrder = assignment.SortOrder,
                        });
                    }
                    
                    
                    dbContext.Users.Update(user);
                    dbContext.SaveChanges();

                    response.User = _userQueries.GetUserDetailModel(model.UserId, false);
                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Encountered Exception: " + ex.Message;
            }
            return response;
        }

        public UpdateUserResponse CreateOrUpdateUser(UserModel model, UserValidator validator)
        {
            var response = new UpdateUserResponse();

            validator.Validate(model, _userQueries);
            if (!validator.IsValid)
            {
                response.Success = false;
                response.FieldMessages = validator.FieldMessages;
                response.Message = "One or more fields failed validation";
                return response;
            }

            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();

                    var user = model.ToUser(false);

                    if (user.UserId == 0)
                    {
                        user = model.ToUser(true);
                        response.CreatedNewUser = true;
                        response.Message = "New User Created";
                        dbContext.Users.Add(user);
                    }
                    else
                    {
                        response.CreatedNewUser = false;
                        response.Message = "Existing User Updated";
                        dbContext.Users.Update(user);
                    }

                    dbContext.SaveChanges();

                    // update Active Metrics relationships
                    dbContext.Database.ExecuteSqlRaw($"delete from dbo.ActiveMetrics where UserId = {user.UserId}");
                    foreach (var selectedMetric in model.ActiveMetrics)
                    {
                        var newActiveMetric = new ActiveMetric
                        {
                            UserId = user.UserId,
                            MetricId = selectedMetric.MetricId,
                            Frequency = selectedMetric.Frequency,
                            Goal = selectedMetric.Goal,
                            Goal2 = selectedMetric.Goal2,
                            DayOfWeek = selectedMetric.DayOfWeek
                        };
                        dbContext.ActiveMetrics.Add(newActiveMetric);
                    }
                    dbContext.SaveChanges();

                    response.User = _userQueries.GetUserDetailModel(user.UserId, false);
                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Encountered Exception: " + ex.Message;
            }

            return response;
        }

        public UpdateUserResponse AddAuthorizedRole(long userId, int userRoleId)
        {
            var response = new UpdateUserResponse();
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();

                    // get the UserRole
                    var userRole = dbContext.UserRoles.Find(userRoleId);
                    if (userRole == null)
                        throw new Exception("Unable to find UserRoleId " + userRoleId);

                    // get the User
                    var user = _userQueries.GetUserDetailModel(userId, false);
                    if (user == null)
                        throw new Exception("Unable to find User with UserId " + userId);

                    // check for existing role
                    if (user.AuthorizedRoles.Any(ar => ar.UserRoleId == userRoleId))
                    {
                        response.Message = "AuthorizedRole already exists for user.";
                        response.Success = false;
                        response.User = user;
                    }
                    else
                    {
                        var authRole = new AuthorizedRole
                        {
                            UserId = userId,
                            UserRoleId = userRoleId
                        };
                        dbContext.AuthorizedRoles.Add(authRole);
                        dbContext.SaveChanges();
                        user = _userQueries.GetUserDetailModel(userId, false);
                        response.Success = true;
                        response.Message = $"Added UserRole({userRole.Name}) to user({user.UserId})";
                        response.User = user;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Encountered Exception: " + ex.Message;
            }

            return response;
        }


        public UpdateUserResponse AddActiveMetric(long userId, int metricId)
        {
            var response = new UpdateUserResponse();
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();

                    // get the Metric
                    var metric = dbContext.Metrics.Find(metricId);
                    if (metric == null)
                        throw new Exception("Unable to find Metric with MetricId " + metricId);

                    // get the User
                    var user = _userQueries.GetUserDetailModel(userId, false);
                    if (user == null)
                        throw new Exception("Unable to find User with UserId " + userId);

                    // check for existing metric
                    if (user.ActiveMetrics.Any(am => am.MetricId == metricId))
                    {
                        response.Message = $"ActiveMetric with MetricId {userId} already exists for user.";
                        response.Success = false;
                        response.User = user;
                    }
                    else
                    {
                        var activeMetric = new ActiveMetric
                        {
                            UserId = userId,
                            MetricId = metricId
                        };
                        dbContext.ActiveMetrics.Add(activeMetric);
                        dbContext.SaveChanges();
                        user = _userQueries.GetUserDetailModel(userId, false);
                        response.Success = true;
                        response.Message = $"Added Metric({metric.Name}) to user({user.UserId})";
                        response.User = user;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Encountered Exception: " + ex.Message;
                _exceptionReporter.Execute(ex, this.GetType());
            }

            return response;
        }

        public UpdateUserResponse RemoveActiveMetric(long userId, int metricId)
        {
            var response = new UpdateUserResponse();
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();

                    // get the Metric
                    var metric = dbContext.Metrics.Find(metricId);
                    if (metric == null)
                        throw new Exception("Unable to find Metric with MetricId " + metricId);

                    // get the User
                    var user = _userQueries.GetUserDetailModel(userId, false);
                    if (user == null)
                        throw new Exception("Unable to find User with UserId " + userId);

                    // check for existing metric
                    if (!user.ActiveMetrics.Any(am => am.MetricId == metricId))
                    {
                        response.Message = $"ActiveMetric with MetricId {userId} does not exists for user.";
                        response.Success = false;
                        response.User = user;
                    }
                    else
                    {
                        // remove the metric
                        var activeMetric = dbContext.ActiveMetrics.Find(metricId);
                        dbContext.ActiveMetrics.Remove(activeMetric);
                        dbContext.SaveChanges();

                        user = _userQueries.GetUserDetailModel(userId, false);
                        response.Success = true;
                        response.Message = $"Removed Metric({metric.Name}) from user({user.UserId})";
                        response.User = user;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Encountered Exception: " + ex.Message;
                _exceptionReporter.Execute(ex, this.GetType());
            }

            return response;
        }

        public UpdateUserResponse DeactivateUser(long userId)
        {
            var response = new UpdateUserResponse();
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();

                    var user = dbContext.Users.Find(userId);
                    if (user == null)
                        throw new Exception($"User with UserId {userId} was not found");

                    user.IsActive = false;
                    dbContext.Users.Update(user);
                    dbContext.SaveChanges();

                    response.Success = true;
                    response.User = user.ToUserModel();
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Encountered Exception: " + ex.Message;
                _exceptionReporter.Execute(ex, this.GetType());
            }

            return response;
        }

        public UpdateUserResponse DeleteUser(long userId)
        {
            var response = new UpdateUserResponse();
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();

                    var user = dbContext.Users.Find(userId);
                    if (user == null)
                        throw new Exception($"User with UserId {userId} was not found");


                    dbContext.Users.Remove(user);
                    dbContext.SaveChanges();

                    response.Success = true;
                    response.User = user.ToUserModel();
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Encountered Exception: " + ex.Message;
                _exceptionReporter.Execute(ex, this.GetType());
            }

            return response;
        }

        public UpdateUserResponse ActivateUser(long userId)
        {
            var response = new UpdateUserResponse();
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();

                    var user = dbContext.Users.Find(userId);
                    if (user == null)
                        throw new Exception($"User with UserId {userId} was not found");

                    user.IsActive = true;
                    dbContext.Users.Update(user); 
                    dbContext.SaveChanges();

                    response.Success = true;
                    response.User = user.ToUserModel();
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Encountered Exception: " + ex.Message;
                _exceptionReporter.Execute(ex, this.GetType());
            }

            return response;
        }

        public void UpdatePersonalizedContentNotificationMessage(int assignmentId, string notificationMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
                var assignment = dbContext.ContentCardsAssignments.Find(assignmentId);
                assignment.NotificationMessage = notificationMessage;
                dbContext.ContentCardsAssignments.Update(assignment);
                dbContext.SaveChanges();              
            }
        }

        public void MarkContentCardAssignmentNotification(int assignmentId, long notificationId)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
                var assignment = dbContext.ContentCardsAssignments.Find(assignmentId);
                assignment.NotificationId = notificationId;
                dbContext.ContentCardsAssignments.Update(assignment);
                dbContext.SaveChanges();
            }
        }
    }
}

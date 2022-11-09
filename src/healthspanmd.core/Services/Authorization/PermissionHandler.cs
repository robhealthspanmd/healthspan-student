using healthspanmd.core.CQRS.Users;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.Services.Authorization
{
    public class PermissionHandler : IAuthorizationHandler
    {
        private readonly IUserQueries _userQueries;
        private readonly IRequestParser _requestParser;

        public PermissionHandler(
            IUserQueries userQueries,
            IRequestParser requestParser
            )
        {
            _userQueries = userQueries;
            _requestParser = requestParser;
        }

        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            var pendingRequirements = context.PendingRequirements.ToList();

            foreach (var requirement in pendingRequirements)
            {

                if (requirement is CanAccessClientListRequirement)
                    if (UserCanAccessClientList(context))
                        context.Succeed(requirement);

                if (requirement is CanEditMetricsRequirement)
                    if (UserCanEditMetrics(context))
                        context.Succeed(requirement);

                if (requirement is CanViewCoachDashboardRequirement)
                    if (UserCanViewCoachDashboard(context))
                        context.Succeed(requirement);

                if (requirement is CanViewUserProgressRequirement)
                    if (UserCanViewUserProgress(context))
                        context.Succeed(requirement);

           
            }

            return Task.CompletedTask;
        }

        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //      Can Access Client List
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private bool UserCanAccessClientList(AuthorizationHandlerContext context)
        {
            if (!context.User.Identity.IsAuthenticated)
                return false;

            var user = _userQueries.GetUserDetailModel(context.User.Identity.Name, false);
            if (user.AuthorizedRoles.Any(a => a.UserRole.Name == "Admin" || a.UserRole.Name == "Coach"))
                return true;

            return false;
        }


        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //      Can Edit Metrics
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private bool UserCanEditMetrics(AuthorizationHandlerContext context)
        {
            if (!context.User.Identity.IsAuthenticated)
                return false;

            var user = _userQueries.GetUserDetailModel(context.User.Identity.Name, false);
            if (user.AuthorizedRoles.Any(a => a.UserRole.Name == "Admin" || a.UserRole.Name == "Coach"))
                return true;

            return false;
        }

        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //      Can View Coach Dashboard
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private bool UserCanViewCoachDashboard(AuthorizationHandlerContext context)
        {
            if (!context.User.Identity.IsAuthenticated)
                return false;

            var user = _userQueries.GetUserDetailModel(context.User.Identity.Name, false);
            if (user.AuthorizedRoles.Any(a => a.UserRole.Name == "Admin" || a.UserRole.Name == "Coach"))
                return true;

            return false;
        }


        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //      Can View User Progress
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private bool UserCanViewUserProgress(AuthorizationHandlerContext context)
        {
            if (!context.User.Identity.IsAuthenticated)
                return false;

            var user = _userQueries.GetUserDetailModel(context.User.Identity.Name, false);
            if (user.AuthorizedRoles.Any(a => a.UserRole.Name == "Admin" || a.UserRole.Name == "Coach"))
                return true;

            // if this is a Client, they can view if it is theirs
            var progressForUserId = _requestParser.GetUserId();
            if (progressForUserId.HasValue)
            {
                return user.UserId == progressForUserId.Value;
            }
            else
            {
                // this is just request to /Analytics, so it will only show the users' progress, so this can be authorized
                return true;
            }

        }

    }
}

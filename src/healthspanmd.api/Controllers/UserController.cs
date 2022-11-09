using healthspanmd.api.Attributes;
using healthspanmd.core.CQRS.MetricTrackingEntries;
using healthspanmd.core.CQRS.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace healthspanmd.api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiKey]
    public class UserController : Controller
    {
        private readonly IUserCommands _userCommands;
        private readonly IUserQueries _userQueries;
        private readonly IMetricTrackingEntryCommands _metricTrackingEntryCommands;

        public UserController(
            IUserCommands userCommands,
            IUserQueries userQueries,
            IMetricTrackingEntryCommands metricTrackingEntryCommands
            )
        {
            _userCommands = userCommands;
            _userQueries = userQueries;
            _metricTrackingEntryCommands = metricTrackingEntryCommands;
        }

        [HttpGet("{userId}")]
        public IActionResult GetUserDetail(long userId)
        {
            var userModel = _userQueries.GetUserDetailModel(userId, true);
            return new JsonResult(userModel);
        }


        [HttpPost("")]
        public IActionResult CreateOrUpdateUser([FromBody] UserModel model)
        {
            var response = _userCommands.CreateOrUpdateUser(model, new UserValidator());
            return new JsonResult(response);
        }

        [HttpPost("{userId}/ActiveMetric/{userRoleId}")]
        public IActionResult AddUserRole(long userId, int userRoleId)
        {
            var response = _userCommands.AddAuthorizedRole(userId, userRoleId);
            return new JsonResult(response);
        }

        [HttpPost("{userId}/ActiveMetric/{metricId}")]
        public IActionResult AddActiveMetric(long userId, int metricId)
        {
            var response = _userCommands.AddActiveMetric(userId, metricId);
            return new JsonResult(response);
        }

        [HttpDelete("{userId}/ActiveMetric/{metricId}")]
        public IActionResult RemoveActiveMetric(long userId, int metricId)
        {
            var response = _userCommands.RemoveActiveMetric(userId, metricId);
            return new JsonResult(response);
        }


        [HttpPost("{userId}/MetricTrackingEntry")]
        public IActionResult AddMetricTrackingEntries([FromBody] AddMetricTrackingEntryRequest requestModel)
        {
            var response = _metricTrackingEntryCommands.CreateOrReplaceEntries(requestModel);
            return new JsonResult(response);
        }

        [HttpGet("{userId}/MetricTrackingEntry")]
        public IActionResult GetMetricTrackingEntries(int userId)
        {
            var response = _userQueries.GetUserDetailModel(userId, true);
            return new JsonResult(response);
        }

    }
}

using healthspanmd.core.CQRS.ActiveMetrics;
using healthspanmd.core.CQRS.Content;
using healthspanmd.core.CQRS.Metrics;
using healthspanmd.core.CQRS.Notifications;
using healthspanmd.core.CQRS.Users;
using healthspanmd.core.Services.Messaging;
using healthspanmd.core.Services.SMS;
using healthspanmd.shared.Extensions;
using healthspanmd.web.Helpers;
using healthspanmd.web.Models;
using healthspanmd.web.Models.Account;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace healthspanmd.web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUserQueries _userQueries;
        private readonly IUserCommands _userCommands;
        private readonly IMetricQueries _metricQueries;
        private readonly IEmailSender _emailSender;
        private readonly ISMSProvider _smsProvider;
        private readonly IConfiguration _config;
        private readonly INotificationQueries _notificationQueries;
        private readonly IContentQueries _contentQueries;
        private readonly INotificationSender _notificationSender;

        public AccountController(
            IUserQueries userQueries,
            IUserCommands userCommands,
            IMetricQueries metricQueries,
            IEmailSender emailSender,
            ISMSProvider smsProvider,
            IConfiguration config,
            INotificationQueries notificationQueries,
            IContentQueries contentQueries,
            INotificationSender notificationSender
            )
        {
            _userQueries = userQueries;
            _userCommands = userCommands;
            _metricQueries = metricQueries;
            _emailSender = emailSender;
            _smsProvider = smsProvider;
            _config = config;
            _notificationQueries = notificationQueries;
            _contentQueries = contentQueries;
            _notificationSender = notificationSender;
        }

        public IActionResult Index()
        {
            return View();
        }


        



        public IActionResult Settings()
        {
            this.AddBreadCrumb("Account");
            this.AddBreadCrumb("Settings");


            var user = _userQueries.GetUserDetailModel(HttpContext.User.Identity.Name, false);
            var model = new AccountSettingsViewModel
            {
                UserRole = user.AuthorizedRoleList,
                UserBasicInfo = new AccountSettingsViewModel.BasicInfo
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phone = user.Phone
                }          
            };

            return View(model);
        }

        [HttpPost]
        [Route("/Account/UpdateBasicInfo")]
        public async Task<IActionResult> UpdateBasicInfoAsync(AccountSettingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userQueries.GetUserDetailModel(HttpContext.User.Identity.Name, false);

                user.FirstName = model.UserBasicInfo.FirstName;
                user.LastName = model.UserBasicInfo.LastName;
                user.Phone = model.UserBasicInfo.Phone;

                _userCommands.CreateOrUpdateUser(user, new UserValidator());

                var html = await this.RenderViewAsync("/Views/Account/_BasicInfoForm.cshtml", model, true);
                return Json(new { success = true, html });
            }
            else
            {
                var html = await this.RenderViewAsync("/Views/Account/_BasicInfoForm.cshtml", model, true);
                return Json(new { success = false, html });
            }


        }


        [Authorize(Policy = "CanAccessClientList")]
        [HttpGet]
        [Route("/Account/ClientList")]
        public IActionResult ClientList()
        {
            this.AddBreadCrumb("Clients");

            //var users = _userQueries.GetUserList(new GetUserListQueryFilter());
            //var model = new ClientListViewModel
            //{
            //    ClientList = users.Select(u => u.ToClientListItemViewModel()).ToList()
            //};
            //return View(model);
            return View();
        }


        [Authorize(Policy = "CanAccessClientList")]
        [Route("/Account/ClientList_Read")]
        public IActionResult ClientList_Read([DataSourceRequest] DataSourceRequest request)
        {
            var users = _userQueries.GetUserList(new GetUserListQueryFilter());
            var result = users
                .Where(u => u.IsActive)
                .Select(u => u.ToClientListItemViewModel())
                .OrderBy(u => u.LastName_comma_FirstName)
                .AsEnumerable();
            return Json(result.ToDataSourceResult(request));
        }

        [Authorize(Policy = "CanAccessClientList")]
        [HttpGet]
        [Route("/Account/ClientListInactive")]
        public IActionResult ClientListInactive()
        {
            this.AddBreadCrumb("Clients");
            return View();
        }

        [Authorize(Policy = "CanAccessClientList")]
        [Route("/Account/ClientListInactive_Read")]
        public IActionResult ClientListInactive_Read([DataSourceRequest] DataSourceRequest request)
        {
            var users = _userQueries.GetUserList(new GetUserListQueryFilter());
            var result = users
                .Where(u => !u.IsActive)
                .Select(u => u.ToClientListItemViewModel())
                .OrderBy(u => u.LastName_comma_FirstName)
                .AsEnumerable();
            return Json(result.ToDataSourceResult(request));
        }

        [Authorize(Policy = "CanAccessClientList")]
        [HttpGet]
        [Route("/Account/AdminDetail/{userId}")]
        public IActionResult AdminDetail(long userId)
        {
            this.AddBreadCrumb("Clients","/Account/ClientList");
            

            UserViewModel userViewModel = null;
            if (userId == 0)
            {
                userViewModel = new UserViewModel
                {
                    SelectedMetrics = new List<SelectedMetricViewModel>()
                };
                this.AddBreadCrumb("Add New Client");
            }
            else
            {
                var userModel = _userQueries.GetUserDetailModel(userId, false);
                userViewModel = userModel.ToUserViewModel();
                this.AddBreadCrumb(userModel.FullName);
            }

            var model = new ClientDetailViewModel
            {
                Client = userViewModel,
                Metrics = _metricQueries.GetMetrics(new GetMetricsQueryFilter()),
            };

            model.MetricsJson = JsonSerializer.Serialize(model.Metrics);
            model.SelectedMetricsJson = JsonSerializer.Serialize(model.Client.SelectedMetrics);

            return View(model);
        }


        [Authorize(Policy = "CanAccessClientList")]
        [HttpPost]
        [Route("/Account/SendTestNotification")]
        public IActionResult SendTestNotification([FromBody] SendRequest sendRequest)
        {
            var response = _smsProvider.Send(sendRequest);
            if (response.Success)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }


        [Authorize(Policy = "CanAccessClientList")]
        [HttpPost]
        [Route("/Account/AdminDetail")]
        public async Task<IActionResult> AdminDetailAsync([FromBody] UserViewModel model)
        {
            var activeMetrics = new List<ActiveMetricModel>();
            if (model.UserId > 0)
            {
                var user = _userQueries.GetUserDetailModel(model.UserId, false);
                activeMetrics = user.ActiveMetrics.ToList();
            }
            
            var result = _userCommands.CreateOrUpdateUser(model.ToUserModel(activeMetrics), new UserValidator());
            if (result.Success)
            {
                if (result.CreatedNewUser)
                {
                    // send notification messages
                    // TODO: Need to tokenize this Url for phones
                    var registerUrl = $"{_config.GetSection("OpenIdConnectSettings:RegisterUrl").Value}?firstname={result.User.FirstName}&lastname={result.User.LastName}&email={result.User.Email}";

                    var notificationTemplate = _notificationQueries.GetNotificationTemplate(NotificationType.InvitationToRegister);
                    var msg = notificationTemplate.Template
                        .Replace("{firstname}", result.User.FirstName)
                        .Replace("{registrationlink}", registerUrl);

                   
                    _smsProvider.Send(new SendRequest
                    {
                        PhoneNumber = result.User.PhoneNumberAs10DigitLongCode,
                        Message = msg
                    });

                    var emailTask = _emailSender.SendEmailAsync(result.User.Email, "Welcome to HealthspanMD!", msg);
                    
                }
                return Json(new { success = true });
            }
            else
            {

                return Json(new { success = false, message = result.Message, fieldMessages = result.FieldMessages });
            }
        }

        [Authorize(Policy = "CanAccessClientList")]
        [HttpGet]
        [Route("/Account/Deactivate/{userId}")]
        public async Task<IActionResult> DeactivateAsync(long userId)
        {
            var result = _userCommands.DeactivateUser(userId);
            bool success = result.Success;
            string html = "";

            if (result.Success)
            {
                // get fresh list of metrics and return partial view
                var users = _userQueries.GetUserList(new GetUserListQueryFilter());
                var model = new ClientListViewModel
                {
                    ClientList = users.Select(u => u.ToClientListItemViewModel()).ToList()
                };
                html = await this.RenderViewAsync("/Views/Account/_ClientListData.cshtml", model, true);
            }

            return Json(new { success, html });
        }

        [Authorize(Policy = "CanAccessClientList")]
        [HttpGet]
        [Route("/Account/Delete/{userId}")]
        public async Task<IActionResult> DeleteAsync(long userId)
        {
            var result = _userCommands.DeleteUser(userId);
            bool success = result.Success;
            string html = "";

            if (result.Success)
            {
                // get fresh list of metrics and return partial view
                var users = _userQueries.GetUserList(new GetUserListQueryFilter());
                var model = new ClientListViewModel
                {
                    ClientList = users.Select(u => u.ToClientListItemViewModel()).ToList()
                };
                html = await this.RenderViewAsync("/Views/Account/_ClientListData.cshtml", model, true);
            }

            return Json(new { success, html });
        }

        [Authorize(Policy = "CanAccessClientList")]
        [HttpGet]
        [Route("/Account/Activate/{userId}")]
        public async Task<IActionResult> ActivateAsync(long userId)
        {
            var result = _userCommands.ActivateUser(userId);
            bool success = result.Success;
            string html = "";

            if (result.Success)
            {
                // get fresh list of metrics and return partial view
                var users = _userQueries.GetUserList(new GetUserListQueryFilter());
                var model = new ClientListViewModel
                {
                    ClientList = users.Select(u => u.ToClientListItemViewModel()).ToList()
                };
                html = await this.RenderViewAsync("/Views/Account/_ClientListData.cshtml", model, true);
            }

            return Json(new { success, html });
        }


        [Authorize(Policy = "CanAccessClientList")]
        [HttpGet]
        [Route("/Account/ContentAssignments/{userId}")]
        public IActionResult ContentAssignments(long userId)
        {
            

            this.AddBreadCrumb("Clients", "/Account/ClientList");
            this.AddBreadCrumb("Content Assignments");

            var model = new ContentAssignmentsViewModel
            {
                TargetUser = _userQueries.GetUserDetailModel(userId, false),
                ContentCardLibrary = _contentQueries.GetList(new GetContentCardListQueryFilter
                {
                    ActiveOnly = true,
                })
            };

            if (model.TargetUser.ContentCardAssignments == null)
                model.TargetUser.ContentCardAssignments = new List<ContentCardAssignmentModel>();

            return View(model);
        }

        [Authorize(Policy = "CanAccessClientList")]
        [HttpGet]
        [Route("/Account/GetPersonalizedMessageDialog/{userId}/{contentCardId}")]
        public async Task<IActionResult> GetPersonalizedMessageDialogAsync(long userId, int contentCardId)
        {
            var targetUser = _userQueries.GetUserDetailModel(userId, false);
            var model = new PersonalizedContentNotificationDialogViewModel
            {
                TargetUser = targetUser,
                ContentCard = _contentQueries.GetContentCard(contentCardId),
                ContentCardAssignment = targetUser.ContentCardAssignments
                    .Where(a => a.ContentCardId == contentCardId)
                    .FirstOrDefault()
            };
            if (model.ContentCardAssignment.NotificationId.HasValue)
                model.Notification = _notificationQueries.GetNotification(model.ContentCardAssignment.NotificationId.Value);
            var html = await this.RenderViewAsync("/Views/Account/_PersonalizedContentNotificationDialog.cshtml", model, true);
            return Json(new { success = true, html });
        }


        [Authorize(Policy = "CanAccessClientList")]
        [HttpGet]
        [Route("/Account/SendContentNotification/{userId}/{contentCardId}")]
        public async Task<IActionResult> SendContentNotification(long userId, int contentCardId)
        {
            var targetUser = _userQueries.GetUserDetailModel(userId, false);
            var contentCard = _contentQueries.GetContentCard(contentCardId);

            _notificationSender.SendContentNotification(targetUser, contentCard);

            return Json(new { success = true });
        }


        [Authorize(Policy = "CanAccessClientList")]
        [HttpGet]
        [Route("/Account/ShowContentNotificationConfirmSend/{userId}/{contentCardId}")]
        public async Task<IActionResult> ShowContentNotificationConfirmSend(long userId, int contentCardId)
        {
            var targetUser = _userQueries.GetUserDetailModel(userId, false);
            var model = new PersonalizedContentNotificationDialogViewModel
            {
                TargetUser = targetUser,
                ContentCard = _contentQueries.GetContentCard(contentCardId),
                ContentCardAssignment = targetUser.ContentCardAssignments
                    .Where(a => a.ContentCardId == contentCardId)
                    .FirstOrDefault()
            };
            if (model.ContentCardAssignment.NotificationId.HasValue)
                model.Notification = _notificationQueries.GetNotification(model.ContentCardAssignment.NotificationId.Value);
            var html = await this.RenderViewAsync("/Views/Account/_SendContentNotificationDialog.cshtml", model, true);
            return Json(new { success = true, html });
        }


        [Authorize(Policy = "CanAccessClientList")]
        [HttpPost]
        [Route("/Account/SetPersonalizedNotificationMessage")]
        public IActionResult SetContentAssignments([FromBody] SetPersonalizedNotificationMessageViewModel model)
        {
            try
            {
                var targetUser = _userQueries.GetUserDetailModel(model.UserId, false);
                var existingAssignment = targetUser.ContentCardAssignments
                    .Where(a => a.ContentCardId == model.ContentCardId)
                    .FirstOrDefault();
                _userCommands.UpdatePersonalizedContentNotificationMessage(existingAssignment.ContentCardAssignmentId, model.NotificationMessage);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }


        [Authorize(Policy = "CanAccessClientList")]
        [HttpPost]
        [Route("/Account/SetContentAssignments")]
        public IActionResult SetContentAssignments([FromBody] SetContentAssignmentsViewModel model)
        {
            var adminUser = _userQueries.GetUserDetailModel(User.Identity.Name, false);
            var targetUser = _userQueries.GetUserDetailModel(model.UserId, false);
            var sortOrder = 0;
            foreach (var contentCardId in model.ContentCardIds)
            {
                var existingAssignment = targetUser.ContentCardAssignments
                    .Where(a => a.ContentCardId == contentCardId)
                    .FirstOrDefault();

                if (existingAssignment != null)
                    existingAssignment.SortOrder = sortOrder;
                else
                    targetUser.ContentCardAssignments.Add(new ContentCardAssignmentModel
                    {
                        ContentCardId = contentCardId,
                        CreatedUtc = DateTime.UtcNow,
                        UserId = targetUser.UserId,
                        SortOrder = sortOrder,
                        AssignedBy = adminUser.UserId
                    });
                sortOrder += 1;
            }

            // look for removed assignments
            var contentCardAssignmentIdsToRemove = new List<int>();
            foreach (var assignment in targetUser.ContentCardAssignments)
                if (!model.ContentCardIds.Contains(assignment.ContentCardId))
                    if (!assignment.CompletedUtc.HasValue)
                        contentCardAssignmentIdsToRemove.Add(assignment.ContentCardAssignmentId);
            foreach (var assignmentId in contentCardAssignmentIdsToRemove)
                targetUser.ContentCardAssignments.Remove(
                    targetUser.ContentCardAssignments
                    .Where(a => a.ContentCardAssignmentId == assignmentId)
                    .FirstOrDefault()
                    );

            // save user
            var response = _userCommands.UpdateContentCardAssignments(targetUser);

            if (response.Success)
                return Json(new { success = response.Success });
            else
                return Json(new { success = response.Success, message = response.Message });
        }


    }
}

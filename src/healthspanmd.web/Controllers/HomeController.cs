using healthspanmd.core.CQRS.Content;
using healthspanmd.core.CQRS.Users;
using healthspanmd.core.Services.FileSystem;
using healthspanmd.core.Services.Messaging;
using healthspanmd.web.Helpers;
using healthspanmd.web.Models;
using healthspanmd.web.Models.Content;
using healthspanmd.web.Models.Home;
using healthspanmd.web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace healthspanmd.web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;
        private readonly IEmailSender _emailSender;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IUserQueries _userQueries;
        private readonly IContentQueries _contentQueries;
        private readonly IFileSystemManager _fileSystemManager;
        private readonly IViewHelper _viewHelper;

        public HomeController(
            IConfiguration config,
            IEmailSender emailSender,
            ILoggerFactory loggerFactory,
            IUserQueries userQueries,
            IContentQueries contentQueries,
            IFileSystemManager fileSystemManager,
            IViewHelper viewHelper
            )
        {
           
            _config = config;
            _emailSender = emailSender;
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<HomeController>();
            _userQueries = userQueries;
            _contentQueries = contentQueries;
            _fileSystemManager = fileSystemManager;
            _viewHelper = viewHelper;
        }

        [AllowAnonymous]
        [Route("/")]
        [Route("/Home")]
        [Route("/Home/Index")]
        [Route("/Index")]
        public IActionResult Index()
        {
            var model = new DashboardViewModel();

            if (User.Identity.IsAuthenticated)
            {
                model = GetDashboardViewModel();
                return View("Welcome", model);
            }

            
            
            return View(model);
        }

        private DashboardViewModel GetDashboardViewModel()
        {
            var model = new DashboardViewModel
            {
                User = _userQueries.GetUserDetailModel(User.Identity.Name, true),
                ContentCardPreviews = new List<ContentCardPreviewViewModel>()
            };

            foreach (var contentCardAssignment in model.User.ContentCardAssignments.Where(a => !a.CompletedUtc.HasValue).OrderBy(a => a.SortOrder))
                model.ContentCardPreviews.Add(_viewHelper.GetContentCardPreviewViewModel(contentCardAssignment.ContentCard));
            

            return model;
        }

        public IActionResult WeeklyReflection()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult TestEmail()
        {
            _logger.LogInformation("Trying TestEmail");
            try
            {
                TestGmailService.SendTestMessageUsingApi(_config);
                _logger.LogInformation("Test Email Sent");
                ViewBag.Info = "Test Email Succeeded";
            }
            catch (Exception ex)
            {
                ViewBag.Info = ex.ToString();
                _logger.LogError(ex.ToString());
            }
            

            return View();
        }

        public IActionResult Progress()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        [Route("Home/Register")]
        public IActionResult Register(string url)
        {
            HttpContext.Session.SetString("returnToUrlAfterLogin", url);
            return Redirect(_config.GetSection("OpenIdConnectSettings").GetValue<string>("RegisterUrl"));
        }

        [Authorize]
        [HttpGet]
        [Route("Home/ReturnAfterRegistration")]
        public IActionResult ReturnAfterRegistration()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("returnToUrlAfterLogin")))
            {
                return Redirect(HttpContext.Session.GetString("returnToUrlAfterLogin"));
            }
            else
            {
                return Redirect("/");
            }
        }

        [Authorize]
        [HttpGet]
        [Route("/Register")]
        public IActionResult Register()
        {
            //var url = $"{_config.GetSection("SiteUrl").Value}/Home/Welcome";
            //HttpContext.Session.SetString("returnToUrlAfterLogin", url);
            ////return Redirect(_config.GetSection("OpenIdConnectSettings").GetValue<string>("RegisterUrl"));
            //return Redirect("ChallengeAuth");
            return View();
        }


        [Authorize]
        [Route("/Home/TestLogin/{email}")]
        public IActionResult TestLogin(string email)
        {
            ViewBag.Email = email;
            return View();
        }


        [Authorize]
        [HttpGet]
        [Route("Home/Welcome")]
        [Route("Home/Dashboard")]
        public IActionResult Welcome()
        {

            var model = GetDashboardViewModel();
            return View(model);
        }


        [HttpPost]
        [Route("Home/Login")]
        public IActionResult Login(string url)
        {
            HttpContext.Session.SetString("returnToUrlAfterLogin", url);
            return Redirect("ChallengeAuth");
        }

        [Authorize]
        [HttpGet]
        [Route("Home/ChallengeAuth")]
        public IActionResult ChallengeAuth()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("returnToUrlAfterLogin")))
            {
                return Redirect(HttpContext.Session.GetString("returnToUrlAfterLogin"));
            }
            else
            {
                return Redirect("/");
            }

        }

        [HttpGet]
        [Route("Home/Logout")]
        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }

       



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

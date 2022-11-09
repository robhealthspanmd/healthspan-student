using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using IEmailSender = healthspanmd.core.Services.Messaging.IEmailSender;
using healthspan.sso.web.Models;
using healthspanmd.core.CQRS.Users;
using Microsoft.Extensions.Configuration;
using healthspanmd.core.CQRS.Notifications;

namespace healthspan.sso.web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IUserQueries _userQueries;
        private readonly IUserCommands _userCommands;
        private readonly IConfiguration _appConfig;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IUserQueries userQueries,
            IUserCommands userCommands,
            IConfiguration appConfig
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _userQueries = userQueries;
            _userCommands = userCommands;
            _appConfig = appConfig;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
        

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            public string OriginalQueryString { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            Input = new InputModel { OriginalQueryString = Request.QueryString.ToString() };

            Input.FirstName = Request.Query["firstname"];
            Input.LastName = Request.Query["lastname"];
            Input.Email = Request.Query["email"];

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            // only allow authorized users to register a login
            var user = _userQueries.GetUserDetailModel(Input.Email, false);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Invalid account info");
            }


            if (ModelState.IsValid)
            {
                


                var identityUser = new ApplicationUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(identityUser, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new Identity account with password.");

                    // we will only get here if account already exists, so this is an update
                    _userCommands.CreateOrUpdateUser(new UserModel
                    {
                        FirstName = Input.FirstName,
                        LastName = Input.LastName,
                        Email = Input.Email, 
                        //IsActive = true,
                        IdentityUserId = identityUser.Id
                    }, new UserValidator());
                    _logger.LogInformation("Updated new User account.");

                    

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = identityUser.Id, code = code, returnUrl = returnUrl },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        _logger.LogInformation($"Sent confirmation email to {Input.Email}");

                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(identityUser, isPersistent: true);

                        returnUrl = $"{_appConfig.GetSection("IdentityServer:HealthspanMDWebClient:BaseUrl").Value}/Home/Welcome";
                        
                        return Redirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}

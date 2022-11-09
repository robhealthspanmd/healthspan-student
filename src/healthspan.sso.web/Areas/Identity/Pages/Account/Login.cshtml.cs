using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using healthspan.sso.web.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.Configuration;
using healthspanmd.core.CQRS.Users;
using healthspanmd.core.Services.Messaging;

namespace healthspan.sso.web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IIdentityServerInteractionService _is4service;
        private readonly IConfiguration _config;
        private readonly IEmailSender _emailSender;
        private readonly IUserQueries _userQueries;

        public LoginModel(SignInManager<ApplicationUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager,
            IIdentityServerInteractionService is4service,
            IConfiguration config,
            IEmailSender emailSender,
            IUserQueries userQueries
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _is4service = is4service;
            _config = config;
            _emailSender = emailSender;
            _userQueries = userQueries;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            var context = await _is4service.GetAuthorizationContextAsync(returnUrl);
            if (!string.IsNullOrEmpty(returnUrl))
                if (returnUrl.Contains("/Home/Welcome"))
                    return RedirectToPage("./Register", new { ReturnUrl = returnUrl });
            

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {

                    // get user record
                    var user = _userQueries.GetUserDetailModel(Input.Email,false); 

                    if (!user.IsActive)
                    {
                        await _signInManager.SignOutAsync();
                        ModelState.AddModelError("Input.Email", "User account has been deactivated.");
                        ViewData.Add("LoginError","User account has been deactivated.");
                        return Page();
                    }



                    _logger.LogInformation("User logged in.");
                    return Redirect(returnUrl);
                    
                    

                    //return LocalRedirect(returnUrl);
                    
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace healthspan.sso.web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordConfirmationModel : PageModel
    {
        private readonly IConfiguration _config;

        public ResetPasswordConfirmationModel(
            IConfiguration config
            )
        {
            _config = config;
        }

        public string ClientUrl { get; set; }
        public void OnGet()
        {
            
            ClientUrl = $"{_config.GetSection("IdentityServer:HealthspanMDWebClient:BaseUrl").Value}/Home/Welcome";

        }
    }
}

using healthspan.sso.web.Models;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace healthspan.sso.web.Services
{
    public class ProfileService : IProfileService
    {
        protected UserManager<ApplicationUser> _userManager;
        //private readonly IGetAuthorizedProfileQuery _getAuthorizedProfile;

        public ProfileService(
            UserManager<ApplicationUser> userManager
            //IGetAuthorizedProfileQuery getAuthorizedProfile
            )
        {
            _userManager = userManager;
            //_getAuthorizedProfile = getAuthorizedProfile;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            IList<Claim> claims = new List<Claim>();

            //var authProfile = _getAuthorizedProfile.Execute(context.Subject.Identity.Name);

            // lookup user's roles and claims 
            // each claim type can be handled separately 
            foreach (var claim in context.Subject.Claims)
            {
                switch (claim.Type)
                {
                    case "role":
                        claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", claim.Value));
                        break;

                    default:
                        claims.Add(new Claim(claim.Type, claim.Value));
                        break;

                }

            }



            context.IssuedClaims.AddRange(claims);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            var user = _userManager.GetUserAsync(context.Subject).Result;

            context.IsActive = (user != null) && ((!user.LockoutEnd.HasValue) || (user.LockoutEnd.Value <= DateTime.Now));

            return Task.FromResult(0);
        }
    }
}

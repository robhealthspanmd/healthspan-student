using healthspanmd.core.CQRS.Users;
using System.Collections.Generic;
using System.Linq;

namespace healthspanmd.web.Models.Account
{
    public static class ClientListItemViewModel_Mapping
    {
        public static ClientListItemViewModel ToClientListItemViewModel(this UserModel u)
        {
            var result = new ClientListItemViewModel
            {
                UserId = u.UserId,
                FirstName = u.FirstName,
                LastName = u.LastName,
                ActiveMetricCount = u.ActiveMetrics == null ? 0 : u.ActiveMetrics.Count,
                Email = u.Email,
                PhoneCountryCode = u.PhoneCountryCode,
                Phone = u.Phone,
                IsActive = u.IsActive,               
                ProgramEndDate = u.ProgramEndDate,
                ProgramStartDate = u.ProgramStartDate,
            };
            
            
            if (u.AuthorizedRoles != null)
            {
                result.AuthorizedRoles = string.Join(", ", u.AuthorizedRoles.Select(r => r.UserRole.Name).ToList());
            }

            return result;
        }
    }
}

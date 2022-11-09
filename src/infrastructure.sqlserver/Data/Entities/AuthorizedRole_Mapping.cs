using healthspanmd.core.CQRS.AuthorizedRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data.Entities
{
    public static class AuthorizedRole_Mapping
    {

        public static AuthorizedRoleModel ToAuthorizedRoleModel(this AuthorizedRole ar)
        {
            if (ar == null)
                return null;

            return new AuthorizedRoleModel
            {
                AuthorizedRoleId = ar.AuthorizedRoleId,
                UserId = ar.UserId,
                UserRoleId = ar.UserRoleId,
                UserRole = ar.UserRole.ToUserRoleModel()
            };
        }


        public static AuthorizedRole ToAuthorizedRole(this AuthorizedRoleModel m)
        {
            if (m == null)
                return null;

            return new AuthorizedRole
            {
                AuthorizedRoleId = m.AuthorizedRoleId,
                UserId = m.UserId,
                UserRoleId = m.UserRoleId,
                UserRole = m.UserRole.ToUserRole()
            };
        }
    }
}

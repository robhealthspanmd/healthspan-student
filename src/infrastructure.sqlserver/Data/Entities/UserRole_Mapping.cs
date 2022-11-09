using healthspanmd.core.CQRS.UserRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data.Entities
{
    public static class UserRole_Mapping
    {
        public static UserRoleModel ToUserRoleModel(this UserRole ur)
        {
            if (ur == null)
                return null;

            return new UserRoleModel
            {
                UserRoleId = ur.UserRoleId,
                Name = ur.Name
            };
        }

        public static UserRole ToUserRole(this UserRoleModel m)
        {
            if (m == null)
                return null;

            return new UserRole
            {
                UserRoleId = m.UserRoleId,
                Name = m.Name
            };
        }

    }
}

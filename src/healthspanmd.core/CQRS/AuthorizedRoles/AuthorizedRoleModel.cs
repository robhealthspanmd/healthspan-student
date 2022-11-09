using healthspanmd.core.CQRS.UserRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.AuthorizedRoles
{
    public class AuthorizedRoleModel
    {
        public long AuthorizedRoleId { get; set; }
        public long UserId { get; set; }
        public int UserRoleId { get; set; }
        public UserRoleModel UserRole { get; set; }
    }
}

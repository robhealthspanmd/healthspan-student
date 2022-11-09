using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data.Entities
{
    public class AuthorizedRole
    {
        public long AuthorizedRoleId { get; set; }
        public long UserId { get; set; }
        public int UserRoleId { get; set; }
        public UserRole UserRole { get; set; }
    }
}

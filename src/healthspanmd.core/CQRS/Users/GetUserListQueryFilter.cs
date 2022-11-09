using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.Users
{
    public class GetUserListQueryFilter
    {
        public string Phone { get; set; }
        public bool? IsInProgramWindow { get; set; }
        public bool? IsActive { get; set; }
    }
}

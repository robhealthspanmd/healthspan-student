using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.Users
{
    public class UpdateUserResponse
    {
        public bool CreatedNewUser { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public UserModel User { get; set; }
        public Dictionary<string, string> FieldMessages { get; set; }
    }
}

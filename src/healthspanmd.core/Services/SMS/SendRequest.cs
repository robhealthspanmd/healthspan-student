using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.Services.SMS
{
    public class SendRequest
    {
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
    }
}

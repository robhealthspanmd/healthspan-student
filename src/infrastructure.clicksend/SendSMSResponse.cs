using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.clicksend
{
    internal class SendSMSResponse
    {
        public int http_code { get; set; }
        public string response_code { get; set; }
        public string response_msg { get; set; }
        public SendSMSResponse_Data data { get; set; }
    }
}

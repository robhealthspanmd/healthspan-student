using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.clicksend
{
    public class ClickSendRequest
    {
        public string from { get; set; }
        public string body { get; set; }
        public string to { get; set; }
    }
}

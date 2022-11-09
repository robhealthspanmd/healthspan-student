using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.clicksend
{
    internal class SendSMSResponse_Data
    {
        public decimal total_price { get; set; }
        public int total_count { get; set; }
        public int queued_count { get; set; }

        public ICollection<SendSMSResponse_Data_Message> messages { get; set; }
        public SendSMSResponse_Data_Currency currency { get; set; }
    }
}

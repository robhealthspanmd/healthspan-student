using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.clicksend
{
    internal class SendSMSResponse_Data_Message
    {
        public string direction { get; set; }
        public long date { get; set; }
        public string to { get; set; }
        public string body { get; set; }
        public string from { get; set; }
        public long schedule { get; set; }
        public string message_id { get; set; }
        public int message_parts { get; set; }
        public string message_price { get; set; }
        public string from_email { get; set; }
        public string list_id { get; set; }
        public string custom_string { get; set; }
        public string contact_id { get; set; }
        public int user_id { get; set; }
        public int subaccount_id { get; set; }
        public string country { get; set; }
        public string carrier { get; set; }
        public string status { get; set; }
    }
}

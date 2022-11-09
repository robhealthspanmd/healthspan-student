using System;
using System.Collections.Generic;
using System.Text;

namespace healthspanmd.core.Services.Messaging
{ 
    public class EmailSenderOptions
    {
        public string SMTPFrom { get; set; }
        public string SMTPServer { get; set; }
        public string SMTPUsername { get; set; }
        public string SMTPPassword { get; set; }
        public int SMTPPort { get; set; }
        public bool SMTPEnableSSL { get; set; }
    }
}

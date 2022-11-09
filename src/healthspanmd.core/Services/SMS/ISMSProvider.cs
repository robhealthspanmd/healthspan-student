using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.Services.SMS
{
    public interface ISMSProvider
    {
        SendResponse Send(SendRequest request);
    }
}

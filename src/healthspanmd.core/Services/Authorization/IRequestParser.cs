using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.Services.Authorization
{
    public interface IRequestParser 
    {
        int? GetUserId();
    }
}

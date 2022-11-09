using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.Services.ExceptionReporting
{
    public class ExceptionReporter : IExceptionReporter
    {

        public void Execute(Exception ex, Type type)
        {
            // do some logging and maybe notifications here
            var message = ex.Message;
            var callStack = ex.ToString();
            var locationOfException = type.Namespace + "." + type.Name;
            
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.Services.ExceptionReporting
{
    public interface IExceptionReporter
    {
        void Execute(Exception ex, Type type);
    }
}

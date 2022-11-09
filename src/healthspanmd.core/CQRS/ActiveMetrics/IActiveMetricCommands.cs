using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.ActiveMetrics
{
    public interface IActiveMetricCommands
    {
        void Create(ActiveMetricModel model);
    }
}

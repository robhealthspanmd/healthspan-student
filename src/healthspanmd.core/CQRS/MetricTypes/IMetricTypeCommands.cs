using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.MetricTypes
{
    public interface IMetricTypeCommands
    {
        UpdateMetricTypeResponse Create(MetricTypeModel model);
    }
}

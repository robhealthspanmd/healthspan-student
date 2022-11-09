using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.Metrics
{
    public enum MetricFrequencyType
    {
        Daily = 0,
        Weekly = 1,
        Monthly = 2,
        OnDemand = 3
    }
}

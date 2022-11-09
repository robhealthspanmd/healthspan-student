using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.Metrics
{
    public interface IMetricCommands
    {
        UpdateMetricResponse CreateOrUpdate(MetricModel model);
        UpdateMetricResponse SetActiveStatus(int metricId, bool isActive);
        UpdateMetricResponse Delete(int metricId);
    }
}

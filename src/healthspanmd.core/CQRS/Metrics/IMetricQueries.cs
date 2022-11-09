using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.Metrics
{
    public interface IMetricQueries
    {
        ICollection<MetricModel> GetMetrics(GetMetricsQueryFilter filter, bool includeActiveUserAccount = false);
        MetricModel GetMetric(int metricId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.Metrics
{
    public class UpdateMetricResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public MetricModel Metric { get; set; }
    }
}

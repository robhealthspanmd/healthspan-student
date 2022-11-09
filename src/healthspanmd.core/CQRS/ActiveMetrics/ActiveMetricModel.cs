using healthspanmd.core.CQRS.Metrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.ActiveMetrics
{
    public class ActiveMetricModel
    {
        public long ActiveMetricId { get; set; }
        public long UserId { get; set; }
        public int MetricId { get; set; }
        public MetricModel Metric { get; set; }
        public double? Goal { get; set; }
        public double? Goal2 { get; set; }
        public MetricFrequencyType Frequency { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
    }
}

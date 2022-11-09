using healthspanmd.core.CQRS.Metrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data.Entities
{
    public class ActiveMetric
    {
        public long ActiveMetricId { get; set; }
        public long UserId { get; set; }
        public int MetricId { get; set; }
        public Metric Metric { get; set; }
        public double? Goal { get; set; }
        public double? Goal2 { get; set; }
        public MetricFrequencyType Frequency { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
    }
}

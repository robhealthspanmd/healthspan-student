using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.Metrics
{
    public class MetricModel
    {
        public int MetricId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public MetricDataType DataType { get; set; }
        public bool AllowMultipleValues { get; set; }
        public double? MinValue { get; set; }
        public double? MaxValue { get; set; }
        public bool IsActive { get; set; }
        public double? Threshold { get; set; }
        public double? Threshold2 { get; set; }
        public bool IsAlphaSelectNumeric { get; set; }
        public bool IsPositivePolarity { get; set; }
        public MetricFrequencyType Frequency { get; set; }
        public ICollection<MetricSelectItemModel> SelectItems { get; set; }
        public int UserCountWithThisMetricAsActive { get; set; }

    }
}

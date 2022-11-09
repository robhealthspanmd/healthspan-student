using System.Collections.Generic;

namespace healthspanmd.web.Models.Metric
{
    public class MetricViewModel
    {
        public int MetricId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DataType { get; set; }
        public string MinValue { get; set; }
        public string MaxValue { get; set; }
        public bool AllowMultipleValues { get; set; }
        public bool IsActive { get; set; }
        public List<MetricSelectItemViewModel> SelectItems { get; set; }
        public int Frequency { get; set; }
        public bool IsPositivePolarity { get; set; }
        public string Threshold { get; set; }
        public string Threshold2 { get; set; }
        public bool IsAlphaSelectNumeric { get; set; }
    }
}

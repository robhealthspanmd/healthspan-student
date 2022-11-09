namespace healthspanmd.web.Models.Metric
{
    public class MetricSelectItemViewModel
    {
        public int MetricSelectItemId { get; set; }
        public int MetricId { get; set; }
        public string ItemValue { get; set; }
        public string ItemDisplayText { get; set; }
        public int SortOrder { get; set; }
    }
}

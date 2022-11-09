namespace healthspanmd.web.Models.Account
{
    public class SelectedMetricViewModel
    {
        public int MetricId { get; set; }
        public int Frequency { get; set; }
        public double? Goal { get; set; }
        public double? Goal2 { get; set; }
        public int DayOfWeek { get; set; }
    }
}

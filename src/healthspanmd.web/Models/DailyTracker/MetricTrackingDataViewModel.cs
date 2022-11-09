using System.Collections.Generic;

namespace healthspanmd.web.Models.DailyTracker
{
    public class MetricTrackingDataViewModel
    {
        public int MetricId { get; set; }

        public ICollection<string> Values { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace healthspanmd.web.Models.DailyTracker
{
    public class DailyTrackerSubmissionViewModel
    {
        public ICollection<MetricTrackingDataViewModel> TrackingData { get; set; }
        public string TrackingDataForDateStr { get; set; }

    }
}

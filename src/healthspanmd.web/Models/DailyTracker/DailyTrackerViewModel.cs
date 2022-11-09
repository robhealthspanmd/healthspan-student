using healthspanmd.core.CQRS.MetricTrackingEntries;
using healthspanmd.web.Models.Content;
using healthspanmd.web.Models.Metric;
using System;
using System.Collections.Generic;

namespace healthspanmd.web.Models.DailyTracker
{
    public class DailyTrackerViewModel
    {
        public ICollection<ContentCardPreviewViewModel> ContentCardPreviews { get; set; }
        public ICollection<MetricViewModel> Metrics { get; set; }

        public string MetricsJson { get; set; }

        public DateTime TrackingDate { get; set; }

        public ICollection<MetricTrackingEntryModel> TrackingData { get; set; }
        public bool EditMode { get; set; }

        public bool IsTrackingEnabled
        {
            get
            {
                return DateTime.Today.Subtract(TrackingDate).TotalDays < 200;
            }
        }
        public bool ExistingTrackingFound
        {
            get
            {
                if (TrackingData == null)
                    return false;
                return TrackingData.Count > 0;
            }
        }
    }
}

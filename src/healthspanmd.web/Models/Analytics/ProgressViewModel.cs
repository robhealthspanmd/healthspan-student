using healthspanmd.core.CQRS.Metrics;
using healthspanmd.core.CQRS.MetricTrackingEntries;
using healthspanmd.core.CQRS.Users;
using healthspanmd.web.Models.Metric;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace healthspanmd.web.Models.Analytics
{
    public class ProgressViewModel
    {
        public enum TimePeriodType
        {
            Week,
            Month,
            Year
        }

        public UserModel User { get; set; }
        public ICollection<MetricTrackingJson> TrackingDataSets { get; set; }

        public TimePeriodType TimePeriod { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ProgramStartDate { get; set; }

        public class MetricTrackingJson
        {
            public int MetricId { get; set; }
            public string MetricName { get; set; }

            /// <summary>
            /// Set this to "ChartOptions" or "Table"
            /// </summary>
            public string ChartType { get; set; }

            /// <summary>
            /// This will get set to the serialized json representation of the ChartOptions class
            /// </summary>
            public string OptionsJson { get; set; }
        }

      
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data.Entities
{
    public class MetricTrackingEntry
    {
        public long MetricTrackingEntryId { get; set; }
        public long UserId { get; set; }
        public int MetricId { get; set; }
        public Metric Metric { get; set; }
        public DateTime EntryDateUtc { get; set; }
        public DateTime EntryForDate { get; set; }
        public ICollection<MetricTrackingEntryValue> EntryValues { get; set; }

    }
}

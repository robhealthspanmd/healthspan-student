using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.MetricTrackingEntries
{
    public class AddMetricTrackingEntryRequestItem
    {
        public long UserId { get; set; }
        public int MetricId { get; set; }
        public DateTime EntryDateUtc { get; set; }
        public DateTime EntryForDate { get; set; }
        public ICollection<MetricTrackingEntryValueModel> EntryValues { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.MetricTrackingEntries
{
    public class GetMetricTrackingEntriesQueryFilter
    {
        public long? UserId { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? MetricId { get; set; }
    }
}

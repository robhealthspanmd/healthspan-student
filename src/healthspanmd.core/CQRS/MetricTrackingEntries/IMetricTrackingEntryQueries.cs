using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.MetricTrackingEntries
{
    public interface IMetricTrackingEntryQueries
    {
        ICollection<MetricTrackingEntryModel> GetList(GetMetricTrackingEntriesQueryFilter filter);
    }
}

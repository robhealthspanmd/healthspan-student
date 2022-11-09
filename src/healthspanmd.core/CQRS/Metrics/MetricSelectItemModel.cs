using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.Metrics
{
    public class MetricSelectItemModel
    {
        public int MetricSelectItemId { get; set; }
        public int MetricId { get; set; }
        public string ItemValue { get; set; }
        public string ItemDisplayText { get; set; }
        public int SortOrder { get; set; }
    }
}

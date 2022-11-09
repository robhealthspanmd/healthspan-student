using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data.Entities
{
    public class MetricTrackingEntryValue
    {
        public long MetricTrackingEntryValueId { get; set; }
        public long MetricTrackingEntryId { get; set; }
        public double? ValueAsNumber { get; set; }
        public double? Value2AsNumber { get; set; }
        public bool? ValueAsBoolean { get; set; }
        public string ValueAsString { get; set; }
        public DateTime? ValueAsDate { get; set; }
    }
}

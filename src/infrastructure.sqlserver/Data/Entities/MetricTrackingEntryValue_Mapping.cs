using healthspanmd.core.CQRS.MetricTrackingEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data.Entities
{
    public static class MetricTrackingEntryValue_Mapping
    {
        public static MetricTrackingEntryValueModel ToMetricTrackingEntryValueModel(this MetricTrackingEntryValue e)
        {
            return new MetricTrackingEntryValueModel
            {
                MetricTrackingEntryValueId = e.MetricTrackingEntryValueId,
                MetricTrackingEntryId = e.MetricTrackingEntryId,
                ValueAsBoolean = e.ValueAsBoolean,
                ValueAsDate = e.ValueAsDate,
                ValueAsNumber = e.ValueAsNumber,
                ValueAsString = e.ValueAsString,
                Value2AsNumber = e.Value2AsNumber
            };
        }

        public static MetricTrackingEntryValue ToMetricTrackingEntryValue(this MetricTrackingEntryValueModel e)
        {
            return new MetricTrackingEntryValue
            {
                MetricTrackingEntryValueId = e.MetricTrackingEntryValueId,
                MetricTrackingEntryId = e.MetricTrackingEntryId,
                ValueAsBoolean = e.ValueAsBoolean,
                ValueAsDate = e.ValueAsDate,
                ValueAsNumber = e.ValueAsNumber,
                ValueAsString = e.ValueAsString,
                Value2AsNumber = e.Value2AsNumber
            };
        }
    }
}

using healthspanmd.core.CQRS.MetricTrackingEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data.Entities
{
    public static class MetricTrackingEntry_Mapping
    {
        public static MetricTrackingEntryModel ToMetricTrackingEntryModel(this MetricTrackingEntry e)
        {
            if (e == null)
                return null;

            return new MetricTrackingEntryModel
            {
                MetricTrackingEntryId = e.MetricTrackingEntryId,
                MetricId = e.MetricId,
                Metric = e.Metric.ToMetricModel(),
                EntryDateUtc = e.EntryDateUtc,
                EntryForDate = e.EntryForDate,
                UserId = e.UserId,
                EntryValues = e.EntryValues != null ? e.EntryValues.Select(v => v.ToMetricTrackingEntryValueModel()).ToList() : null

            };
        }


        public static MetricTrackingEntry ToMetricTrackingEntry(this MetricTrackingEntryModel m)
        {
            if (m == null)
                return null;

            return new MetricTrackingEntry
            {
                MetricTrackingEntryId = m.MetricTrackingEntryId,
                MetricId = m.MetricId,
                EntryDateUtc = m.EntryDateUtc,
                EntryForDate = m.EntryForDate,
                UserId = m.UserId,
                EntryValues = m.EntryValues != null ? m.EntryValues.Select(v => v.ToMetricTrackingEntryValue()).ToList() : null
            };
        }

        public static MetricTrackingEntry ToMetricTrackingEntry(this AddMetricTrackingEntryRequestItem m)
        {
            if (m == null)
                return null;

            return new MetricTrackingEntry
            {
                MetricId = m.MetricId,
                EntryDateUtc = m.EntryDateUtc,
                EntryForDate = m.EntryForDate,
                UserId = m.UserId,
                EntryValues = m.EntryValues != null ? m.EntryValues.Select(v => v.ToMetricTrackingEntryValue()).ToList() : null
            };
        }
    }
}

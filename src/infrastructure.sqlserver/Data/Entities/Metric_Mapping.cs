using healthspanmd.core.CQRS.Metrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data.Entities
{
    public static class Metric_Mapping
    {
        public static MetricModel ToMetricModel(this Metric m)
        {
            if (m == null)
                return null;

            return new MetricModel
            {
                MetricId = m.MetricId,
                Name = m.Name,
                Description = m.Description,
                DataType = m.DataType,
                AllowMultipleValues = m.AllowMultipleValues,
                MaxValue = m.MaxValue,
                MinValue = m.MinValue,  
                IsActive = m.IsActive,
                SelectItems = m.SelectItems != null ? m.SelectItems.Select(i => i.ToMetricSelectItemModel()).ToList() : null,
                Threshold = m.Threshold,
                Threshold2 = m.Threshold2,
                IsPositivePolarity = m.IsPositivePolarity,
                Frequency = m.Frequency,
                IsAlphaSelectNumeric = m.IsAlphaSelectNumeric,
            };
        }

        public static Metric ToMetric(this MetricModel m)
        {
            if (m == null)
                return null;

            return new Metric
            {
                MetricId = m.MetricId,
                Name = m.Name,
                Description = m.Description,
                DataType = m.DataType,
                AllowMultipleValues=m.AllowMultipleValues,
                MinValue = m.MinValue,
                MaxValue=m.MaxValue,
                IsActive = m.IsActive,
                SelectItems = m.SelectItems != null ? m.SelectItems.Select(i => i.ToMetricSelectItem()).ToList() : null,
                Threshold = m.Threshold,
                Threshold2=m.Threshold2,
                IsPositivePolarity = m.IsPositivePolarity,
                Frequency = m.Frequency,
                IsAlphaSelectNumeric = m.IsAlphaSelectNumeric,
            };
        }
    }
}

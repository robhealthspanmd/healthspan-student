using healthspanmd.core.CQRS.ActiveMetrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data.Entities
{
    public static class ActiveMetric_Mapping
    {
        public static ActiveMetricModel ToActiveMetricModel(this ActiveMetric am)
        {
            if (am == null)
                return null;

            return new ActiveMetricModel
            {
                ActiveMetricId = am.ActiveMetricId,
                UserId = am.UserId,
                MetricId = am.MetricId,
                Metric = am.Metric.ToMetricModel(),
                Goal = am.Goal,
                Goal2 = am.Goal2,
                Frequency = am.Frequency,
                DayOfWeek = am.DayOfWeek,
            };
        }


        public static ActiveMetric ToActiveMetric(this ActiveMetricModel m)
        {
            if (m == null)
                return null;


            return new ActiveMetric
            {
                ActiveMetricId = m.ActiveMetricId,
                UserId = m.UserId,
                MetricId = m.MetricId,
                Metric = m.Metric.ToMetric(),
                Goal = m.Goal,
                Goal2= m.Goal2,
                Frequency = m.Frequency,
                DayOfWeek= m.DayOfWeek,
            };
        }

    }
}

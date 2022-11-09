using healthspanmd.core.CQRS.Metrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data.Entities
{
    public static class MetricSelectItem_Mapping
    {
        public static MetricSelectItemModel ToMetricSelectItemModel(this MetricSelectItem i)
        {
            return new MetricSelectItemModel
            {
                MetricSelectItemId = i.MetricSelectItemId,
                ItemDisplayText = i.ItemDisplayText,
                ItemValue = i.ItemValue,
                MetricId = i.MetricId,
                SortOrder = i.SortOrder,
            };
        }

        public static MetricSelectItem ToMetricSelectItem(this MetricSelectItemModel i)
        {
            return new MetricSelectItem
            {
                MetricSelectItemId = i.MetricSelectItemId,
                ItemDisplayText = i.ItemDisplayText,
                ItemValue = i.ItemValue,
                MetricId = i.MetricId,
                SortOrder = i.SortOrder,
            };
        }
    }
}

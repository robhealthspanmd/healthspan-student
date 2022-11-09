using healthspanmd.core.CQRS.MetricTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data.Entities
{
    public static class MetricType_Mapping
    {
        public static MetricTypeModel ToMetricTypeModel(this MetricType mt)
        {
            if (mt == null)
                return null;

            return new MetricTypeModel
            {
                MetricTypeId = mt.MetricTypeId,
                Name = mt.Name,
                Description = mt.Description,
                DataType = mt.DataType
            };
        }


        public static MetricType ToMetricType(this MetricTypeModel m)
        {
            if (m == null)
                return null;

            return new MetricType
            {
                MetricTypeId = m.MetricTypeId,
                Name = m.Name,
                DataType = m.DataType,
                Description = m.Description
            };
        }
    }
}

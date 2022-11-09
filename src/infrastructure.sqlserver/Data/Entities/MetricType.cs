using healthspanmd.core.CQRS.MetricTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data.Entities
{
    public class MetricType
    {
        public int MetricTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public MetricDataTypeEnum DataType { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.MetricTypes
{
    public class UpdateMetricTypeResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public MetricTypeModel MetricType { get; set; }
    }
}

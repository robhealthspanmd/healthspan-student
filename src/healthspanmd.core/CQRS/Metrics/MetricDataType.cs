using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.Metrics 
{ 
    public enum MetricDataType
    {
        YesNo = 0,
        Numeric_Integer = 1,
        Numeric_Decimal = 2,
        Alpha = 3,
        Alpha_Select = 4,
        Date = 5,
        Numeric_Integer_Dial = 6,
        Numeric_Integer_Slider = 7,
        Numeric_Integer_2Values = 8
    }
}

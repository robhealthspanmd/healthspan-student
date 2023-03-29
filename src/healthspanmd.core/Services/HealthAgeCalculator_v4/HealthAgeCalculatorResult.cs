using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.Services.HealthAgeCalculator_v3
{
    public class HealthAgeCalculatorResult
    {
        public bool Success { get; set; }
        public double HealthAge { get; set; }
        public string Message { get; set; }
        public Patient Input { get; set; }
    }
}

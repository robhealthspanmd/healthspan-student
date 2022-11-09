using System;
using System.Collections.Generic;
using System.Text;

namespace healthspanmd.core.Services.HealthAgeCalculator_v2
{
    public class HealthAgeCalculatorResult
    {
        public bool Success { get; set; }
        public double HealthAge { get; set; }
        public string Message { get; set; }
        public HealthAgeCalculatorInputModel Input { get; set; }
    }
}

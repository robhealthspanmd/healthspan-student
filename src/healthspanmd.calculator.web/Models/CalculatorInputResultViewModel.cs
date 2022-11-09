using healthspanmd.core.Services.HealthAgeCalculator_v2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace healthspanmd.calculator.web.Models
{
    public class CalculatorInputResultViewModel
    {
        public HealthAgeCalculatorInputModel Input { get; set; }

        public CalculatorResultModel Result { get; set; }
    }
}

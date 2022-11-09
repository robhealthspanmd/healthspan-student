using System;
using System.Collections.Generic;
using System.Text;

namespace healthspanmd.core.Services.HealthAgeCalculator_v2
{
    public interface IHealthAgeCalculator
    {
        HealthAgeCalculatorResult Execute(HealthAgeCalculatorInputModel model);
    }
}

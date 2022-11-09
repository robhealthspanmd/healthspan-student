using System;
using System.Collections.Generic;
using System.Text;

namespace healthspanmd.core.Services.HealthAgeCalculator_v1
{
    public interface IHealthAgeCalculator
    {
        HealthAgeCalculatorResult Execute(HealthAgeCalculatorInputModel model);
    }
}

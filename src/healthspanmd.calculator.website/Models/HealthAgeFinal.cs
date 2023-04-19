using healthspanmd.core.Services.HealthAgeCalculator_v3;

namespace healthspanmd.calculator.website.Models
{
    public class HealthAgeFinal
    {
       public HealthAgeCalculatorResult? result { get; set; }

        public Patient? patient { get; set; }
    }
}

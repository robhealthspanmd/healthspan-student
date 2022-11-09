using System;
using System.Collections.Generic;
using System.Text;

namespace healthspanmd.core.Services.HealthAgeCalculator_v1
{
    public class HealthAgeCalculatorInputModel
    {
        public enum SexType
        {
            Male,
            Female
        }

        public enum SmokerType
        {
            Never,
            FormerLight,
            FormerHeavy,
            Current
        }

        public SexType Sex { get; set; }
        public int Age { get; set; }
        public int WaistCircumference { get; set; }
        public int AerobicPhysicalActivity { get; set; }
        public int StrengthTraining { get; set; }
        public int BodyFatPercentage { get; set; }
        public int NutritionQuality { get; set; }
        public int Sleep { get; set; }
        public int NonHDLCholesterol { get; set; }
        public int BloodPressureSystolic { get; set; }
        public int BloodPressureDiastolic { get; set; }
        public SmokerType Smoking { get; set; }
        public double HemoglobinA1c { get; set; }



    }
}

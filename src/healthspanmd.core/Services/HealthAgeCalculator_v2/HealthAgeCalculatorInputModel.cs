using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace healthspanmd.core.Services.HealthAgeCalculator_v2
{
    public class HealthAgeCalculatorInputModel
    {
        public enum SexType
        {
            Male,
            Female
        }

        //public enum SmokerType
        //{
        //    Never,
        //    FormerLight,
        //    FormerHeavy,
        //    Current
        //}

        [DisplayName("Name")]
        public string Name { get; set; }
        public string Email { get; set; }

        [DisplayName("Height (inches)")]
        public int? HeightInInches { get; set; }

        [DisplayName("Weight (lbs)")]
        public int? WeightInPounds { get; set; }

        [DisplayName("Waist Circumference")]
        public int? WaistCircumference { get; set; }

        [DisplayName("Body Fat Percentage")]
        public int? BodyFatPercentage { get; set; }

        public SexType? Sex { get; set; }

        [DisplayName("Moderate Activity per day (minutes)")]
        public int? ModerateActivityMinutes { get; set; }

        [DisplayName("Vigorous Activity per day (minutes)")]
        public int? VigorousActivityMinutes { get; set; }

        [DisplayName("Strength Training per day (minutes)")]
        public int? StrengthTrainingMinutes { get; set; }

        [DisplayName("Red Food Count (per week)")]
        public int? RedFoodCount { get; set; }

        [DisplayName("Sleep per day (hours)")]
        public int? SleepHours { get; set; }

        [DisplayName("Total Cholesterol")]
        public int? TotalCholesterol { get; set; }

        [DisplayName("HDL Cholesterol")]
        public int? HDLCholesterol { get; set; }

        
        public int Age { get; set; }
 
        [DisplayName("Blood Pressure - Systolic")]
        public int? BloodPressureSystolic { get; set; }

        [DisplayName("Blood Pressure - Diastolic")]
        public int? BloodPressureDiastolic { get; set; }

        [DisplayName("Are you a smoker?")]
        public bool? Smoker { get; set; }

        [DisplayName("Do you have diabetes or pre-diabetes?")]
        public bool? DiabetesOrPreDiabetes { get; set; }

        [DisplayName("Blood Sugar (Hemoglobin A1c)")]
        public double? HemoglobinA1c { get; set; }



    }
}

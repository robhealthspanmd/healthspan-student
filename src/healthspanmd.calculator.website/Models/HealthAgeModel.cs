using System.ComponentModel.DataAnnotations;

namespace healthspanmd.calculator.website.Models
{
    public class HealthAgeModel
    {
        [Display(Name = "Enter your name: ")]
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public int HeightFeet { get; set; }
        public int HeightInches { get; set; }
        public int Weight { get; set; }
        public string ModActivity { get; set; }
        public string VigActivity { get; set; }
        public string SugarDrink { get; set; }
        public string SevenA { get; set; }
        public string SevenB { get; set; }
        public string SevenC { get; set; }
        public string DeepFriedFoods { get; set; }
        public string ProcessedMeatCheese { get; set; }
        public string AlcoholicDrinks { get; set; }
        public string BodyType { get; set; }
        public string SleepHours { get; set; }
        public string CholesterolLevel { get; set; }
        public string BloodPressure { get; set; }
        public string SmokingStatus { get; set; }
        public string DiabetesStatus { get; set; }
        public string Loneliness { get; set; }
        public string StressEffect { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;

namespace healthspanmd.core.Services.HealthAgeCalculator_v3
{

    public class Patient
    {


        [DisplayName("Name")]
        public String name { get; set; }
        public String email { get; set; }
        public int age { get; set; }
        public int heightFt { get; set; }
        public int heightIn { get; set; }
        public double weight { get; set; }
        public string moderateActivity { get; set; }
        public string vigorousActivity { get; set; }
        public string strengthTraining { get; set; }
        public string sugarAddedDrinks { get; set; }
        public string addedSugar { get; set; }
        public string refinedGrains { get; set; }
        public string deepFriedFoods { get; set; }
        public string processedFoods { get; set; }
        public string AlcoholicDrinks { get; set; }
        public string BMI { get; set; }
        public string sleep { get; set; }
        public string cholesterol { get; set; }
        public string bloodPressure { get; set; }
        public string smoker { get; set; }
        public string diabetes { get; set; }
        public string lonely { get; set; }
        public string stress { get; set; }
        public bool canEmail { get; set; }

    }
}
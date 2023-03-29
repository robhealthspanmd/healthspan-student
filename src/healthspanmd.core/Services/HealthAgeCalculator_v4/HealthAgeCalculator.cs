using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.Services.HealthAgeCalculator_v3
{
    public class HealthAgeCalculator
    {

        private Patient _patient;

        
        public HealthAgeCalculatorResult CalculateResult(Patient patient)
        {
            _patient = patient;

            try
            {
                var age = Convert.ToDouble(_patient.age);

                var healthage = (Z1_PhysicalActivity() + Z2_StrengthTraining() +
                    + Z3_RedFoods() + Z4_BMI() + Z5_Sleep() + Z6_Cholesterol() + Z7_BloodPressure()
                    + Z8_Smoking() + Z9_Diabetes() + Z10_Lonely() + Z11_Stress()) / 2;

                healthage = healthage + age;

                return new HealthAgeCalculatorResult()
                {
                    Success = true,
                    HealthAge = healthage,
                    Input = patient
                };
            }
            catch (Exception ex)
            {
                return new HealthAgeCalculatorResult()
                {
                    Success = false,
                    Message = ex.Message,
                    Input = patient
                };
            }

            
        }
        private double Z1_PhysicalActivity()
        {
           
            int activity = 0;
            if (_patient.moderateActivity.Equals("less-than-5"))
            {
                activity += 5;
            }
            else if (_patient.moderateActivity.Equals("about-15"))
            {
                activity += 15;
            }
            else if (_patient.moderateActivity.Equals("about-30"))
            {
                activity += 30;
            }
            else if (_patient.moderateActivity.Equals("more-than-45"))
            {
                activity += 45;
            }
            if (_patient.vigorousActivity.Equals("none"))
            {
                activity += 0;
            }
            else if (_patient.vigorousActivity.Equals("about-10"))
            {
                activity += 10;
            }
            else if (_patient.vigorousActivity.Equals("more-than-20"))
            {
                activity += 20;
            }
            

            if (activity < 5)
                return _patient.age * .06;
            else if (activity < 15)
                return _patient.age * .02;
            else if (activity < 30)
                return 0;
            else if (activity < 45)
                return _patient.age * -.03;
            else
                return _patient.age * -.06;
         


        }
     
        
        private double Z2_StrengthTraining()
        {
            if (_patient.strengthTraining.Equals("less-than-30"))
                return _patient.age * .04;
            else if (_patient.strengthTraining.Equals("30-50"))
                return _patient.age * .01;
            else if (_patient.strengthTraining.Equals("60-119"))
                return _patient.age * -.02;
            else if (_patient.strengthTraining.Equals("more-than-120"))
                return _patient.age * -.06;


            return 0;
        }
        private double Z3_RedFoods()
        {
            int servings = (redFoodServings(_patient.sugarAddedDrinks) + redFoodServings(_patient.addedSugar) + redFoodServings(_patient.refinedGrains)
                + redFoodServings(_patient.deepFriedFoods) + redFoodServings(_patient.processedFoods) + redFoodServings(_patient.AlcoholicDrinks)) * 7;
           
            if (servings <= 7)
                return _patient.age * -.11;
            else if (servings <= 12)
                return _patient.age * -.05;
            else if (servings <= 20)
                return _patient.age * .01;
            else if (servings <= 27)
                return _patient.age * .06;
            else if (servings > 27)
                return _patient.age * .11;
            return 0;
        }
        private double Z4_BMI()
        {
            if (_patient.BMI.Equals("too-thin"))
                return _patient.age * .02;
            else if (_patient.BMI.Equals("healthy"))
                return _patient.age * -.05;
            else if (_patient.BMI.Equals("excess-fat"))
                return _patient.age * -.01;
            else if (_patient.BMI.Equals("mild-overweight"))
                return _patient.age * .05;
            else if (_patient.BMI.Equals("moderate-overweight"))
                return _patient.age * .08;
            else if (_patient.BMI.Equals("severe-overweight"))
                return _patient.age * .10;
            return 0;
        }
        private double Z5_Sleep()
        {
            if (_patient.sleep.Equals("less-than-7"))
                return _patient.age * .03;
            else if (_patient.sleep.Equals("7-to-9"))
                return _patient.age * -.03;
            else if (_patient.sleep.Equals("more-than-9"))
                return _patient.age * .05;
            return 0;
        }
        private double Z6_Cholesterol()
        {
            if (_patient.cholesterol.Equals("great"))
                return _patient.age * -.04;
            else if (_patient.cholesterol.Equals("good"))
                return _patient.age * -.02;
            else if (_patient.cholesterol.Equals("high"))
                return _patient.age * .02;
            else if (_patient.cholesterol.Equals("really-high"))
                return _patient.age * .04;
            return 0;
        }

        private double Z7_BloodPressure()
        {
            if (_patient.bloodPressure.Equals("great"))
                return _patient.age * -.05;
            else if (_patient.bloodPressure.Equals("borderline-high"))
                return _patient.age * -.02;
            else if (_patient.bloodPressure.Equals("high"))
                return _patient.age * .03;
            else if (_patient.bloodPressure.Equals("moderately-high"))
                return _patient.age * .05;
            else if (_patient.bloodPressure.Equals("severely-elevated"))
                return _patient.age * .08;
            return 0;
        }

        private double Z8_Smoking()
        {
            if (_patient.smoker.Equals("currently-smoking"))
                return _patient.age * .12;
            return 0;
        }

        private double Z9_Diabetes()
        {
            if (_patient.diabetes.Equals("no-diabetes"))
                return _patient.age * -.06;
            else if (_patient.diabetes.Equals("pre-diabetes"))
                return _patient.age * .01;
            else if (_patient.diabetes.Equals("well-controlled-diabetes"))
                return _patient.age * .02;
            else if (_patient.diabetes.Equals("not-well-controlled-diabetes"))
                return _patient.age * .06;
            return 0;
        }
        private double Z10_Lonely()
        {
            if (_patient.lonely.Equals("rarely"))
                return _patient.age * -.03;
            else if (_patient.lonely.Equals("sometimes"))
                return _patient.age * -.01;
            else if (_patient.lonely.Equals("often"))
                return _patient.age * .03;
            
            return 0;
        }
        private double Z11_Stress()
        {
            if (_patient.stress.Equals("a-little"))
                return _patient.age * -.03;
            else if (_patient.stress.Equals("some"))
                return _patient.age * -.01;
            else if (_patient.stress.Equals("a-lot"))
                return _patient.age * .03;

            return 0;
        }

        private int redFoodServings(string s)
        {
            if (s.Equals("4-or-more"))
            {
                return 4;
            }
            return Convert.ToInt32(s);
        }
    }
}

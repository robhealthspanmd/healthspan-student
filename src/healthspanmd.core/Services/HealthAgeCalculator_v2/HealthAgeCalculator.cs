using System;
using System.Collections.Generic;
using System.Text;

namespace healthspanmd.core.Services.HealthAgeCalculator_v2
{
    public class HealthAgeCalculator : IHealthAgeCalculator
    {

        private HealthAgeCalculatorInputModel _model;

        public HealthAgeCalculatorResult Execute(HealthAgeCalculatorInputModel model)
        {
            _model = model;

            try
            {
                var result = Convert.ToDouble(_model.Age);

                if (_model.WaistCircumference.HasValue && _model.BodyFatPercentage.HasValue & _model.Sex.HasValue)
                {
                    result += Adj_WaistCircumference() + Adj_BodyFatPercentage();
                }
                else
                {
                    result += Z1_BodyCompositionAndWeight();
                }

                result += Z2_AerobicExercise()
                   + Z3_StrengthTraining()
                   + Z4_RedFoods()
                   + Z5_Sleep()
                   + Z6_Cholesterol()
                   + Z7_BloodPressure()
                   + Z8_Smoking()
                   + Z9_BloodGlucose();

                return new HealthAgeCalculatorResult()
                {
                    Success = true,
                    HealthAge = result,
                    Input = model
                };
            }
            catch (Exception ex)
            {
                return new HealthAgeCalculatorResult()
                {
                    Success = false,
                    Message = ex.Message,
                    Input = model
                };
            }
            
            

        }

       

        private double GetBMI(int weightInPounds, int HeightInInches)
        {
            return (double)weightInPounds / Math.Pow(((double)HeightInInches), 2) * 703;
        }


        private double Z1_BodyCompositionAndWeight()
        {
            if (_model.WeightInPounds.HasValue && _model.HeightInInches.HasValue)
            {
                var bmi = GetBMI((int)_model.WeightInPounds, (int)_model.HeightInInches);

                var Y = _model.Age * 0.25;

                if (bmi < 18.5)  // too thin
                    return Y * 0.04;
                else if (bmi <= 25)  // healthy
                    return Y * (-.16);
                else if (bmi <= 30)  // a few extra pounds
                    return Y * (-.12);
                else if (bmi <= 35)  // overweight
                    return Y * 0.04;
                else if (bmi <= 40)  // moderately overweight
                    return Y * 0.1;
                else // severely overweight
                    return _model.Age * 0.25 * 0.16;
            }
            else
            {
                return 0.0;
            }
            
        }

        private double Adj_WaistCircumference()
        {
            if (_model.Age >= 20 && _model.Age <= 39 && _model.Sex == HealthAgeCalculatorInputModel.SexType.Male)
                return Zee1M2039();
            else if (_model.Age >= 40 && _model.Age <= 79 && _model.Sex == HealthAgeCalculatorInputModel.SexType.Male)
                return Zee1M4079();
            else if (_model.Age >= 20 && _model.Age <= 39 && _model.Sex == HealthAgeCalculatorInputModel.SexType.Female)
                return Zee1W2039();
            else if (_model.Age >= 40 && _model.Age <= 79 && _model.Sex == HealthAgeCalculatorInputModel.SexType.Female)
                return Zee1W4079();
            else
                throw new Exception("Waist Circumference adjustment is not available for sex/age combination");
        }

        private double Zee1M2039()
        {
            if (_model.WaistCircumference <= 31)
                return (double)_model.Age * 0.33 * (-0.09);
            else if (_model.WaistCircumference <= 35)
                return (double)_model.Age * 0.33 * (-0.05);
            else if (_model.WaistCircumference <= 39)
                return (double)_model.Age * 0.33 * 0;
            else if (_model.WaistCircumference <= 43)
                return (double)_model.Age * 0.33 * 0.05;
            else
                return (double)_model.Age * 0.33 * 0.09;
        }

        private double Zee1M4079()
        {
            if (_model.WaistCircumference <= 34)
                return (double)_model.Age * 0.33 * (-0.09);
            else if (_model.WaistCircumference <= 37)
                return (double)_model.Age * 0.33 * (-0.05);
            else if (_model.WaistCircumference <= 41)
                return (double)_model.Age * 0.33 * 0;
            else if (_model.WaistCircumference <= 45)
                return (double)_model.Age * 0.33 * 0.05;
            else
                return (double)_model.Age * 0.33 * 0.09;
        }

        private double Zee1W2039()
        {
            if (_model.WaistCircumference <= 27)
                return (double)_model.Age * 0.33 * (-0.09);
            else if (_model.WaistCircumference <= 31)
                return (double)_model.Age * 0.33 * (-0.05);
            else if (_model.WaistCircumference <= 34)
                return (double)_model.Age * 0.33 * 0;
            else if (_model.WaistCircumference <= 37)
                return (double)_model.Age * 0.33 * 0.05;
            else
                return (double)_model.Age * 0.33 * 0.09;
        }

        public double Zee1W4079()
        {
            if (_model.WaistCircumference <= 29)
                return (double)_model.Age * 0.33 * (-0.09);
            else if (_model.WaistCircumference <= 33)
                return (double)_model.Age * 0.33 * (-0.05);
            else if (_model.WaistCircumference <= 36)
                return (double)_model.Age * 0.33 * 0;
            else if (_model.WaistCircumference <= 39)
                return (double)_model.Age * 0.33 * 0.05;
            else
                return (double)_model.Age * 0.33 * 0.09;
        }

        private double Adj_BodyFatPercentage()
        {
            if (_model.Age >= 20 && _model.Age <= 39 && _model.Sex == HealthAgeCalculatorInputModel.SexType.Male)
                return BodyFat_M2039();
            else if (_model.Age >= 40 && _model.Age <= 59 && _model.Sex == HealthAgeCalculatorInputModel.SexType.Male)
                return BodyFat_M4059();
            else if (_model.Age >= 60 && _model.Age <= 79 && _model.Sex == HealthAgeCalculatorInputModel.SexType.Male)
                return BodyFat_M6079();
            else if (_model.Age >= 20 && _model.Age <= 39 && _model.Sex == HealthAgeCalculatorInputModel.SexType.Female)
                return BodyFat_W2039();
            else if (_model.Age >= 40 && _model.Age <= 59 && _model.Sex == HealthAgeCalculatorInputModel.SexType.Female)
                return BodyFat_W4059();
            else if (_model.Age >= 60 && _model.Age <= 79 && _model.Sex == HealthAgeCalculatorInputModel.SexType.Female)
                return BodyFat_W6079();
            else
                throw new Exception("Body Fat Percentage adjustment is not available for sex/age combination");
        }

        public double BodyFat_M2039()
        {
            if (_model.BodyFatPercentage <= 17)
                return (double)_model.Age * 0.33 * (-0.07);
            else if (_model.BodyFatPercentage <= 23)
                return (double)_model.Age * 0.33 * (-0.03);
            else if (_model.BodyFatPercentage <= 29)
                return (double)_model.Age * 0.33 * 0;
            else if (_model.BodyFatPercentage <= 35)
                return (double)_model.Age * 0.33 * 0.03;
            else
                return (double)_model.Age * 0.33 * 0.07;
        }

        public double BodyFat_M4059()
        {
            if (_model.BodyFatPercentage <= 19)
                return (double)_model.Age * 0.33 * (-0.07);
            else if (_model.BodyFatPercentage <= 24)
                return (double)_model.Age * 0.33 * (-0.03);
            else if (_model.BodyFatPercentage <= 29)
                return (double)_model.Age * 0.33 * 0;
            else if (_model.BodyFatPercentage <= 35)
                return (double)_model.Age * 0.33 * 0.03;
            else
                return (double)_model.Age * 0.33 * 0.07;
        }

        public double BodyFat_M6079()
        {
            if (_model.BodyFatPercentage <= 24)
                return (double)_model.Age * 0.33 * (-0.07);
            else if (_model.BodyFatPercentage <= 28)
                return (double)_model.Age * 0.33 * (-0.03);
            else if (_model.BodyFatPercentage <= 32)
                return (double)_model.Age * 0.33 * 0;
            else if (_model.BodyFatPercentage <= 37)
                return (double)_model.Age * 0.33 * 0.03;
            else
                return (double)_model.Age * 0.33 * 0.07;
        }

        public double BodyFat_W2039()
        {
            if (_model.BodyFatPercentage <= 28)
                return (double)_model.Age * 0.33 * (-0.07);
            else if (_model.BodyFatPercentage <= 34)
                return (double)_model.Age * 0.33 * (-0.03);
            else if (_model.BodyFatPercentage <= 40)
                return (double)_model.Age * 0.33 * 0;
            else if (_model.BodyFatPercentage <= 46)
                return (double)_model.Age * 0.33 * 0.03;
            else
                return (double)_model.Age * 0.33 * 0.07;
        }

        public double BodyFat_W4059()
        {
            if (_model.BodyFatPercentage <= 31)
                return (double)_model.Age * 0.33 * (-0.07);
            else if (_model.BodyFatPercentage <= 36)
                return (double)_model.Age * 0.33 * (-0.03);
            else if (_model.BodyFatPercentage <= 41)
                return (double)_model.Age * 0.33 * 0;
            else if (_model.BodyFatPercentage <= 47)
                return (double)_model.Age * 0.33 * 0.03;
            else
                return (double)_model.Age * 0.33 * 0.07;
        }

        public double BodyFat_W6079()
        {
            if (_model.BodyFatPercentage <= 34)
                return (double)_model.Age * 0.33 * (-0.07);
            else if (_model.BodyFatPercentage <= 38)
                return (double)_model.Age * 0.33 * (-0.03);
            else if (_model.BodyFatPercentage <= 43)
                return (double)_model.Age * 0.33 * 0;
            else if (_model.BodyFatPercentage <= 47)
                return (double)_model.Age * 0.33 * 0.03;
            else
                return (double)_model.Age * 0.33 * 0.07;
        }


        private double Z2_AerobicExercise()
        {
            if (!_model.ModerateActivityMinutes.HasValue && !_model.VigorousActivityMinutes.HasValue)
                return 0.0;

            int minutesEffective = _model.ModerateActivityMinutes.HasValue ? (int)_model.ModerateActivityMinutes : 0;
            if (_model.VigorousActivityMinutes.HasValue)
                minutesEffective += 2 * (int)_model.VigorousActivityMinutes;
                
            var Y = _model.Age * 0.25;

            if (minutesEffective < 5)
                return Y * 0.08;
            else if (minutesEffective <= 25)
                return Y * 0.02;
            else if (minutesEffective <= 45)
                return Y * (-0.05);
            else
                return Y * (-0.08);
        }

        private double Z3_StrengthTraining()
        {
            if (!_model.StrengthTrainingMinutes.HasValue)
                return 0.0;

            var Y = _model.Age * 0.25;

            if (_model.StrengthTrainingMinutes == 0.0)
                return Y * 0.08;
            else if (_model.StrengthTrainingMinutes < 10)
                return Y * 0.03;
            else if (_model.StrengthTrainingMinutes < 20)
                return Y * (-0.05);
            else
                return Y * (-0.08);
        }

        private double Z4_RedFoods()
        {
            if (_model.RedFoodCount.HasValue)
            {
                var Y = _model.Age * 0.25;

                if (_model.RedFoodCount <= 7)
                    return Y * (-0.14);
                else if (_model.RedFoodCount < 14)
                    return Y * (-0.07);
                else if (_model.RedFoodCount < 21)
                    return 0.0;
                else if (_model.RedFoodCount < 28)
                    return Y * 0.07;
                else
                    return Y * 0.14;

            }
            else
            {
                // no data
                return 0.0;
            }
        }

        private double Z5_Sleep()
        {
            if (_model.SleepHours.HasValue)
            {
                var Y = _model.Age * 0.25;

                if (_model.SleepHours < 7)
                    return 0.0;
                else if (_model.SleepHours >= 7 && _model.SleepHours <= 9)
                    return Y * (-0.06);
                else
                    return Y * 0.06;
            }
            else
            {
                return 0.0;
            }
        }

        private double Z6_Cholesterol()
        {
            if (_model.TotalCholesterol.HasValue && _model.HDLCholesterol.HasValue)
            {
                var cholesterol = _model.TotalCholesterol - _model.HDLCholesterol;
                var Y = _model.Age * 0.25;

                if (cholesterol <= 100) // Great
                    return Y * (-0.10);
                else if (cholesterol <= 160)    // Good
                    return Y * (-0.05);
                else if (cholesterol <= 190) // High
                    return Y * 0.05;
                else      // Really High
                    return Y * 0.10;
            }
            else
            {
                // no data
                return 0.0;
            }
        }

        private double Z7_BloodPressure()
        {
            if (_model.BloodPressureDiastolic.HasValue && _model.BloodPressureSystolic.HasValue)
            {
                var Y = _model.Age * 0.25;

                if (_model.BloodPressureSystolic < 120 && _model.BloodPressureDiastolic < 80)   // Great
                    return Y * (-0.10);
                else if (_model.BloodPressureSystolic < 130 && _model.BloodPressureDiastolic < 80) // Borderline High
                    return 0.0;
                else if (_model.BloodPressureSystolic < 140 && _model.BloodPressureDiastolic < 90)  // High
                    return Y * 0.05;
                else if (_model.BloodPressureSystolic < 150 && _model.BloodPressureDiastolic < 100) // Moderately High
                    return Y * 0.075;
                else   // Severely High
                    return Y * 0.10;
            }
            else
            {
                // no data
                return 0.0;
            }
        }


        private double Z8_Smoking()
        {
            if (_model.Smoker.HasValue)
            {
                var Y = _model.Age * 0.25;

                if ((bool)_model.Smoker)
                    return Y * (0.15);
                else
                    return Y * (-0.15);

            }
            else
            {
                // no data
                return 0.0;
            }
        }


        private double Z9_BloodGlucose()
        {
            if (_model.HemoglobinA1c.HasValue && _model.DiabetesOrPreDiabetes.HasValue)
            {
                var Y = _model.Age * 0.25;

                if (!(bool)_model.DiabetesOrPreDiabetes) // no diabetes
                    return Y * (-0.15);
                else if (_model.HemoglobinA1c < 6.4) // pre-diabetes 
                    return Y * (0.075);
                else if (_model.HemoglobinA1c < 7.0) // diabetes - controlled
                    return Y * (0.075);             
                else     // i have diabetes - not well controlled
                    return Y * (0.15);
            }
            else
            {
                // no data
                return 0.0;
            }
        }

    }
}

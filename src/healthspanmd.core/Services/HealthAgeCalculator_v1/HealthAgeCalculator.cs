using System;
using System.Collections.Generic;
using System.Text;

namespace healthspanmd.core.Services.HealthAgeCalculator_v1
{
    public class HealthAgeCalculator : IHealthAgeCalculator
    {

        private HealthAgeCalculatorInputModel _model;

        public HealthAgeCalculatorResult Execute(HealthAgeCalculatorInputModel model)
        {
            _model = model;

            try
            {
                var result = _model.Age
                    + Adj_WaistCircumference()
                    + Adj_AerobicPhysicalActivity()
                    + Adj_StrengthTraining()
                    + Adj_BodyFatPercentage()
                    + Adj_NutritionQuality()
                    + Adj_Sleep()
                    + Adj_NonHDLCholesterol()
                    + Adj_BloodPressure()
                    + Adj_Smoking()
                    + Adj_HemoglobinA1c();
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

        private double Adj_AerobicPhysicalActivity()
        {
            return Zee2();
        }

        private double Adj_StrengthTraining()
        {
            return Zee3();
        }

        private double Adj_BodyFatPercentage()
        {
            if (_model.Age >= 20 && _model.Age <= 39 && _model.Sex == HealthAgeCalculatorInputModel.SexType.Male)
                return Zee4M2039();
            else if (_model.Age >= 40 && _model.Age <= 59 && _model.Sex == HealthAgeCalculatorInputModel.SexType.Male)
                return Zee4M4059();
            else if (_model.Age >= 60 && _model.Age <= 79 && _model.Sex == HealthAgeCalculatorInputModel.SexType.Male)
                return Zee4M6079();
            else if (_model.Age >= 20 && _model.Age <= 39 && _model.Sex == HealthAgeCalculatorInputModel.SexType.Female)
                return Zee4W2039();
            else if (_model.Age >= 40 && _model.Age <= 59 && _model.Sex == HealthAgeCalculatorInputModel.SexType.Female)
                return Zee4W4059();
            else if (_model.Age >= 60 && _model.Age <= 79 && _model.Sex == HealthAgeCalculatorInputModel.SexType.Female)
                return Zee4W6079();
            else
                throw new Exception("Body Fat Percentage adjustment is not available for sex/age combination");
        }

        private double Adj_NutritionQuality()
        {
            return Zee5();
        }

        private double Adj_Sleep()
        {
            return Zee6();
        }

        private double Adj_NonHDLCholesterol()
        {
            return Zee7();
        }

        private double Adj_BloodPressure()
        {
            return Zee8Top() > Zee8Bottom() ? Zee8Top() : Zee8Bottom();
        }

        private double Adj_Smoking()
        {
            return Zee9();
        }

        private double Adj_HemoglobinA1c()
        {
            return Zee10();
        }

        private double Zee1M2039()
        {
            if (_model.WaistCircumference <= 31)
                return (double)_model.Age * 0.33 * (-0.11);
            else if (_model.WaistCircumference <= 35)
                return (double)_model.Age * 0.33 * (-0.05);
            else if (_model.WaistCircumference <= 39)
                return (double)_model.Age * 0.33 * 0;
            else if (_model.WaistCircumference <= 43)
                return (double)_model.Age * 0.33 * 0.05;
            else
                return (double)_model.Age * 0.33 * 0.11;
        }

        private double Zee1M4079()
        {
            if (_model.WaistCircumference <= 34)
                return (double)_model.Age * 0.33 * (-0.11);
            else if (_model.WaistCircumference <= 37)
                return (double)_model.Age * 0.33 * (-0.05);
            else if (_model.WaistCircumference <= 41)
                return (double)_model.Age * 0.33 * 0;
            else if (_model.WaistCircumference <= 45)
                return (double)_model.Age * 0.33 * 0.05;
            else
                return (double)_model.Age * 0.33 * 0.11;
        }

        private double Zee1W2039()
        {
            if (_model.WaistCircumference <= 27)
                return (double)_model.Age * 0.33 * (-0.11);
            else if (_model.WaistCircumference <= 31)
                return (double)_model.Age * 0.33 * (-0.05);
            else if (_model.WaistCircumference <= 34)
                return (double)_model.Age * 0.33 * 0;
            else if (_model.WaistCircumference <= 37)
                return (double)_model.Age * 0.33 * 0.05;
            else
                return (double)_model.Age * 0.33 * 0.11;
        }

        public double Zee1W4079()
        {
            if (_model.WaistCircumference <= 29)
                return (double)_model.Age * 0.33 * (-0.11);
            else if (_model.WaistCircumference <= 33)
                return (double)_model.Age * 0.33 * (-0.05);
            else if (_model.WaistCircumference <= 36)
                return (double)_model.Age * 0.33 * 0;
            else if (_model.WaistCircumference <= 39)
                return (double)_model.Age * 0.33 * 0.05;
            else
                return (double)_model.Age * 0.33 * 0.11;
        }

        public double Zee2()
        {
            if (_model.AerobicPhysicalActivity <= 30)
                return (double)_model.Age * 0.33 * 0.07;
            else if (_model.AerobicPhysicalActivity <= 90)
                return (double)_model.Age * 0.33 * 0.03;
            else if (_model.AerobicPhysicalActivity <= 300)
                return (double)_model.Age * 0.33 * (-0.03);
            else
                return (double)_model.Age * 0.33 * (-0.07);
        }

        public double Zee3()
        {
            if (_model.StrengthTraining <= 0)
                return (double)_model.Age * 0.33 * 0.07;
            else if (_model.StrengthTraining <= 20)
                return (double)_model.Age * 0.33 * 0.03;
            else if (_model.StrengthTraining <= 120)
                return (double)_model.Age * 0.33 * (-0.03);
            else 
                return (double)_model.Age * 0.33 * (-0.07);
        }

        //public double Zee11()
        //{
        //    // what is FAC? (B18)
        //}

        public double Zee6()
        {
            if (_model.Sleep < 7)
                return (double)_model.Age * 0.33 * 0.02;
            else if (_model.Sleep <= 9)
                return (double)_model.Age * 0.33 * (-0.06);
            else
                return (double)_model.Age * 0.33 * 0.06;
        }

        public double Zee7()
        {
            if (_model.NonHDLCholesterol <= 100)
                return (double)_model.Age * 0.33 * (-0.11);
            else if (_model.NonHDLCholesterol <= 130)
                return (double)_model.Age * 0.33 * (-0.05);
            else if (_model.NonHDLCholesterol <= 160)
                return (double)_model.Age * 0.33 * 0.05;
            else
                return (double)_model.Age * 0.33 * 0.11;
        }

        public double Zee8Top()
        {
            if (_model.BloodPressureSystolic <= 120)
                return (double)_model.Age * -.33 * (-0.11);
            else if (_model.BloodPressureSystolic <= 130)
                return (double)_model.Age * -.33 * 0.025;
            else if (_model.BloodPressureSystolic <= 140)
                return (double)_model.Age * -.33 * 0.05;
            else if (_model.BloodPressureSystolic <= 160)
                return (double)_model.Age * -.33 * 0.075;
            else
                return (double)_model.Age * -.33 * 0.11;
        }

        public double Zee8Bottom()
        {
            if (_model.BloodPressureDiastolic <= 80)
                return (double)_model.Age * 0.33 *(-0.11);
            else if (_model.BloodPressureDiastolic <= 90)
                return (double)_model.Age * 0.33 * 0.05;
            else if (_model.BloodPressureDiastolic <= 99)
                return (double)_model.Age * 0.33 * 0.075;
            else
                return (double)_model.Age * 0.33 * 0.11;
        }

        public double Zee4M2039()
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

        public double Zee4M4059()
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

        public double Zee4M6079()
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

        public double Zee4W2039()
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

        public double Zee4W4059()
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

        public double Zee4W6079()
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

        public double Zee5()
        {
            if (_model.NutritionQuality <= 7)
                return (double)_model.Age * 0.33 * (-0.06);
            else if (_model.NutritionQuality <= 14)
                return (double)_model.Age * 0.33 * (-0.03);
            else if (_model.NutritionQuality <= 21)
                return (double)_model.Age * 0.33 * 0.03;
            else
                return (double)_model.Age * 0.33 * 0.06;

        }

        public double Zee9()
        {
            if (_model.Smoking == HealthAgeCalculatorInputModel.SmokerType.Never)
                return (double)_model.Age * 0.33 * (-0.17);
            else if (_model.Smoking == HealthAgeCalculatorInputModel.SmokerType.FormerLight)
                return (double)_model.Age * 0.33 * 0.03;
            else if (_model.Smoking == HealthAgeCalculatorInputModel.SmokerType.FormerHeavy)
                return (double)_model.Age * 0.33 * 0.06;
            else
                return (double)_model.Age * 0.33 * 0.17;
        }

        public double Zee10()
        {
            if (_model.HemoglobinA1c <= 5.2)
                return (double)_model.Age * 0.33 * (-0.17);
            else if (_model.HemoglobinA1c <= 6.4)
                return (double)_model.Age * 0.33 * 0.1;
            else
                return (double)_model.Age * 0.33 * 0.17;
        }

    }
}

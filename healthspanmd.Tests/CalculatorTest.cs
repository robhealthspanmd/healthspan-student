using Xunit;
using System;
using healthspanmd.core.Services.HealthAgeCalculator_v3;
namespace healthspanmd.Tests
{
    public class CalculatorTest : IDisposable
    {
        readonly HealthAgeCalculator testCase;
        HealthAgeCalculatorResult result;

        public CalculatorTest() 
        {
            testCase = new HealthAgeCalculator();
            result = new HealthAgeCalculatorResult();
            testCase._patient = new Patient();
            testCase._patient.age = 23;
            testCase._patient.moderateActivity = "less-than-5";
            testCase._patient.vigorousActivity = "none";
            testCase._patient.stress = "a-lot";
            testCase._patient.strengthTraining = "less-than-30";
            testCase._patient.sugarAddedDrinks = "2";
            testCase._patient.addedSugar = "1";
            testCase._patient.refinedGrains = "1";
            testCase._patient.deepFriedFoods = "3";
            testCase._patient.processedFoods = "1";
            testCase._patient.AlcoholicDrinks = "1";
            testCase._patient.BMI = "too-thin";
            testCase._patient.sleep = "less-than-7";
            testCase._patient.cholesterol = "great";
            testCase._patient.bloodPressure = "great";
            testCase._patient.smoker = "currently-smoking";
            testCase._patient.diabetes = "no-diabetes";
            testCase._patient.lonely = "rarely";
            result.Success = true;
            result.HealthAge = 25.184999999999999;
            result.Input = testCase._patient;
        }
        public void Dispose()
        {
            
        }
        [Fact]
        public void CalculatorResultTest()
        {
            HealthAgeCalculatorResult expected = result;
            HealthAgeCalculatorResult actual = testCase.CalculateResult(testCase._patient);
            Assert.Equal(expected.HealthAge, actual.HealthAge);
            Assert.Equal(expected.Success, actual.Success);
            Assert.Equal(expected.Input, actual.Input);
            Assert.Equal(expected.Message, actual.Message);
        }

        [Fact]
        public void PhysicalActivityTest()
        {
            double expected = 0.46;
            double Actual = testCase.Z1_PhysicalActivity();
            Assert.Equal(expected, Actual);
        }

        [Fact]
        public void StressTest()
        {
            double expected = 0.69;
            double actual = testCase.Z11_Stress();
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void StrengthTrainingTest() 
        {
            double expected = 0.92;
            double actual = testCase.Z2_StrengthTraining();
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void RedFoodsTest()
        {
            testCase._patient.age = 22;
            double expected = 2.42;
            double actual = Math.Round(testCase.Z3_RedFoods(), 2);
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void BMITest()
        {
            double expected = 0.46;
            double actual = testCase.Z4_BMI();
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void SleepTest() 
        {
            testCase._patient.age = 24;
            double expected = 0.72;
            double actual = testCase.Z5_Sleep();
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void CholesteralTest()
        {
            double expected = -0.92;
            double actual = testCase.Z6_Cholesterol();
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void BloodPressureTest()
        {
            testCase._patient.age = 25;
            double expected = -1.25;
            double actual = testCase.Z7_BloodPressure();
            Assert.Equal(expected, actual); 
        }
        [Fact]
        public void SmokingTest()
        {
            double expected = 2.76;
            double actual = testCase.Z8_Smoking();
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void DiabetesTest()
        {
            double expected = -1.38;
            double actual = testCase.Z9_Diabetes();
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void LonelyTest()
        {
            double expected = -0.69;
            double actual = testCase.Z10_Lonely();
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void RedFoodServingsTest()
        {
            string test = "4";
            int expected = 4;
            int actual = testCase.redFoodServings(test);
            Assert.Equal(expected, actual);
        }
    }
}
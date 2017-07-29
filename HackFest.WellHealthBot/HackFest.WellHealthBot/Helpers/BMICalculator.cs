using System;
using HackFest.WellHealthBot.Models.BMI;

namespace HackFest.WellHealthBot.Helpers
{
    public static class BMICalculator
    {
        public static BMIResponse CalulateBMI(BMI request)
        {
            var result = new BMIResponse();
            double height;
            if (request.weightUnit == WeightUnit.LBS)
                request.weight = request.weight.LbsToKg();

            if (request.heightUnit == HeightUnit.Feet)
                height = request.height.FeetToCentimeter();
            else
                height = Convert.ToDouble(request.height);


            result.BMI = request.weight / Math.Pow(height / 100.0, 2);

            if (result.BMI < 19 & request.gender == Gender.Female)
            { result.HealthStatus = "Underweight"; }
            if (result.BMI >= 19 & result.BMI <= 24 & request.gender == Gender.Female)
            { result.HealthStatus = "Normal"; }
            if (result.BMI > 24 & request.gender == Gender.Female)
            { result.HealthStatus = "Overweight"; }

            if (result.BMI < 20 & request.gender == Gender.Male)
            { result.HealthStatus = "Underweight"; }
            if (result.BMI >= 20 & result.BMI <= 25 & request.gender == Gender.Male)
            { result.HealthStatus = "Normal"; }
            if (result.BMI > 25 & request.gender == Gender.Male)
            { result.HealthStatus = "Overweight"; }
            return result;
        }
    }
}
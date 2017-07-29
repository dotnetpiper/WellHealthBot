using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HackFest.WellHealthBot.Models.BMI;

namespace HackFest.WellHealthBot.Helpers
{
    public static class BMICalculator
    {
        public static BMIResponse CalulateBMI(double weight, double height, int age, Gender gender)
        {
            var result = new BMIResponse();
            result.BMI = weight / Math.Pow(height / 100.0, 2);

            if (result.BMI < 19 & gender == Gender.Female)
            { result.HealthStatus = "Underweight"; }
            if (result.BMI >= 19 & result.BMI <= 24 & gender == Gender.Female)
            { result.HealthStatus = "Normal"; }
            if (result.BMI > 24 & gender == Gender.Female)
            { result.HealthStatus = "Overweight"; }

            if (result.BMI < 20 & gender == Gender.Male)
            { result.HealthStatus = "Underweight"; }
            if (result.BMI >= 20 & result.BMI <= 25 & gender == Gender.Male)
            { result.HealthStatus = "Normal"; }
            if (result.BMI > 25 & gender == Gender.Male)
            { result.HealthStatus = "Overweight"; }
            return result;
        }
    }
}
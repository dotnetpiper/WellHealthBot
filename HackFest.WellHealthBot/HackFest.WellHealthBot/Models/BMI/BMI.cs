using System;
using System.Text.RegularExpressions;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.FormFlow.Advanced;

namespace HackFest.WellHealthBot.Models.BMI
{
    [Serializable]
    public class BMI
    {
        [Template(TemplateUsage.EnumSelectOne,"Please choose your preferred weight unit {||}","What is your preferred unit for weight {||}")]
        public WeightUnit weightUnit;
        [Prompt("Please enter your weight in {weightUnit}")]
        public double weight;
        [Template(TemplateUsage.EnumSelectOne, "Please choose your preferred height unit {||}", "What is your preferred unit for height {||}")]
        public HeightUnit heightUnit;
        [Prompt("Please enter your height in {heightUnit}")]
        [Pattern("^([0-9]+\'([ ]?[0-9]{1,2}[\"]?|))|([\\d]{1,3}([.,][\\d]{1,2})?)$")]
        public string height;
        //[Optional]
        //[Prompt("Please enter your height in {heightUnit}")]       
        //public string heightInFeet;
        //[Optional]
        //[Prompt("Please enter your height in {heightUnit}:")]
        //public double? heightInCentimeter;
        [Template(TemplateUsage.EnumSelectOne,"Please select your gender {||}","What is your gender {||}")]
        public Gender gender;
        [Prompt("Please enter your age")]
        public int age;

        public static IForm<BMI> BuildForm()
        {
            return new FormBuilder<BMI>()
                .Field("weightUnit")
                .Field("weight")
                .Field("heightUnit")
                .Field(nameof(height))
                /*, 
                validate: async (state, value) =>
                {
                    Regex r = new Regex("^[0-9]+\'([ ]?[0-9]{1,2}[\"]?|)$");
                    var values = (value as string).Trim();
                    var result = new ValidateResult { IsValid = true, Value = values };
                    var match = r.Match(values);
                    if (match.Success)
                    {
                        return result;
                    }
                    else
                    {
                        result.IsValid = false;
                        result.FeedbackCard = new FormPrompt()
                        {
                            Prompt = "Invalid Height",
                            Description = new DescribeAttribute("Height should be in format like: 5'7\"")
                        };
                        return result;
                    }
                })*/
                
                //.Field(nameof(heightInFeet), (h)=>h.heightUnit == HeightUnit.Feet)
                //.Field(nameof(heightInCentimeter), IsHeightUnitIsCentimeter)
                .Field("gender")
                .Field("age")
                .AddRemainingFields()
                //.Confirm("Is the following details are correct? I am going to submit them.\r\r Weight: {weight}{weightUnit}\r\rHeight: {height}{heightUnit}\r\rGender: {gender}\r\rAge: {age}")
                .Confirm("Is the following details are correct? I am going to submit them.\r\r Weight: {weight}\r\rHeight: {height}\r\rGender: {gender}\r\rAge: {age}")
            .Build();
        }

        //private static bool IsHeightUnitIsCentimeter(BMI bmi)
        //{
        //    return bmi.heightUnit == HeightUnit.Centimeter;
        //}
    }
}
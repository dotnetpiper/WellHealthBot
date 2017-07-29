using System;
using HackFest.WellHealthBot.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;

namespace HackFest.WellHealthBot.Models.BMI
{
    [Serializable]
    public class BMI
    {
        public WeightUnit? weightUnit;
        [Prompt("Please enter your weight")]
        public double weight;
        public HeightUnit? heightUnit;
        [Prompt("Please enter your height")]
        public double height;        
        public Gender? gender;
        [Prompt("Please enter your Age")]
        public int age;

        public static IForm<BMI> BuildForm()
        {
            return new FormBuilder<BMI>()
                .Field("weightUnit")
                .Field("weight")
                .Field("heightUnit")
                .Field("height")
                .Field("gender")
                .Field("age")
                .Confirm("Is the following details are correct? I am going to submit them.\r\r Weight: {weight}\r\rHeight: {height}\r\rGender: {gender}\r\rAge: {age}")
                .OnCompletion(async (context, bmi) =>
                {
                    context.PrivateConversationData.SetValue<WeightUnit>("weightUnit", bmi.weightUnit.Value);
                    context.PrivateConversationData.SetValue<double>("weight", bmi.weight);
                    context.PrivateConversationData.SetValue<HeightUnit>("heightUnit", bmi.heightUnit.Value);
                    context.PrivateConversationData.SetValue<double>("height", bmi.height);
                    context.PrivateConversationData.SetValue<Gender>("gender", bmi.gender.Value);
                    context.PrivateConversationData.SetValue<int>("age", bmi.age);
                    var result = BMICalculator.CalulateBMI(bmi.weight, bmi.height, bmi.age, bmi.gender.Value);
                    // Tell the user that the form is complete
                    await context.SayAsync(text: "Your BMI is " + result.BMI.ToString("00.00") + " and you are " + result.HealthStatus);
                })            
            .Build();
        }
    }
}
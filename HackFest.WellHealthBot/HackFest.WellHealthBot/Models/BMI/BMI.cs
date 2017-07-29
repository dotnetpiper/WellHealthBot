using System;
using HackFest.WellHealthBot.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;

namespace HackFest.WellHealthBot.Models.BMI
{
    [Serializable]
    public class BMI
    {
        public WeightUnit weightUnit;
        [Prompt("Please enter your weight")]
        public double weight;
        public HeightUnit heightUnit;
        [Prompt("Please enter your height")]
        public double height;        
        public Gender gender;
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
            .Build();
        }
    }
}
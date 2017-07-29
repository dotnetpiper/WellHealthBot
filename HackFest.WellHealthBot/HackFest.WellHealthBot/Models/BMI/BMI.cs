using System;
using Microsoft.Bot.Builder.FormFlow;

namespace HackFest.WellHealthBot.Models.BMI
{
    [Serializable]
    public class BMI
    {
        [Prompt ("Please choose your preferred Weight Unit {||}")]
        public WeightUnit weightUnit;
        [Prompt("Please enter your Weight in {weightUnit}")]
        public double weight;
        [Prompt("Please choose your preferred Height Unit {||}")]
        public HeightUnit heightUnit;
        [Prompt("Please enter your Height in {heightUnit}")]
        public string height;
        [Prompt("Please select your Gender {||}")]
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
                //.Confirm("Is the following details are correct? I am going to submit them.\r\r Weight: {weight}{weightUnit}\r\rHeight: {height}{heightUnit}\r\rGender: {gender}\r\rAge: {age}")
                //.Confirm("Is the following details are correct? I am going to submit them.\r\r Weight: {weight}\r\rHeight: {height}\r\rGender: {gender}\r\rAge: {age}")
            .Build();
        }
    }
}
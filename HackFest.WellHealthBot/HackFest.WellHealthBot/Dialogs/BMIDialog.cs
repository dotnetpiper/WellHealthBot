using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HackFest.WellHealthBot.Helpers;
using HackFest.WellHealthBot.Models.BMI;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;

namespace HackFest.WellHealthBot.Dialogs
{
    [Serializable]
    public class BMIDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("");

            var bmiFormDialog = FormDialog.FromForm(BuildBmiForm, FormOptions.PromptInStart);

            context.Call(bmiFormDialog, Callback);
        }

        private IForm<BMI> BuildBmiForm()
        {
            OnCompletionAsyncDelegate<BMI> processBmiSearch = async (context, state) => { };

            return new FormBuilder<BMI>()
                .AddRemainingFields()
                .OnCompletion(processBmiSearch)
                .Build();
        }

        private async Task Callback(IDialogContext context, IAwaitable<BMI> result)
        {
            try
            {
                var actualResult = await result;
                var obj = BMICalculator.CalulateBMI(actualResult);
                // Tell the user that the form is complete
                await context.PostAsync("Your BMI is " + obj.BMI.ToString("00.00") + " and you are " + obj.HealthStatus);
                context.PrivateConversationData.SetValue<string>("HealthStatus", obj.HealthStatus);
                CallChoices(context);
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Failed with message: {ex.Message}");
            }
        }

        private void CallChoices(IDialogContext context)
        {
            PromptDialog.Choice(context, OnOptionSelected,
                new List<string> {"Health Chart", "Suggest Doctor", "Physical Activity"},
                "To maintain your weight, make sure your eat a healthy,balanced diet, and that you exercise regualarly.");
        }


        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                var optionSelected = await result;
                var message = context.MakeMessage();
                string healthStatus = context.PrivateConversationData.GetValue<string>("HealthStatus");
                Attachment attachment;
                switch (optionSelected)
                {
                    case "Health Chart":
                        attachment = GetHealthyChart(optionSelected, healthStatus);
                        message.Attachments.Add(attachment);
                        await context.PostAsync(message);
                        CallChoices(context);
                        break;
                    case "Suggest Doctor":
                        context.Call(new DoctorDialog(), ResumeAfterOptionDialog);
                        break;
                    case "Physical Activity":
                         attachment = GetPhysicalActivity(optionSelected, healthStatus);
                        message.Attachments.Add(attachment);
                        await context.PostAsync(message);
                        CallChoices(context);
                        break;
                }
            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync(
                    $"Ooops! Too many attemps :(. But don't worry, I'm handling that exception and you can try again!");

                context.Wait(ResumeAfterOptionDialog);
            }
        }
        private Attachment GetHealthyChart(string optionSelected,string healthStatus)
        {
            ThumbnailCard healthyHeroCard;

            if (healthStatus== "Overweight")
            {
                 healthyHeroCard = new ThumbnailCard
                {
                    Title = "Health Chart-Obesity",
                    Subtitle = "Fitness for your mind,body and spirit,Stay Healthy and Fit!!",
                    Text = "The term 'obese' describes a person who's very overweight, with a lot of body fat.",
                    Images = new List<CardImage> { new CardImage("https://pbs.twimg.com/profile_images/586208484305330176/3p08A5LG.jpg") },
                    Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "NHS Choices", value: "http://www.nhs.uk/livewell/loseweight/Pages/Loseweighthome.aspx") }
                };
            }
           else if (healthStatus == "Underweight")
            {
                 healthyHeroCard = new ThumbnailCard
                 {
                    Title = "Health Chart for - Malnutrition",
                    Subtitle = "Fitness for your mind,body and spirit,Stay Healthy and Fit!!",
                    Text = "Malnutrition is a serious condition that occurs when a person’s diet doesn't contain the right amount of nutrients.",
                    Images = new List<CardImage> { new CardImage("https://image.slidesharecdn.com/8-141103081706-conversion-gate01/95/malnutrition-the-public-health-issue-overshadowed-by-obesity-joanne-casey-5-638.jpg?cb=1415003185") },
                    Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "NHS Choices", value: "http://www.nhs.uk/Conditions/Malnutrition/Pages/Introduction.aspx") }
                };
            }
            else
            {
                 healthyHeroCard = new ThumbnailCard
                 {
                    Title = "Health Chart- Keep Excersizing",
                    Subtitle = "Fitness for your mind,body and spirit,Stay Healthy and Fit!!",
                    Text = "Good nutrition diet is the sum of food consumed by a person or other organism.The word diet often implies the use of specific intake of nutrition for health or weight-management reasons. Although humans are omnivores, each culture and each person holds some food preferences or some food taboos. This may be due to personal tastes or ethical reasons. Individual dietary choices may be more or less healthy.Complete nutrition requires ingestion and absorption of vitamins,minerals,and food energy in the form of carbohydrates,proteins,and fats.Dietary habits and choices play a significant role in the quality of life,health and longevity.",
                    Images = new List<CardImage> { new CardImage("http://www.nhs.uk/Livewell/Goodfood/PublishingImages/A_0617_balanced-diet_cey7ek_A.jpg") },
                    Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "NHS Choices", value: "http://www.nhs.uk/Livewell/Goodfood/Pages/Healthyeating.aspx") }
                };
            }


            return healthyHeroCard.ToAttachment();
        }

        private Attachment GetPhysicalActivity(string optionSelected, string healthStatus)
        {
            ThumbnailCard physicalActivityCard;

            if (healthStatus == "Overweight")
            {
                physicalActivityCard = new ThumbnailCard
                {
                    Title = "Physical Activity - Obesity",
                    Subtitle = "Fitness for your mind,body and spirit,Stay Healthy and Fit!!",
                    Text = "The term 'obese' describes a person who's very overweight, with a lot of body fat.",
                    Images = new List<CardImage> { new CardImage("https://media.healthdirect.org.au/images/banners/w760h217/standing-on-scales_h.jpg") },
                    Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "LossWeight Activity", value: "http://www.nhs.uk/livewell/loseweight/Pages/Loseweighthome.aspx") }
                };
            }
            else if (healthStatus == "Underweight")
            {
                physicalActivityCard = new ThumbnailCard
                {
                    Title = "Physical Activity - Malnutrition",
                    Subtitle = "Fitness for your mind,body and spirit,Stay Healthy and Fit!!",
                    Text = "Malnutrition is a serious condition that occurs when a person’s diet doesn't contain the right amount of nutrients.",
                    Images = new List<CardImage> { new CardImage("https://ntyen2013.files.wordpress.com/2013/06/worldhealthday.jpg") },
                    Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "Behealthy Activity", value: "http://www.nutraingredients.com/Research/Supplements-and-exercise-effective-for-malnutrition-and-frailty-Review") }
                };
            }
            else
            {
                physicalActivityCard = new ThumbnailCard
                {
                    Title = "Physical Activity- Keep Excersizing",
                    Subtitle = "Fitness for your mind,body and spirit,Stay Healthy and Fit!!",
                    Text = "Good nutrition diet is the sum of food consumed by a person or other organism.The word diet often implies the use of specific intake of nutrition for health or weight-management reasons. Although humans are omnivores, each culture and each person holds some food preferences or some food taboos. This may be due to personal tastes or ethical reasons. Individual dietary choices may be more or less healthy.Complete nutrition requires ingestion and absorption of vitamins,minerals,and food energy in the form of carbohydrates,proteins,and fats.Dietary habits and choices play a significant role in the quality of life,health and longevity.",
                    Images = new List<CardImage> { new CardImage("https://health.gov/images/pag_02.jpg") },
                    Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "To be fit ", value: "https://health.gov/images/pag_02.jpg") },
                    
                };
            }


            return physicalActivityCard.ToAttachment();
        }

        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var message = await result;
                CallChoices(context);
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Failed with message: {ex.Message}");
            }
            finally
            {
                context.Wait(ResumeAfterOptionDialog);
            }
        }
    }
}
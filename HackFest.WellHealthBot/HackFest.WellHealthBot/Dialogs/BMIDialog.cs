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
                var obj = BMICalculator.CalulateBMI(actualResult.weight, actualResult.height, actualResult.age,
                    actualResult.gender);
                // Tell the user that the form is complete
                await context.PostAsync("Your BMI is " + obj.BMI.ToString("00.00") + " and you are " + obj.HealthStatus);
                PromptDialog.Choice(context, OnOptionSelected,
                    new List<string> {"Health Chart", "Suggest Doctor", "Physical Activity"},
                    "To maintain your weight, make sure your eat a healthy,balanced diet, and that you exercise regualarly.");
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Failed with message: {ex.Message}");
            }
        }


        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                var optionSelected = await result;

                switch (optionSelected)
                {
                    case "Health Chart":
                        var message = context.MakeMessage();
                        var attachment = GetSelectedCard(optionSelected);
                        message.Attachments.Add(attachment);
                        await context.PostAsync(message);;
                        break;
                    case "Suggest Doctor":
                        context.Call(new DoctorDialog(), ResumeAfterOptionDialog);
                        break;
                    case "Physical Activity":
                        context.Done<object>(null);
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
        private Attachment GetSelectedCard(string optionSelected)
        {
            var healthyHeroCard = new HeroCard
            {
                Title = "Health Chart",
                Subtitle = "Fitness for your mind,body and spirit,Stay Healthy and Fit!!",
                Text = "n nutrition, diet is the sum of food consumed by a person or other organism.The word diet often implies the use of specific intake of nutrition for health or weight-management reasons. Although humans are omnivores, each culture and each person holds some food preferences or some food taboos. This may be due to personal tastes or ethical reasons. Individual dietary choices may be more or less healthy.Complete nutrition requires ingestion and absorption of vitamins,minerals,and food energy in the form of carbohydrates,proteins,and fats.Dietary habits and choices play a significant role in the quality of life,health and longevity.",
                Images = new List<CardImage> { new CardImage("http://img.freepik.com/free-vector/farming-harvesting-and-agriculture-badges-or-labels-set-on-white-background-isolated-vector-illustration_1284-2137.jpg?size=338&ext=jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "NHS Choices", value: "http://www.nhs.uk/pages/home.aspx") }
            };

            return healthyHeroCard.ToAttachment();
        }

        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var message = await result;
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
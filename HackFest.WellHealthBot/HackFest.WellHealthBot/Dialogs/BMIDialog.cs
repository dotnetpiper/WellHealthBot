using System;
using System.Threading.Tasks;
using HackFest.WellHealthBot.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using HackFest.WellHealthBot.Models;
using Microsoft.Bot.Builder.FormFlow;
using HackFest.WellHealthBot.Models.BMI;

namespace HackFest.WellHealthBot.Dialogs
{
    [Serializable]
    public class BMIDialog : IDialog<object>
    {      

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("");

            var bmiFormDialog = FormDialog.FromForm(this.BuildBmiForm, FormOptions.PromptInStart);

            context.Call(bmiFormDialog, this.Callback);
        }

        private IForm<BMI> BuildBmiForm()
        {
            OnCompletionAsyncDelegate<BMI> processBmiSearch = async (context, state) =>
            {
            };

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
                var obj = BMICalculator.CalulateBMI(actualResult.weight, actualResult.height, actualResult.age, actualResult.gender);
                // Tell the user that the form is complete
                await context.PostAsync(text: "Your BMI is " + obj.BMI.ToString("00.00") + " and you are " + obj.HealthStatus);
            }
            finally
            {
                context.Done<object>(null);
            }
        }
    }
}
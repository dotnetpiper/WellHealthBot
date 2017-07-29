using System;
using System.Threading.Tasks;
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
            OnCompletionAsyncDelegate<BMI> processHotelsSearch = async (context, state) =>
            {
            };

            return new FormBuilder<BMI>()
            .AddRemainingFields()
            .OnCompletion(processHotelsSearch)
            .Build();
        }

        private async Task Callback(IDialogContext context, IAwaitable<object> result)
        {
            //context.Wait(MessageReceived);
        }
    }
}
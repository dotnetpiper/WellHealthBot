using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace HackFest.WellHealthBot.Dialogs
{
    [LuisModel("4af37619-41ec-4d5a-b512-870240b4d92b", "cbc8c5b2ae5445ccb2415e41348acaf9")]
    [Serializable]
    public class LuisAIDialog : LuisDialog<object>
    {
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Sorry,I dont know much what you said.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Greet.Welcome")]
        private async Task Welcome(IDialogContext context, LuisResult result)
        {
            context.Call(new GreetingDialog(), Callback);
        }

        private async Task Callback(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceived);

            //await Conversation.SendAsync(activity, () => new Dialogs.BMIDialog(BMI.BuildForm));
        }
    }
}
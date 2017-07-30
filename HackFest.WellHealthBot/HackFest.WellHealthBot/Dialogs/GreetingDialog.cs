using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HackFest.WellHealthBot.Models.BMI;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace HackFest.WellHealthBot.Dialogs
{
    [Serializable]
    public class GreetingDialog : IDialog
    {
        public async Task StartAsync(IDialogContext context)
        {
            //await context.PostAsync("..");
            await Respond(context);            
        }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string optionSelected = await result;

                switch (optionSelected)
                {
                    case "BMI Calculator":                    
                        context.Call(new Dialogs.BMIDialog(), ResumeAfterOptionDialog1Async);
                        break;
                    case "Suggest Doctor":
                        context.Call(new Dialogs.DoctorDialog(), ResumeAfterOptionDialog1Async);
                        break;
                    case "Symptoms":
                        await context.PostAsync("First, let me know your symptoms or the condition you want to learn more about. Shorter descriptions are usually a good start. For example: <i>I have a sore throat.</i>");
                        context.Done<object>(null);
                        break;
                    case "Help":
                        await context.PostAsync(
                            "Please leave a message and we will respond as soon as possible. Or, say <i>Hi</i> again to return to the main menu.");
                            context.Done<object>(null);
                        break;
                    case "No":
                        await context.PostAsync(
                            "Tell me how you’re feeling or your symptoms, and I will try to find relevant medical information to help you understand why you’re feeling this way.");
                        await context.PostAsync("I can also instantly search for a doctor to you,Kindly type<b>doctor</b>");
                        context.Done<object>(null);
                        break;
                }
            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync($"Ooops! Too many attemps :(. But don't worry, I'm handling that exception and you can try again!");

                context.Wait(this.ResumeAfterOptionDialog);
            }
        }

        private async Task ResumeAfterOptionDialog1Async(IDialogContext context, IAwaitable<object> result)
        {
            var a = await result;
            context.Done<object>(null);
        }

        private async Task Respond(IDialogContext context)
        {
            var userName = String.Empty;
            context.UserData.TryGetValue<string>("Name", out userName);
            if (string.IsNullOrEmpty(userName))
            {
                context.UserData.SetValue<bool>("GetName", true);
                PromptDialog.Text(context,this.MessageReceivedAsync, "May I know your name?");
            }
            else
            {
                await context.PostAsync($" Welcome {userName}.  Would you like to choose any of the below services?");
                PromptDialog.Choice(context, OnOptionSelected, new List<string> { "BMI Calculator", "Suggest Doctor", "Symptoms", "Help" }, "I can help you understand the symptoms you're experiencing. I'm quite chatty, but keep in mind that I am not a certified medical doctor");
            }
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
                await Respond(context);
            }
        }



        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<string> result)
        {
            var message = await result;
            var userName = String.Empty;
            var getName = false;
            context.UserData.TryGetValue<string>("Name", out userName);
            context.UserData.TryGetValue<bool>("GetName", out getName);

            if (getName)
            {
                userName = message;
                context.UserData.SetValue<string>("Name", userName);
                context.UserData.SetValue<bool>("GetName", false);
            }
            //context.
            await Respond(context);
            //context.Done(message);
        }
    }
}
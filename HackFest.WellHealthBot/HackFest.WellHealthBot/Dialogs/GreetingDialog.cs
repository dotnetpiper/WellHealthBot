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
                    case "Yes":                    
                        context.Call(new Dialogs.BMIDialog(), ResumeAfterOptionDialog1Async);
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
                var attachmentMsg = context.MakeMessage();
                attachmentMsg.Text = "Hi, I'm Well-Health Bot!";
                attachmentMsg.Attachments.Add(new Attachment() { ContentUrl = "https://logo.clearbit.com/https:/www.wellhealthqc.com/", ContentType = "image/png", Name = "wellhealth.jpeg" });
                //attachmentMsg.Attachments.Add(new Attachment() { ContentUrl = "https://logo.clearbit.com/https:/www.healthwellfoundation.org/", ContentType = "image/png", Name = "wellhealth.jpeg" });
                attachmentMsg.AttachmentLayout = AttachmentLayoutTypes.List;
                await context.PostAsync(attachmentMsg);
                PromptDialog.Text(context,this.MessageReceivedAsync, "May I know your name?");
            }
            else
            {
                await context.PostAsync($" Welcome {userName}.  Would you like to check your BMI?");
                PromptDialog.Choice(context, OnOptionSelected, new List<string> { "Yes", "No" }, "Body mass index (BMI) is a measure of body fat based on height and weight that applies to adult men and women.");
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
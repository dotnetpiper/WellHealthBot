using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace HackFest.WellHealthBot.Dialogs
{
    [Serializable]
    public class GreetingDialog : IDialog
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Hi I'm 3Pillar Bot System..");
            await Respond(context);
        }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                var optionSelected = await result;

                switch (optionSelected)
                {
                    case "Yes":
                        context.Call(new BMIDialog(), ResumeAfterOptionDialog);
                        break;

                    case "No":
                        await context.PostAsync("Thanks for using WellHealth Bot.");
                        context.Done<object>(null);
                        break;
                }
            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync(
                    $"Ooops! Too many attemps :(. But don't worry, I'm handling that exception and you can try again!");

                context.Wait(MessageReceivedAsync);
            }
        }

        private async Task Respond(IDialogContext context)
        {
            var userName = string.Empty;
            context.UserData.TryGetValue("Name", out userName);
            if (string.IsNullOrEmpty(userName))
            {
                context.UserData.SetValue("GetName", true);
                var attachmentMsg = context.MakeMessage();
                attachmentMsg.Text = "3PillatBot";
                attachmentMsg.Attachments.Add(new Attachment
                {
                    ContentUrl = "https://logo.clearbit.com/www.3pillarglobal.com",
                    ContentType = "image/png",
                    Name = "3Pillar.jpeg"
                });
                attachmentMsg.AttachmentLayout = AttachmentLayoutTypes.List;
                await context.PostAsync(attachmentMsg);
                await context.PostAsync("May i know your name?");

            }
            else
            {
                await context.PostAsync(
                    $"Hi {userName}.  Would you like to check your BMI?{Environment.NewLine}Body mass index (BMI) is a measure of body fat based on height and weight that applies to adult men and women");
                PromptDialog.Choice(context, OnOptionSelected, new List<string> {"Yes", "No"}, " ");
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
                context.Wait(MessageReceivedAsync);
            }
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            var userName = string.Empty;
            var getName = false;
            context.UserData.TryGetValue("Name", out userName);
            context.UserData.TryGetValue("GetName", out getName);

            if (getName)
            {
                userName = message.Text;
                context.UserData.SetValue("Name", userName);
                context.UserData.SetValue("GetName", false);
            }
            //context.
            await Respond(context);
            //context.Done(message);
            context.Wait(ResumeAfterOptionDialog);
        }
    }
}
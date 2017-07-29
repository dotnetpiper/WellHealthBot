using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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

            context.Wait(MessageReceivedAsync);
        }

        private static  async Task Respond(IDialogContext context)
        {
            var userName = String.Empty;
            context.UserData.TryGetValue<string>("Name", out userName);
            if (string.IsNullOrEmpty(userName))
            {

                context.UserData.SetValue<bool>("GetName", true);
                var attachmentMsg = context.MakeMessage();
                attachmentMsg.Text = "3PillatBot";
                attachmentMsg.Attachments.Add(new Attachment() { ContentUrl = "https://logo.clearbit.com/www.3pillarglobal.com", ContentType = "image/png", Name = "3Pillar.jpeg" });
                attachmentMsg.AttachmentLayout = AttachmentLayoutTypes.List;
                await context.PostAsync(attachmentMsg);
                await context.PostAsync("May i know your name?");

            }
            else
            {
                await context.PostAsync(
                    $"Hi {userName}.  Would you like to check your BMI?{Environment.NewLine}Body mass index (BMI) is a measure of body fat based on height and weight that applies to adult men and women");
            }
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            var userName = String.Empty;
            var getName = false;
            context.UserData.TryGetValue<string>("Name", out userName);
            context.UserData.TryGetValue<bool>("GetName", out getName);

            if (getName)
            {
                userName = message.Text;
                context.UserData.SetValue<string>("Name", userName);
                context.UserData.SetValue<bool>("GetName", false);
            }
            //context.
            await Respond(context);
            context.Done(message);
        }
    }
}
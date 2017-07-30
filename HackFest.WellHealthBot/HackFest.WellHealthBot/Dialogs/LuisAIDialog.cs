using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace HackFest.WellHealthBot.Dialogs
{
    [LuisModel("4af37619-41ec-4d5a-b512-870240b4d92b", "cbc8c5b2ae5445ccb2415e41348acaf9")]
    [Serializable]
    public class LuisAIDialog : LuisDialog<object>
    {
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Sorry, I dont know much what you said.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Greet.Welcome")]
        private Task Welcome(IDialogContext context, LuisResult result)
        {
            context.Call(new GreetingDialog(), Callback);
            return Task.CompletedTask;
        }

        private async Task Callback(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceived);

            //await Conversation.SendAsync(activity, () => new Dialogs.BMIDialog(BMI.BuildForm));
        }

        [LuisIntent("Greet.Farewell")]
        public async Task GreetFarewell(IDialogContext context, LuisResult luisResult)
        {
            string response = string.Empty;
            var userName = context.UserData.Get<string>("Name");
            if (DateTime.Now.ToString("tt") == "AM")
            {
                response = $"Good bye {userName}.. Have a nice day. :)\r\rThank you for using WellHealth Bot!";
            }
            else
            {
                response = $"b'bye {userName}, Take care.\r\rThank you for using WellHealth Bot!";
            }
            await context.PostAsync(response);
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Search.Doctor")]
        public async Task SearchDoctor(IDialogContext context, LuisResult luisResult)
        {
            EntityRecommendation specialization;
            string name = string.Empty;
            if (luisResult.TryFindEntity("Medical.Specialist", out specialization))
            {
                name = specialization.Entity;
            }
            context.Call(new DoctorDialog(), this.Callback);
        }

        [LuisIntent("Check.Symptom")]
        public async Task CheckSymptom(IDialogContext context, LuisResult luisResult)
        {
            EntityRecommendation symptom;
            string disease = string.Empty;
            if(luisResult.TryFindEntity("General.Diseases", out symptom))
            {
                disease = symptom.Entity;
            }
            var message = context.MakeMessage();
            Attachment attachment = GetRemeady(disease.ToLower());
            message.Attachments.Add(attachment);
            await context.PostAsync(message);
        }

        private Attachment GetRemeady(string disease)
        {
            ThumbnailCard remeadyHeroCard;

            if (new List<string>(){"throat","soar","infection"}.Contains(disease))
            {
                remeadyHeroCard = new ThumbnailCard
                {
                    Title = "Remeady for " + disease,
                    Subtitle = "12 Natural Remedies for Sore Throat",
                    Text = "A sore throat is pain, scratchiness or irritation of the throat that often worsens when you swallow.",
                    Images = new List<CardImage> { new CardImage("http://www.clevelandsurgery.nhs.uk/sites/default/files/sorethroat.jpg") },
                    Buttons = new List<CardAction>
                    {
                        new CardAction(ActionTypes.OpenUrl, "Check Remeady", value: "http://www.healthline.com/health/cold-flu/sore-throat-natural-remedies?algo=f#overview1"),
                    },

                };
            }
            else if (new List<string>(){"fever", "feverish","viral" }.Contains(disease))
            {
                remeadyHeroCard = new ThumbnailCard
                {
                    Title = "Remeady for " + disease,
                    Subtitle = "Home Remedies for Fever",
                    Text = "Whenever the body’s temperature is higher than the normal range, it is called a fever. Although we commonly hear that 98.6 degrees F, or 37 degrees C, is considered normal, this is not a set number that applies universally to all. Normal body temperature is different for children than adults and also can vary among individuals.",
                    Images = new List<CardImage> { new CardImage("http://img2.timeinc.net/health/images/slides/high-fever-bed-400x400.jpg") },
                    Buttons = new List<CardAction>
                    {
                        new CardAction(ActionTypes.OpenUrl, "Check Remeady", value: "http://www.top10homeremedies.com/home-remedies/home-remedies-fever.html"),
                    }
                };
            }
            else
            {
                remeadyHeroCard = new ThumbnailCard
                {
                    Title = "symptom-checker",
                    Subtitle = "Experiencing symptoms but not sure what they mean?",
                    Text = "Use our Symptom Checker to help determine possible causes and treatments, and when to see a doctor.",
                    Images = new List<CardImage> { new CardImage("https://refluxmd.files.wordpress.com/2013/04/home-remedies.jpg") },
                    Buttons = new List<CardAction>
                    {
                        new CardAction(ActionTypes.OpenUrl, "Check Remeady", value: "http://www.healthline.com/symptom-checker"),
                    }
                };
            }

            return remeadyHeroCard.ToAttachment();
        }
    }
}
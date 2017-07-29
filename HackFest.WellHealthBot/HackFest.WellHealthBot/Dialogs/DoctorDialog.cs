using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HackFest.WellHealthBot.Models.Doctor;
using HackFest.WellHealthBot.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;

namespace HackFest.WellHealthBot.Dialogs
{
    [Serializable]
    public class DoctorDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            var searchFormDialog = FormDialog.FromForm(BuildSearchForm, FormOptions.PromptInStart);

            context.Call(searchFormDialog, ResumeAfterDoctorFormDialog);
        }

        private IForm<SearchDoctorQuery> BuildSearchForm()
        {
            OnCompletionAsyncDelegate<SearchDoctorQuery> processSearch =
                async (context, state) =>
                {
                    await context.PostAsync(
                        $"Ok. Searching for Doctor in {state.Location} Specialist in {state.Specialist}....");
                };

            return new FormBuilder<SearchDoctorQuery>()
                .Message("Welcome to the Doctor finder!")
                .OnCompletion(processSearch)
                .Build();
        }

        private async Task ResumeAfterDoctorFormDialog(IDialogContext context, IAwaitable<SearchDoctorQuery> result)
        {
            try
            {
                var searchQuery = await result;

                var doctors = await DoctorService.FindDoctorsAsync(searchQuery);

                await context.PostAsync($"I found in total {doctors.Count()} doctors near your location:");

                var resultMessage = context.MakeMessage();
                resultMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                resultMessage.Attachments = new List<Attachment>();

                foreach (var doc in doctors)
                {
                    var heroCard = new ThumbnailCard
                    {
                        Title = doc.Name,
                        Subtitle = $"{doc.Rating} stars. {doc.NumberOfReviews} reviews.",
                        Images = new List<CardImage>
                        {
                            new CardImage {Url = doc.Image}
                        },
                        Buttons = new List<CardAction>
                        {
                            new CardAction
                            {
                                Title = "More details",
                                Type = ActionTypes.OpenUrl,
                                Value =
                                    $"https://www.google.com/search?q=doctors+in+" + HttpUtility.UrlEncode(doc.Location)
                            }
                        }
                    };

                    resultMessage.Attachments.Add(heroCard.ToAttachment());
                }

                await context.PostAsync(resultMessage);
            }
            catch (FormCanceledException ex)
            {
                string reply;

                if (ex.InnerException == null)
                    reply = "You have canceled the operation. Quitting from the Search Dialog";
                else
                    reply = $"Oops! Something went wrong :( Technical Details: {ex.InnerException.Message}";

                await context.PostAsync(reply);
            }
        }
    }
}
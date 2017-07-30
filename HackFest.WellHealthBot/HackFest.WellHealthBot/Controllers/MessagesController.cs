using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using HackFest.WellHealthBot.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace HackFest.WellHealthBot.Controllers
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        ///     POST: api/Messages
        ///     Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
                await Conversation.SendAsync(activity, () => new LuisAIDialog());
            else
                HandleSystemMessage(activity);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message

                message.GetStateClient().BotState.DeleteStateForUser(message.ChannelId, message.From.Id);

            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels


                IConversationUpdateActivity update = message;

                if (update.MembersAdded != null && update.MembersAdded.Any())
                {
                    foreach (var newMember in update.MembersAdded)
                    {
                        if (newMember.Id != message.Recipient.Id)
                        {
                            ConnectorClient connector = new ConnectorClient(new Uri(message.ServiceUrl));

                            Activity reply = message.CreateReply();
                            reply.Text = "Hi, I'm Well-Health Bot!";
                            reply.Attachments.Add(new Attachment()
                            {
                                ContentUrl = "https://logo.clearbit.com/https:/www.wellhealthqc.com/",
                                ContentType = "image/png",
                                Name = "wellhealth.jpeg"
                            });
                            //attachmentMsg.Attachments.Add(new Attachment() { ContentUrl = "https://logo.clearbit.com/https:/www.healthwellfoundation.org/", ContentType = "image/png", Name = "wellhealth.jpeg" });
                            reply.AttachmentLayout = AttachmentLayoutTypes.List;
                            connector.Conversations.ReplyToActivity(reply);
                        }
                    }
                }
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                return message.CreateReply("...");
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
                return message.CreateReply("ping");
            }

            return null;
        }

    }
}
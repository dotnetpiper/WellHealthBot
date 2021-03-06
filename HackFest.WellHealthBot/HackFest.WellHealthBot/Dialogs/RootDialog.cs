﻿using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;

namespace HackFest.WellHealthBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Call(new DoctorDialog(), ResumeAfterOptionDialog);

            return Task.CompletedTask;
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
                context.Wait(ResumeAfterOptionDialog);
            }
        }
    }
}
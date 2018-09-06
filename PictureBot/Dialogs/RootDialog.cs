using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace PictureBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result;

            PromptOptions<string> prompt = new PromptOptions<string>(
                "Would you like to see a picture?",
                "Please answer yes or no.",
                "Too many attempts... we can try again later!");

            PromptDialog.Confirm(context, ResumeAfterPromptDialog, prompt);
        }

        private async Task ResumeAfterPromptDialog(IDialogContext context, IAwaitable<bool> result)
        {
            try
            {
                var confirm = await result;

                if (confirm)
                {
                    context.Call(new PictureDialog(), ResumeAfterPictureDialog);
                }
                else
                {
                    await context.PostAsync("OK, we can try again later!");
                }
            }
            catch (TooManyAttemptsException)
            {
                // handled by prompt
            }
            catch (Exception ex)
            {
                await context.PostAsync($"An error occurred: {ex.Message}");
            }
        }

        private async Task ResumeAfterPictureDialog(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var message = await result;
            }
            catch (Exception ex)
            {
                await context.PostAsync($"An error occurred: {ex.Message}");
            }
            finally
            {
                context.Wait(MessageReceivedAsync);
            }
        }
    }
}
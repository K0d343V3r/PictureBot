using Microsoft.Bot.Builder.Dialogs;
using PictureBot.Helpers;
using System;
using System.Threading.Tasks;

namespace PictureBot.Dialogs
{
    [Serializable]
    public class PictureDialog : IDialog<object>
    {
        private const string _dogsOption = "A dog";
        private const string _catsOption = "A cat";
        private const string _otherOption = "Something else";
        private static readonly string[] _options = { _dogsOption, _catsOption, _otherOption };

        public Task StartAsync(IDialogContext context)
        {
            DisplayOptions(context);

            return Task.CompletedTask;
        }

        private void DisplayOptions(IDialogContext context)
        {
            PromptOptions<string> prompt = new PromptOptions<string>(
                 "What would you like to see?",
                 "Please select one of these options.",
                 "Too many attempts... please try again!",
                 _options
                 );
            PromptDialog.Choice(context, ResumeAfterPromptDialog, prompt);
        }

        private async Task ResumeAfterPromptDialog(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                var selectedOption = await result;

                switch (selectedOption)
                {
                    case _dogsOption:
                    case _catsOption:
                        var url = await PhotoHelper.GetPhotoUrlAsync(selectedOption);
                        var message = MessageHelper.GetHeroCardMessage(context, url);
                        await context.PostAsync("There you go!");
                        await context.PostAsync(message);

                        context.Done<object>(null);
                        break;

                    case _otherOption:
                        context.Call(new UnknownPictureDialog(), ResumeAfterUnknownPictureDialog);
                        break;
                }
            }
            catch (TooManyAttemptsException)
            {
                // handled by prompt
                context.Done<object>(null);
            }
            catch (Exception ex)
            {
                await context.PostAsync($"An error occurred: {ex.Message}");
                context.Done<object>(null);
            }
        }

        private async Task ResumeAfterUnknownPictureDialog(IDialogContext context, IAwaitable<object> result)
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
                context.Done<object>(null);
            }
        }
    }
}
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using PictureBot.Helpers;
using System;
using System.Threading.Tasks;

namespace PictureBot.Dialogs
{
    [Serializable]
    public class UnknownPictureDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("OK, what would you like to see?");

            context.Wait(AfterMessageReceived);
        }

        private async Task AfterMessageReceived(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            try
            {
                var activity = await result;
                var url = await PhotoHelper.GetPhotoUrlAsync(activity.Text);
                var message = MessageHelper.GetHeroCardMessage(context, url);
                await context.PostAsync("There you go!");
                await context.PostAsync(message);
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
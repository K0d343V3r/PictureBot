using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace PictureBot.Helpers
{
    public static class MessageHelper
    {
        public static bool IsExitMessage(string message)
        {
            if (String.IsNullOrEmpty(message))
                throw new ArgumentOutOfRangeException("message", "is null or empty.");

            return string.Compare(message, "exit", true, CultureInfo.CurrentCulture) == 0 ||
                string.Compare(message, "cancel", true, CultureInfo.CurrentCulture) == 0 ||
                string.Compare(message, "done", true, CultureInfo.CurrentCulture) == 0;
        }

        public static IMessageActivity GetHeroCardMessage(IDialogContext context, string pictureUrl)
        {
            if (String.IsNullOrEmpty(pictureUrl))
                throw new ArgumentOutOfRangeException("pictureUrl", "is null or empty.");

            var card = new HeroCard()
            {
                Images = new List<CardImage>() { new CardImage(pictureUrl) }
            };

            var message = context.MakeMessage();
            message.Attachments.Add(card.ToAttachment());
            return message;
        }
    }
}
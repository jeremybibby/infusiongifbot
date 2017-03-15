using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using InfusionGifBot.Helpers;

namespace InfusionGifBot.Dialogs
{
    [Serializable]
    public class GiphyDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(HandleDialog);

            var welcomeMessage = context.MakeMessage();
            welcomeMessage.Text = "Welcome to GIF search.  Please enter the terms you want to search for!";
            await context.PostAsync(welcomeMessage);
        }

        private async Task HandleDialog(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var inboundMessage = await argument;
            var outboundMessage = await GiphyHelper.GenerateMessage(context, inboundMessage.Text.Split(' ').ToList());

            await context.PostAsync(outboundMessage);
            context.Done<object>(null);
        }
    }
}






// await GiphyHelper.GenerateMessage(context, inboundMessage.Text.Split(' ').ToList());
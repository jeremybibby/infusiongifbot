using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace InfusionGifBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);            
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var inboundMessage = await argument;

            if (inboundMessage.Text.ToLower().Contains("search"))
            {
                context.Call(new GiphyDialog(), dialogcomplete);
            }
            else if (inboundMessage.Text.ToLower().Contains("form"))
            {
                context.Call(new GiphyFormDialog(), dialogcomplete);
            }
            else
            {
                var outboundMessage = context.MakeMessage();
                outboundMessage.Text = string.Format("Please enter search or form!", inboundMessage.Text.Length);
                await context.PostAsync(outboundMessage);
                context.Wait(MessageReceivedAsync);
            }
        }

        private async Task dialogcomplete(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceivedAsync);
        }
    }











    /*    [Serializable]
        public class RootDialog : IDialog<object>
        {
            public async Task StartAsync(IDialogContext context)
            {
                context.Wait(this.MessageReceivedAsync);
            }

            public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
            {
                var inboundMessage = await argument;

                // determine if they picked one of the options
                if (inboundMessage.Text.ToLower().Contains("search"))
                {
                    context.Call(new GiphyDialog(), ResumeAfterGIFDialog);
                }
                else if (inboundMessage.Text.ToLower().Contains("form"))
                {
                    context.Call(new GiphyFormDialog(), ResumeAfterGIFDialog);
                }
                else
                {
                    var outboundMessage = context.MakeMessage();
                    outboundMessage.Text = "Please enter either 'search' or 'form' to continue";
                    await context.PostAsync(outboundMessage);

                    context.Wait(MessageReceivedAsync);
                }
            }

            private async Task ResumeAfterGIFDialog(IDialogContext context, IAwaitable<object> result)
            {
                context.Wait(MessageReceivedAsync);
            }
        } */
}
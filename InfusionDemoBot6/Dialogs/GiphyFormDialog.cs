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
using Microsoft.Bot.Builder.FormFlow;
using InfusionGifBot.Helpers;

namespace InfusionGifBot.Dialogs
{
    public enum Topic
    {
        Animals,
        Cat,
        Dog,
        Panda,
        Robot,
        Transformers,        
    }

    public enum Modifier
    {
        Funny,
        Serious,
        Crazy,
        Spicy,
        Dank,
        Weird,
        None
    }

    
    [Serializable]
    public class SearchParameters
    {
        public Topic SearchTopic { get; set; }
        public Modifier SearchModifier { get; set; }

        public static IForm<SearchParameters> BuildForm()
        {
            return new FormBuilder<SearchParameters>().Message("Welcome to the GIF search helper!").Build();
        }
    }








    [Serializable]
    public class GiphyFormDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            var searchForm = FormDialog.FromForm(SearchParameters.BuildForm,FormOptions.PromptInStart);
            context.Call(searchForm, ResumeAfterGIFDialog);
        }

        private async Task ResumeAfterGIFDialog(IDialogContext context, IAwaitable<SearchParameters> argument)
        {
            var parameters= await argument;
            List<string> searchParams = new List<string>() { parameters.SearchTopic.ToString() };
            if (parameters.SearchModifier != Modifier.None) searchParams.Add(parameters.SearchModifier.ToString());

            var outboundMessage = await GiphyHelper.GenerateMessage(context, searchParams);
            await context.PostAsync(outboundMessage);
            context.Done<object>(null);
        }
        
    }
}
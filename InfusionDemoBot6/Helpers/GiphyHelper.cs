using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace InfusionGifBot.Helpers
{
    public static class GiphyHelper
    {
        public static async Task<IMessageActivity> GenerateMessage(IDialogContext context, List<string> searchTerms)
        {
            if (searchTerms.Count == 0) searchTerms.Add("cat"); // Cats always a safe bet

            // Create out message
            var outboundMessage = context.MakeMessage();

            // form our search string, probably better ways to do this :)
            var searchString = string.Concat(searchTerms.Select(s=>s+"+"));
            searchString = searchString.Substring(0, searchString.Length - 1);

            // fire up a web client and get the search results
            var client = new HttpClient() { BaseAddress = new Uri("http://api.giphy.com") };
            var result = await client.GetStringAsync("/v1/gifs/search?q=" + searchString + "&api_key=dc6zaTOxFJmzC");

            // parse and pull out the relevant data
            var data = ((dynamic)JObject.Parse(result)).data;
            var gif = data[(new Random()).Next(data.Count)];
            var gifUrl = gif.images.fixed_height.url.Value;
            var slug = gif.slug.Value;

            // attach the gif URL to the message
            outboundMessage.Attachments = new List<Attachment>();
            outboundMessage.Attachments.Add(new Attachment()
            {
                ContentUrl = gifUrl,
                ContentType = "image/gif",
                Name = slug + ".gif"
            });

            return outboundMessage;

        }
    }
}
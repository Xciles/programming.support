using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Programming.Bot.Domain;

namespace Programming.Bot.Dialogs
{
    [Serializable]
    public class SupportDialog : IDialog<object>
    {
        private readonly HttpClient _httpClient = new HttpClient();
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;


            string resultString;

            QueryResult luisResult = await GetEntityFromLuis(message.Text);

            switch (luisResult.topScoringIntent.intent)
            {
                case "StackOverflow":
                    {
                        if (luisResult.entities.Any())
                        {
                            resultString = "";
                        }
                        break;
                    }
                default:
                    {
                        resultString = "Sorry, I am not getting you...";
                        break;
                    }
                   
            }
            await context.PostAsync(resultString);
            context.Wait(MessageReceivedAsync);
        }

        private async Task<QueryResult> GetEntityFromLuis(string query)
        {
            query = Uri.EscapeDataString(query);
            var data = new QueryResult();
            //TODO url
            var requestUri = "https://" + query;
            //var requestUri = "https://api.projectoxford.ai/luis/v2.0/apps/5c31bde1-79e0-4dbe-b1e7-7b046fc37f13?subscription-key=b118918f818b4cc6acc9e1e87bbdb74b&q=" + query;
            var msg = await _httpClient.GetAsync(requestUri);

            if (msg.IsSuccessStatusCode)
            {
                var jsonDataResponse = await msg.Content.ReadAsStringAsync();
                data = JsonConvert.DeserializeObject<QueryResult>(jsonDataResponse);
            }
        
            return data;
        }
    }
}
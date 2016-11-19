using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Programming.Bot.Business;
using Programming.Bot.Domain;

namespace Programming.Bot.Dialogs
{
    [Serializable]
    public class SupportDialog : IDialog<object>
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string[] Greetings = new[] { "Hi", "Hello!", "Hallo", "Konichiwa" };
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            
            string resultString = "Sorry, I am not getting you...";

            QueryResult luisResult = await GetEntityFromLuis(message.Text);

            switch (luisResult.topScoringIntent.intent)
            {
                case "Coding":
                    {
                        if (luisResult.entities.Any())
                        {
                            resultString = await StackOverflow.Query(luisResult.entities[0].entity);
                        }
                        break;
                    }

                case "Greeting":
                    {
                        var rand = new Random();
                        resultString = Greetings[rand.Next(0, Greetings.Length)];
                        break;
                    }

                case "OrderPizza":
                    {
                        var rand = new Random();
                        resultString = Greetings[rand.Next(0, Greetings.Length)];
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
            var requestUri = string.Empty;
            
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
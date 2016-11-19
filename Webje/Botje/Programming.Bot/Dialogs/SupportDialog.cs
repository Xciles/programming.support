using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Programming.Bot.Business;
using Programming.Bot.Domain;
using Xkcd;

namespace Programming.Bot.Dialogs
{
    [Serializable]
    public class SupportDialog : IDialog<object>
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string[] Greetings = new[] {"Hi", "Hello!", "Hallo", "Konichiwa"};
        private bool _pizzaFlow = false;

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            if (_pizzaFlow)
            {
                
                if (message.Text == "abort")
                {
                    PromptDialog.Confirm(context, AfterResetAsync, "Are you sure you want to abort ordering?",
                        "Didn't get that!", promptStyle: PromptStyle.None);
                }
                context.Wait(MessageReceivedAsync);

            }
            else

            {


                string resultString = "Sorry, I am not getting you...";

                QueryResult luisResult = await GetEntityFromLuis(message.Text);

                switch (luisResult.topScoringIntent.intent)
                {
                    case "Coding":
                    {
                        if (luisResult.entities.Any())
                        {
                            var query =
                                luisResult.entities.Select(x => x.entity)
                                    .Aggregate((current, next) => current + " " + next);

                            resultString = await StackOverflow.Query(query);
                        }
                        context.Wait(MessageReceivedAsync);
                        break;
                    }

                    case "Greeting":
                    {
                        var rand = new Random();
                        resultString = Greetings[rand.Next(0, Greetings.Length)];
                        context.Wait(MessageReceivedAsync);
                        break;
                    }

                    case "xkcd":
                    {
                        var reply = context.MakeMessage();
                        var imgString = string.Empty;
                        if (luisResult.entities.Any())
                        {
                            imgString = await XkcdLib.GetComic(luisResult.entities[0].entity);
                        }
                        else
                        {
                           imgString = await XkcdLib.GetRandomComic();
                        }

                        reply.Attachments = new List<Attachment>
                        {
                            new Attachment()
                            {
                                ContentUrl = imgString,
                                ContentType = "image/jpg",
                                Name = $"{Guid.NewGuid()}.jpg"
                            }
                        };

                        await context.PostAsync(reply);
                        return;
                    }

                    case "meme":
                    {
                        var reply = context.MakeMessage();

                        await context.PostAsync(reply);
                        return;
                    }


                    case "OrderPizza":
                    {
                            _pizzaFlow = true;
                            PromptDialog.Text(context, Resume, "Hmm would you like to order pizza?", "Didn't get that!");
                        
                        break;
                    }

                    default:
                    {
                        resultString = "Sorry, I am not getting you...";
                        context.Wait(MessageReceivedAsync);
                        break;
                    }

                }
                await context.PostAsync(resultString);
            }
        }


        private async Task Resume(IDialogContext context, IAwaitable<string> result)
        {
            var message = await result;
            message = message.ToLower();

            if (message == "yes" || message == "yeah")
            {
                _pizzaFlow = true;
                await context.PostAsync($"You have now entered the order pizza flow (type 'abort' to abort!)");

            }
            else
                _pizzaFlow = false;
                await context.PostAsync($"Too bad");

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



        public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                _pizzaFlow = false;
                await context.PostAsync("Aborted!");
            }
            else
            {
                await context.PostAsync("Did not abort.");
            }
            context.Wait(MessageReceivedAsync);
        }

    }
}
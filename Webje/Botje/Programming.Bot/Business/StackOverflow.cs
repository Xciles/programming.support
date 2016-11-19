using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Programming.Bot.Domain.StackOverflow;

namespace Programming.Bot.Business
{

    public static class StackOverflow
    {
        private static readonly HttpClient HttpClient = new HttpClient( new HttpClientHandler()
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        });
        private const string StackOverflowSearchUri = "https://api.stackexchange.com/2.2/search?order=desc&sort=votes&tagged={1}&intitle={0}&site=stackoverflow";
        //private const string StackOverflowSearchUri = "http://api.stackexchange.com/2.2/search?order=desc&sort=votes&site=stackoverflow&intitle={0}";
        private const string StackOverflowQuestionUri = "https://api.stackexchange.com/2.2/questions/{0}/answers?order=desc&sort=votes&site=stackoverflow";
        private const string StackOverflowAnswerUri = "http://api.stackexchange.com/2.2/answers/{0}?order=desc&sort=activity&site=stackoverflow&filter=!9YdnSM68f";

        public static async Task<SoResult> Query(string query, string tag = "")
        {
            var result = new SoResult()
            {
                Body = "No satisfying answer found"
            };

            query = Uri.EscapeDataString(query);
            tag = Uri.EscapeDataString(tag);

            var msg = await HttpClient.GetAsync(string.Format(StackOverflowSearchUri, query, tag));

            if (!msg.IsSuccessStatusCode)
            {
                throw new Exception("Stackoverflow search API call failed");
            }
            
            var jsonDataResponse = await msg.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<SearchResults>(jsonDataResponse);

            if (data.items == null || !data.items.Any()) return result;

            msg = await HttpClient.GetAsync(string.Format(StackOverflowQuestionUri, data.items[0].question_id));
            if (!msg.IsSuccessStatusCode)
            {
                throw new Exception("Stackoverflow answer API call failed");
            }
            jsonDataResponse = await msg.Content.ReadAsStringAsync();
            var questionData = JsonConvert.DeserializeObject<QuestionResult>(jsonDataResponse);

            if (questionData.items == null || !questionData.items.Any()) return result;

            var acceptedAnswerId = questionData.items[0].answer_id;
            if (acceptedAnswerId != 0)
            {
                msg = await HttpClient.GetAsync(string.Format(StackOverflowAnswerUri, acceptedAnswerId));

                if (!msg.IsSuccessStatusCode)
                {
                    throw new Exception("Stackoverflow answer API call failed");
                }

                jsonDataResponse = await msg.Content.ReadAsStringAsync();
                var answerData = JsonConvert.DeserializeObject<AnswerResult>(jsonDataResponse);

                result.Body = answerData.items[0].body;
                result.Link = $"http://stackoverflow.com/a/{answerData.items[0].answer_id}";
                //result.Link = $"https://stackoverflow.com/questions/{answerData.items[0].question_id}";
                result.Title = data.items[0].title;

                return result;
            }
            return result;
        }

        public static string RemoveHtmlTags(string html)
        {
            if (html == null) return string.Empty;
            var response = Regex.Replace(html, @"<[^>]+>|&nbsp;", "").Trim();
            response = response.Replace("&amp;", "&");
            return response;
        }
    }
}
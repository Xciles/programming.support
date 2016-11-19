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
        private const string StackOverflowSearchUri = "http://api.stackexchange.com/2.2/search?order=desc&sort=votes&site=stackoverflow&intitle={0}";
        private const string StackOverflowQuestionUri = "https://api.stackexchange.com/2.2/questions/{0}/answers?order=desc&sort=votes&site=stackoverflow";
        private const string StackOverflowAnswerUri = "http://api.stackexchange.com/2.2/answers/{0}?order=desc&sort=activity&site=stackoverflow&filter=!9YdnSMKKT";

        public static async Task<string> Query(string query, bool cleanHtml = false)
        {
            query = Uri.EscapeDataString(query);
            var msg = await HttpClient.GetAsync(string.Format(StackOverflowSearchUri,query));

            if (!msg.IsSuccessStatusCode)
            {
                throw new Exception("Stackoverflow search API call failed");
            }
            
            var jsonDataResponse = await msg.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<SearchResults>(jsonDataResponse);

            if (data.items == null || data.items.Length <= 0) return "No satisfying answer found";

            msg = await HttpClient.GetAsync(string.Format(StackOverflowQuestionUri, data.items[0].question_id));
            if (!msg.IsSuccessStatusCode)
            {
                throw new Exception("Stackoverflow answer API call failed");
            }
            jsonDataResponse = await msg.Content.ReadAsStringAsync();
            var questionData = JsonConvert.DeserializeObject<QuestionResult>(jsonDataResponse);

            if (questionData.items == null || questionData.items.Length <= 0) return "No satisfying answer found";


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

                return cleanHtml ? RemoveHtmlTags(answerData.items[0].body) : answerData.items[0].body;
            }
            return "No satisfying answer found";
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
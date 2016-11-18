using System;
using System.Collections.Generic;
using System.Linq;
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
        private static readonly HttpClient HttpClient = new HttpClient();
        private const string StackOverflowSearchUri = "http://api.stackexchange.com/2.2/search?order=desc&sort=votes&intitle={0}&site=stackoverflow";
        private const string StackOverflowAnswerUri = "http://api.stackexchange.com/2.2/answers/{0}?order=desc&sort=activity&site=stackoverflow&filter=!9YdnSMKKT";

        public static async Task<string> Query(string query)
        {
            var msg = await HttpClient.GetAsync(string.Format(StackOverflowSearchUri,query));

            if (!msg.IsSuccessStatusCode)
            {
                throw new Exception("Stackoverflow search API call failed");
            }

            var jsonDataResponse = await msg.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<SearchResults>(jsonDataResponse);

            var acceptedAnswerId = data.items[0].accepted_answer_id;

            msg = await HttpClient.GetAsync(string.Format(StackOverflowAnswerUri, acceptedAnswerId));

            if (!msg.IsSuccessStatusCode)
            {
                throw new Exception("Stackoverflow answer API call failed");
            }

            jsonDataResponse = await msg.Content.ReadAsStringAsync();
            var answerData = JsonConvert.DeserializeObject<AnswerResult>(jsonDataResponse);

            return RemoveHtmlTags(answerData.items[0].body);
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
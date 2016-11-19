using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Programming.Web.Domain.StackOverflow;
using Programming.Web.ServiceDomain;

namespace Programming.Web.Business
{
    public static class StackOverflow
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private const string StackOverflowSearchUri = "http://api.stackexchange.com/2.2/search?order=desc&sort=votes&intitle={0}&site=stackoverflow";
        private const string StackOverflowQuestionUri = "https://api.stackexchange.com/2.2/questions/{0}/answers?order=desc&sort=votes&site=stackoverflow";
        private const string StackOverflowAnswerUri = "http://api.stackexchange.com/2.2/answers/{0}?order=desc&sort=activity&site=stackoverflow&filter=!9YdnSMKKT";

        public static async Task<SoResponse> Query(string query)
        {
            query = Uri.EscapeDataString(query);
            var msg = await HttpClient.GetAsync(string.Format(StackOverflowSearchUri, query));

            if (!msg.IsSuccessStatusCode)
            {
                throw new Exception("Stackoverflow search API call failed");
            }

            var jsonDataResponse = await msg.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<SearchResults>(jsonDataResponse);

            if (data.items == null || !data.items.Any())
            {
                return new SoResponse()
                {
                    Body = "NoResults!",
                    Question = query, 
                    StrippedBody = "NoResults!"
                };
            }

            msg = await HttpClient.GetAsync(string.Format(StackOverflowQuestionUri, data.items[0].question_id));
            if (!msg.IsSuccessStatusCode)
            {
                throw new Exception("Stackoverflow answer API call failed");
            }
            jsonDataResponse = await msg.Content.ReadAsStringAsync();
            var questionData = JsonConvert.DeserializeObject<QuestionResult>(jsonDataResponse);

            if (questionData.items == null || !questionData.items.Any())
            {
                return new SoResponse()
                {
                    Body = "NoResults!",
                    Question = query,
                    StrippedBody = "NoResults!"
                };
            }

            var acceptedAnswerId = questionData.items[0].answer_id;

            msg = await HttpClient.GetAsync(string.Format(StackOverflowAnswerUri, acceptedAnswerId));

            if (!msg.IsSuccessStatusCode)
            {
                throw new Exception("Stackoverflow answer API call failed");
            }

            jsonDataResponse = await msg.Content.ReadAsStringAsync();
            var answerData = JsonConvert.DeserializeObject<AnswerResult>(jsonDataResponse);

            if (answerData.items == null || !answerData.items.Any())
            {
                return new SoResponse()
                {
                    Body = "NoResults!",
                    Question = query,
                    StrippedBody = "NoResults!"
                };
            }

            var strippedHtml = RemoveHtmlTags(answerData.items[0].body);

            var response = new SoResponse()
            {
                Body = answerData.items[0].body,
                Question = query, 
                StrippedBody = strippedHtml
            };

            return response;
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

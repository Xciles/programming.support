using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xciles.Uncommon.Net;
using System.Net.Http;

namespace ProgrammingSupport.Core.Business
{
    public static class StackAnswerer
    {
        private const string _hyperion = "http://programmingsupport.azurewebsites.net/api/hyperion/soquestion";

        public static async Task<string> AnswerMe(string question)
        {
            try
            {
                HttpContent contentPost = new StringContent(GetStackRequest(question), Encoding.UTF8, "application/json");

                using (HttpClient client = new HttpClient())
                {
                    var result = await client.PostAsync(_hyperion, contentPost);
                    if (result.IsSuccessStatusCode)
                    {
                        var response = JsonConvert.DeserializeObject<StackResponse>(await result.Content.ReadAsStringAsync());

                        return response.StrippedBody;
                    }
                    return "NoResults!";
                }                
            }
            catch(Exception ex)
            {
                return "Oh no Kenny Died!";
            }
        }


        public static string GetStackRequest(string question)
        {
            return JsonConvert.SerializeObject(new StackRequest() { Question = question });
        }
    }

    public class StackRequest
    {
        public string Question { get; set; }
    }

   
    public class StackResponse
    {
        [JsonProperty(PropertyName = "body")]
        public string Body { get; set; }
        [JsonProperty(PropertyName = "question")]
        public string Question { get; set; }
        [JsonProperty(PropertyName = "strippedBody")]
        public string StrippedBody { get; set; }
    }
}

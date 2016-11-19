using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Xkcd
{
    public static class XkcdLib
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        private const string XkcdUri = "http://xkcd.com/{0}";
        private const string XkcdRandomUri = "http://c.xkcd.com/random/comic/";


        public static async Task<string> GetComic(string comicId)
        {
            var msg = await HttpClient.GetAsync(string.Format(XkcdUri, comicId));

            if (!msg.IsSuccessStatusCode)
            {
                throw new Exception("");
            }

            var response = await msg.Content.ReadAsStringAsync();
            return GetImageUri(response);
        }
        public static async Task<string> GetRandomComic()
        {
            var msg = await HttpClient.GetAsync(string.Format(XkcdRandomUri));

            if (!msg.IsSuccessStatusCode)
            {
                throw new Exception("");
            }

            var response = await msg.Content.ReadAsStringAsync();

            return GetImageUri(response);
        }

        private static string GetImageUri(string response)
        {
            var htmlDoc = new HtmlAgilityPack.HtmlDocument {OptionFixNestedTags = true};
            
            htmlDoc.LoadHtml(response);

            string result = string.Empty;
            if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Any())
            {
                // Handle any parse errors as required

            }
            else
            {
                var imageNode = htmlDoc.DocumentNode?.SelectSingleNode("//body//div[@id='comic']//img");

                if (imageNode != null)
                {
                    result = $"http:{imageNode.GetAttributeValue("src", "")}";
                }
            }

            return result;
        }
    }
}

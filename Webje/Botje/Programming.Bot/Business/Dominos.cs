using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Programming.Bot.Domain.Dominos;

namespace Programming.Bot.Business
{
    public static class Dominos
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private const string PizzaMenuUri = "https://hackathon-menu.dominos.cloud/Rest/nl/menus/30544/en";
        private const string PizzaDetailsUri ="https://hackathon-menu.dominos.cloud/Rest/nl/menus/30544/products/{0}/en";
        private const string PizzaOrderUri = "https://hackathon.dominos.cloud/order/place";
        private const string PizzaOrderStatusUri = "https://hackathon.dominos.cloud/order/status";

        private const string PizzaStoreNumber = "1111";
        private const string PizzaVendorId = "1234";
 
        public static IList<PizzaResult> Pizzases;

        private static readonly Task InitTask;
        
        static Dominos()
        {
            InitTask = Init();
        }

        private static async Task Init()
        {
            // init requests
            
           await ProcessMenuResult(GetMenu().Result);

        }

        private static async Task ProcessMenuResult(MenuResult getMenu)
        {
            //pizza's
            var submenus = getMenu.MenuPages[0].SubMenus;
            foreach (var sub in submenus)
            {
                foreach (var product in sub.Products)
                {
                    var pizza = await GetPizzaDetails(product.LinkedItem.ItemCode);
                    Pizzases.Add(pizza);
                }
                
            }
        }

        private static async Task<MenuResult> GetMenu()
        {
            var msg = await HttpClient.GetAsync(string.Format(PizzaMenuUri));

            if (!msg.IsSuccessStatusCode)
            {
                throw new Exception("Pizza menu API call failed");
            }

            var jsonDataResponse = await msg.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<MenuResult>(jsonDataResponse);

            return data;
        }



        private static async Task<PizzaResult> GetPizzaDetails(string pizzacode)
        {
            var msg = await HttpClient.GetAsync(string.Format(PizzaDetailsUri,pizzacode));

            if (!msg.IsSuccessStatusCode)
            {
                throw new Exception("Pizza details API call failed");
            }

            var jsonDataResponse = await msg.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<PizzaResult>(jsonDataResponse);
            return data;
        }



        public static async Task<OrderResult> PlaceOrder(bool payedInCash,string name, string phoneNumber, Product2[] products)
        {
            await InitTask;

            OrderRequest order = new OrderRequest()
            {
                CountryCode = "nl",IsCashPayment = payedInCash,
                Language = "NL-nl",
                Name = name,
                OrderDate = DateTime.Now,
                StoreNo = PizzaStoreNumber,
                VendorId = PizzaVendorId,
                Products = products,
            };

            var msg = await HttpClient.PostAsJsonAsync(PizzaOrderUri, order);
            if (!msg.IsSuccessStatusCode)
            {
                throw new Exception("Pizza order  API call failed");
            }

            var jsonDataResponse = await msg.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<OrderResult>(jsonDataResponse);
            return data;
        }

        public static async Task<OrderStatusResult> GetOrderStatus(string orderstatus)
        {

            var msg = await HttpClient.PostAsJsonAsync(PizzaOrderStatusUri,new OrderStatusRequest() {CountryCode = "nl",VendorId = PizzaVendorId, OrderId = orderstatus});

            if (!msg.IsSuccessStatusCode)
            {
                throw new Exception("Pizza order status API call failed");
            }

            var jsonDataResponse = await msg.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<OrderStatusResult>(jsonDataResponse);
            return data;
        }
    }
}
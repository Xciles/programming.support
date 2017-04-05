using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Newtonsoft.Json;
using Programming.Bot.Business;
using Programming.Bot.Domain.Dominos;

namespace Programming.Bot.Dialogs
{
    public enum PizzaOptions
    {
        // 0 value in enums is reserved for unknown values.  Either you can supply an explicit one or start enumeration at 1.

        ChefBestPizzas,
        SoGaucho,
        SoAsian,
        BBQMixedGrill,
        Extravaganzza,
        FourCheese,
        Shawarma,
        Supreme,
        Americana,
        ChickenSupreme,
    };

    public enum SignatureOptions { Hawaiian = 1, Pepperoni, MurphysCombo, ChickenGarlic, TheCowboy };
    public enum GourmetDeliteOptions { SpicyFennelSausage = 1, AngusSteakAndRoastedGarlic, GourmetVegetarian, ChickenBaconArtichoke, HerbChickenMediterranean };
    public enum StuffedOptions { ChickenBaconStuffed = 1, ChicagoStyleStuffed, FiveMeatStuffed };

    // Fresh Pan is large pizza only
    public enum CrustOptions
    {
        Original = 1, Thin, Stuffed, FreshPan, GlutenFree
    };

    public enum SauceOptions
    {
        [Terms(new string[] { "traditional", "tomatoe?" })]
        Traditional = 1,
        CreamyGarlic, OliveOil
    };

    public enum ToppingOptions
    {
        Beef = 1,
        BlackOlives,
        CanadianBacon,
        CrispyBacon,
        Garlic,
        GreenPeppers,
        GrilledChicken,

        [Terms(new string[] { "herb & cheese", "herb and cheese", "herb and cheese blend", "herb" })]
        HerbAndCheeseBlend,
        ItalianSausage,
        ArtichokeHearts,
        MixedOnions,
        MozzarellaCheese,
        Mushroom,
        Onions,
        ParmesanCheese,
        Pepperoni,
        Pineapple,
        RomaTomatoes,
        Salami,
        Spinach,
        SunDriedTomatoes,
        Zucchini,
        ExtraCheese
    };

    public enum CouponOptions { Large20Percent = 1, Pepperoni20Percent };

    [Serializable]
    class BYOPizza
    {
        public CrustOptions Crust;
        public SauceOptions Sauce;
        public List<ToppingOptions> Toppings = new List<ToppingOptions>();
    };

    [Serializable]
    class PizzaOrder
    {
        [Prompt("What kind of pizza do you want? {||}")]
        public PizzaOptions Pizza;
        //[Template(TemplateUsage.NotUnderstood, "What does \"{0}\" mean???")]
        //[Describe("Kind of pizza")]
        //public PizzaOptions Kind;
        //public SignatureOptions Signature;
        //public GourmetDeliteOptions GourmetDelite;
        //public StuffedOptions Stuffed;
        //public BYOPizza BYO;
        public string YourAddress;
        public string YourPhonenumber;
        public bool PayInCash;
        //[Optional]
        //public CouponOptions Coupon;

        //public string OrderId { get; set; }

        //public override string ToString()
        //{
        //    var builder = new StringBuilder();
        //    //builder.AppendFormat("PizzaOrder({0}, ", Size);
        //    //switch (Kind)
        //    //{
        //    //    //case PizzaOptions.BYOPizza:
        //    //    //    builder.AppendFormat("{0}, {1}, {2}, [", Kind, BYO.Crust, BYO.Sauce);
        //    //    //    foreach (var topping in BYO.Toppings)
        //    //    //    {
        //    //    //        builder.AppendFormat("{0} ", topping);
        //    //    //    }
        //    //    //    builder.AppendFormat("]");
        //    //    //    break;
        //    //    case PizzaOptions.GourmetDelitePizza:
        //    //        builder.AppendFormat("{0}, {1}", Kind, GourmetDelite);
        //    //        break;
        //    //    case PizzaOptions.SignaturePizza:
        //    //        builder.AppendFormat("{0}, {1}", Kind, Signature);
        //    //        break;
        //    //    case PizzaOptions.StuffedPizza:
        //    //        builder.AppendFormat("{0}, {1}", Kind, Stuffed);
        //    //        break;
        //    //}
        //    builder.AppendFormat(", {0}, {1})", Address, Coupon);
        //    return builder.ToString();
        //}

        public static IForm<PizzaOrder> BuildForm()
        {
            return new FormBuilder<PizzaOrder>()
                    .Message("Welcome to the Dominos pizza bot!")
                    .OnCompletion(Callback)
                    .Build();
        }

        private const string PizzaMenuUri = "https://hackathon-menu.dominos.cloud/Rest/nl/menus/30544/en";
        private const string PizzaDetailsUri = "https://hackathon-menu.dominos.cloud/Rest/nl/menus/30544/products/{0}/en";
        private const string PizzaOrderUri = "https://hackathon.dominos.cloud/order/place";
        private const string PizzaOrderStatusUri = "https://hackathon.dominos.cloud/order/status";
        private const string PizzaStoreNumber = "11111";
        private const string PizzaVendorId = "1234";

        private static async Task Callback(IDialogContext context, PizzaOrder state)
        {
            // Todo dominos flow
            //state.OrderId = "TempOrderId";
            //var result = await Dominos.PlaceOrder(state.YourAddress, state.PayInCash, "Hack3r", state.YourPhonenumber, null);

            OrderRequest order = new OrderRequest()
            {
                CountryCode = "NL",
                IsCashPayment = state.PayInCash,
                Language = "nl",
                Name = "Hack3r Hack3r",
                OrderDate = DateTime.Now,
                StoreNo = PizzaStoreNumber,
                VendorId = PizzaVendorId,
                PhoneNumber=state.YourPhonenumber,
                Products = new Product2[] {
                    new Product2()
                    {
                        ProductCode ="PPPE",
                        SizeCode="Pizza.30CM",
                        Price = "9.45"
                    }
                }
            };
            var client = new HttpClient();
            try
            {
                var msg = await client.PostAsJsonAsync(PizzaOrderUri, order);
                if (!msg.IsSuccessStatusCode)
                {
                    var content = await msg.Content.ReadAsStringAsync();
                    //throw new Exception("Pizza order  API call failed");
                }
            }
            catch (Exception e)
            {
            }

           // var jsonDataResponse = await msg.Content.ReadAsStringAsync();

            //var data = JsonConvert.DeserializeObject<OrderResult>(jsonDataResponse);

            await
                context.PostAsync(
                    $"Thank you for your order, your id is 13, and will be at your doorstep in 37 minutes");

            context.Done(state);
        }
    };
}
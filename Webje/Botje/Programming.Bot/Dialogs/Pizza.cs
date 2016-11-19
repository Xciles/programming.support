﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;

namespace Programming.Bot.Dialogs
{
    public enum SizeOptions
    {
        // 0 value in enums is reserved for unknown values.  Either you can supply an explicit one or start enumeration at 1.
        Unknown,
        [Terms(new string[] { "med", "medium" })]
        Medium,
        Large,

        [Terms(new string[] { "family", "extra large" })]
        Family
    };
    public enum PizzaOptions
    {
        Unkown, SignaturePizza, GourmetDelitePizza, StuffedPizza,

        [Terms(new string[] { "byo", "build your own" })]
        [Describe("Build your own")]
        BYOPizza
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
        public SizeOptions Size;
        [Prompt("What kind of pizza do you want? {||}")]
        [Template(TemplateUsage.NotUnderstood, "What does \"{0}\" mean???")]
        [Describe("Kind of pizza")]
        public PizzaOptions Kind;
        public SignatureOptions Signature;
        public GourmetDeliteOptions GourmetDelite;
        public StuffedOptions Stuffed;
        //public BYOPizza BYO;
        public string Address;
        [Optional]
        public CouponOptions Coupon;

        public string OrderId { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendFormat("PizzaOrder({0}, ", Size);
            switch (Kind)
            {
                //case PizzaOptions.BYOPizza:
                //    builder.AppendFormat("{0}, {1}, {2}, [", Kind, BYO.Crust, BYO.Sauce);
                //    foreach (var topping in BYO.Toppings)
                //    {
                //        builder.AppendFormat("{0} ", topping);
                //    }
                //    builder.AppendFormat("]");
                //    break;
                case PizzaOptions.GourmetDelitePizza:
                    builder.AppendFormat("{0}, {1}", Kind, GourmetDelite);
                    break;
                case PizzaOptions.SignaturePizza:
                    builder.AppendFormat("{0}, {1}", Kind, Signature);
                    break;
                case PizzaOptions.StuffedPizza:
                    builder.AppendFormat("{0}, {1}", Kind, Stuffed);
                    break;
            }
            builder.AppendFormat(", {0}, {1})", Address, Coupon);
            return builder.ToString();
        }

        public static IForm<PizzaOrder> BuildForm()
        {
            return new FormBuilder<PizzaOrder>()
                    .Message("Welcome to the pizza order bot!")
                    .OnCompletion(Callback)
                    .Build();
        }

        private static async Task Callback(IDialogContext context, PizzaOrder state)
        {
            // Todo dominos flow
            state.OrderId = "TempOrderId";

            context.Done(state);
        }
    };
}
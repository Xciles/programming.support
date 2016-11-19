using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Programming.Bot.Domain.Dominos
{
    public class PizzaResult
    {
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int MaxQuantity { get; set; }
        public string Description { get; set; }
        public string SeoDescription { get; set; }
        public Largeimage LargeImage { get; set; }
        public Smallimage SmallImage { get; set; }
        public object[] Legends { get; set; }
        public string Template { get; set; }
        public Sizes[] Sizes { get; set; }
        public int PortionCount { get; set; }
        public string PageCode { get; set; }
        public bool PizzaChefEnabled { get; set; }
        public bool HalfnHalfEnabled { get; set; }
        public Recipecomponent[] RecipeComponents { get; set; }
    }

    public class Largeimage
    {
        public string AltText { get; set; }
        public string Url { get; set; }
    }

    public class Smallimage
    {
        public string AltText { get; set; }
        public string Url { get; set; }
    }

    public class Sizes
    {
        public string Size { get; set; }
        public string Name { get; set; }
        public Prices Prices { get; set; }
        public bool Selected { get; set; }
        public string Status { get; set; }
        public Crusts Crusts { get; set; }
        public Sauces Sauces { get; set; }
        public Toppings Toppings { get; set; }
    }

    public class Prices
    {
        public string Pickup { get; set; }
        public string Delivered { get; set; }
    }

    public class Crusts
    {
        public string ComponentType { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public int MaxOfEach { get; set; }
        public Component[] Components { get; set; }
        public bool AllowAdd { get; set; }
        public bool AllowRemove { get; set; }
    }

    public class Component
    {
        public string ComponentCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Image Image { get; set; }
        public string Price { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
    }

    public class Image
    {
        public string AltText { get; set; }
        public string Url { get; set; }
    }

    public class Sauces
    {
        public string ComponentType { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public int MaxOfEach { get; set; }
        public Component1[] Components { get; set; }
        public bool AllowAdd { get; set; }
        public bool AllowRemove { get; set; }
    }

    public class Component1
    {
        public string ComponentCode { get; set; }
        public string Name { get; set; }
        public Image1 Image { get; set; }
        public string Price { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
    }

    public class Image1
    {
        public string AltText { get; set; }
        public string Url { get; set; }
    }

    public class Toppings
    {
        public string ComponentType { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public int MaxOfEach { get; set; }
        public Component2[] Components { get; set; }
        public bool AllowAdd { get; set; }
        public bool AllowRemove { get; set; }
    }

    public class Component2
    {
        public string ComponentCode { get; set; }
        public string Name { get; set; }
        public Image2 Image { get; set; }
        public string Price { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
    }

    public class Image2
    {
        public string AltText { get; set; }
        public string Url { get; set; }
    }

    public class Recipecomponent
    {
        public string ComponentCode { get; set; }
        public int Quantity { get; set; }
    }

}
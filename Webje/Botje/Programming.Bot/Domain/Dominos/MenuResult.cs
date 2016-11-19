using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Programming.Bot.Domain.Dominos
{
    public class MenuResult
    {
        public DateTime MenuFor { get; set; }
        public string CountryCode { get; set; }
        public string Culture { get; set; }
        public int StoreNo { get; set; }
        public bool IsDelivery { get; set; }
        public DateTime StoreTime { get; set; }
        public Menupage[] MenuPages { get; set; }
    }

    public class Menupage
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public Logo Logo { get; set; }
        public Submenu[] SubMenus { get; set; }
        public Legend1[] Legends { get; set; }
        public object[] Disclaimers { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
    }

    public class Logo
    {
        public string AltText { get; set; }
        public string Url { get; set; }
    }

    public class Submenu
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public Product[] Products { get; set; }
        public object[] SubMenus { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
    }

    public class Product
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public Price Price { get; set; }
        public string Status { get; set; }
        public string ComponentStatus { get; set; }
        public Linkeditem LinkedItem { get; set; }
        public Legend[] Legends { get; set; }
        public bool HalfnHalfEnabled { get; set; }
    }

    public class Price
    {
        public string Pickup { get; set; }
        public string Delivered { get; set; }
    }

    public class Linkeditem
    {
        public string ItemType { get; set; }
        public string ItemCode { get; set; }
    }

    public class Legend
    {
        public string Code { get; set; }
        public Image Image { get; set; }
        public string Text { get; set; }
    }

    public class Legend1
    {
        public string Code { get; set; }
        public Image1 Image { get; set; }
        public string Text { get; set; }
    }

}
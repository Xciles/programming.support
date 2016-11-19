using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Programming.Bot.Domain.Dominos
{

    public class OrderRequest
    {
        public string StoreNo { get; set; }
        public string CountryCode { get; set; }
        public string Language { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsCashPayment { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string VendorId { get; set; }
        public Deliverto DeliverTo { get; set; }
        public Product2[] Products { get; set; }
    }

    public class Deliverto
    {
        public string UnitNo { get; set; }
        public string StreetNo { get; set; }
        public string StreetName { get; set; }
        public string Suburb { get; set; }
        public string PostalCode { get; set; }
        public string DeliveryInstruction { get; set; }
    }

    public class Product2
    {
        public string ProductCode { get; set; }
        public string Price { get; set; }
        public string SizeCode { get; set; }
        public string[] Additions { get; set; }
    }


}
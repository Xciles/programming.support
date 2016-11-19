using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Programming.Bot.Domain.Dominos
{
    public class OrderStatusRequest
    {
    public string CountryCode { get; set; }
    public string VendorId { get; set; }
    public string OrderId { get; set; }
    }
}
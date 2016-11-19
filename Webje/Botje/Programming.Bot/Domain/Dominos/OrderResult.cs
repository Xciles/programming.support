using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Programming.Bot.Domain.Dominos
{
    public class OrderResult
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public string OrderId { get; set; }
        public string EstimatedCompleteTimeInMinute { get; set; }
    }

}
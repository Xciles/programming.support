using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Programming.Bot.Domain
{
    public class QueryResult
    {
        public string query { get; set; }
        public Intent topScoringIntent { get; set; }
        public Entity[] entities { get; set; }
    }
}
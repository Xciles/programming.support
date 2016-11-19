using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming.Web.ServiceDomain
{
    public class SoRequest
    {
        public string Question { get; set; }
    }

    public class SoResponse
    {
        public string Body { get; set; }
        public string StrippedBody { get; set; }
        public string Question { get; set; }
    }
}

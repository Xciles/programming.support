using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Programming.Web.Controllers
{
    [Route("api/hyperion")]
    public class HyperionController : Controller
    {
        [HttpPost()]
        public string Get(int id)
        {
            return "value";
        }
    }
}

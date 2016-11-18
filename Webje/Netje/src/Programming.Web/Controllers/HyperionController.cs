using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Programming.Web.Business;
using Programming.Web.ServiceDomain;

namespace Programming.Web.Controllers
{
    [Route("api/hyperion")]
    public class HyperionController : Controller
    {
        [HttpPost("SoQuestion")]

        public async Task<SoResponse> SoQuestion([FromBody]SoRequest query)
        {
            return await StackOverflow.Query(query.Question);
        }

        [HttpPost("Pizzaaaa")]

        public async Task<PizzaaaResponse> Pizzaaaa([FromBody]PizzaaaRequest query)
        {
            return await Task.FromResult(new PizzaaaResponse());
        }
    }
}

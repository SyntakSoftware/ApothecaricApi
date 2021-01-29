using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ApothecaricApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        [HttpGet]
        [Authorize]
        public IEnumerable<string> Get()
        {
            Random r = new Random();
            int val1 = r.Next(0, 100);
            int val2 = r.Next(0, 100);

            return new string[] { "value1 - " + val1.ToString(), " ::  value2 - " + val2.ToString() };
        }

    }
}

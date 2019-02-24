using Hudson.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hudson.SampleServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly CloudContext cloudContext;
        IConfiguration _Configuration = null;
        public HomeController(IConfiguration Configuration, CloudContext cloudContext)
        {
            this.cloudContext = cloudContext;
            _Configuration = Configuration;
        }

        public string get123()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(cloudContext);
        }

        [Route("/get321")]
        [HttpGet]
        public string get321()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(cloudContext);
        }

    }
}

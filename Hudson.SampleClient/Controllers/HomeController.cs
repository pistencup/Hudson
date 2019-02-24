using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hudson.SampleClient.Models;
using Hudson.Core;
using SampleServer.Api;
using SampleApiServer.Api;

namespace Hudson.SampleClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly CloudContext cloudContext;
        private readonly ISample sampleClient;
        private readonly ISampleApi sampleApiClient;
        public HomeController(ISample sampleClient, ISampleApi sampleApiClient, CloudContext cloudContext)
        {
            this.cloudContext = cloudContext;
            this.sampleClient = sampleClient;
            this.sampleApiClient = sampleApiClient;
        }

        public async Task<IActionResult> Index()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            string rst = await sampleClient.Get123();
            dic.Add("sampleClient.Get123()", rst);

            dic.Add("sampleClient.Get321()", await sampleClient.Get321());

            dic.Add("sampleApiClient.Get(\"whosyourdaddy\")", Newtonsoft.Json.JsonConvert.SerializeObject(await sampleApiClient.Get("whosyourdaddy")));

            dic.Add("cloudcontext", Newtonsoft.Json.JsonConvert.SerializeObject(cloudContext));
            

            ViewData["dic"] = dic;
            return View();
        }







        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

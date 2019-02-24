using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hudson.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SampleServer.Api;

namespace Hudson.SampleApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly CloudContext cloudContext;
        private readonly IConfiguration configuration;
        private readonly ISample sampleClient;
        private readonly string baseName;
        public ValuesController(IConfiguration configuration, CloudContext cloudContext, ISample sampleClient)
        {
            this.sampleClient = sampleClient;
            this.cloudContext = cloudContext;
            this.configuration = configuration;
            baseName = $"{configuration.GetValue<string>("spring:application:name")}:{configuration.GetValue<string>("server:port")}";
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get(string arg)
        {
            var remote = await sampleClient.Get321();
            return new string[] {
                $"arg:{ arg }",
                $"remote:{ remote }",
                $"local:{ Newtonsoft.Json.JsonConvert.SerializeObject(cloudContext) }"
            };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return $"{baseName} - Get - id:{id} - {Newtonsoft.Json.JsonConvert.SerializeObject(cloudContext)}";
        }

        // POST api/values
        [HttpPost]
        public ActionResult<string> Post([FromBody] string value)
        {
            return $"{baseName} - Post - value:{value}";
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public ActionResult<string> Put(int id, [FromBody] string value)
        {
            return $"{baseName} - Put - id:{id} - value:{value}";
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public ActionResult<string> Delete(int id)
        {
            return $"{baseName} - Delete - id:{id}";
        }
    }
}

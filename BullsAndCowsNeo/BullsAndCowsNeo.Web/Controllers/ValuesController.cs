using BullsAndCowsNeo.Web.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Neo;
using Neo.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BullsAndCowsNeo.Web.Controllers
{
    public class ValuesController : BaseApiController
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var height = Neo.Core.Blockchain.Default.Height;

            return new string[] { "value1", height.ToString() };
            //return new string[] { "value1" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public async Task Post([FromBody]dynamic obj)
        {
            var value = (string)obj.value;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

using BullsAndCowsNeo.Web.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Neo;
using Neo.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BullsAndCowsNeo.Web.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IHubContext<ChatHub> context;

        public ValuesController(IHubContext<ChatHub> hub)
        {
            context = hub;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            //var height = Neo.Core.Blockchain.Default.Height;

            //return new string[] { "value1", height.ToString() };
            return new string[] { "value1" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public async Task Post([FromBody]string value)
        {
            UInt160 script_hash = UInt160.Parse("0xd2096944344fdeb8b6379d856d8361b1e4596ae0");
            ContractState contract = Blockchain.Default.GetContract(script_hash);
            var parameters = contract.ParameterList;

            await context.Clients.All.SendAsync("ReceiveMessage", "Contract Name : ", contract.Name);
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

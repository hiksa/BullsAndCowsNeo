using BullsAndCowsNeo.ViewModels;
using BullsAndCowsNeo.Web.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Neo;
using Neo.Core;
using Neo.SmartContract;
using Neo.VM;
using System.Linq;
using System.Threading.Tasks;

namespace BullsAndCowsNeo.Web.Controllers
{
    [Route("api/[controller]")]
    public class ContractController : Controller
    {
        private readonly IHubContext<ContractHub> _contractHub;

        public ContractController(IHubContext<ContractHub> contractHub)
        {
            _contractHub = contractHub;
        }

        public async Task<IActionResult> Post([FromBody]ContractBaseViewModel model)
        {
            string result = null;
            UInt160 script_hash = UInt160.Parse("0xd2096944344fdeb8b6379d856d8361b1e4596ae0");
            ContractState contract = Blockchain.Default.GetContract(script_hash);
            var parameters = contract.ParameterList.Select(p => new ContractParameter(p)).ToArray();

            parameters[0].Value = "op1";
            using (ScriptBuilder sb = new ScriptBuilder())
            {
                sb.EmitAppCall(script_hash, parameters);
                result = sb.ToArray().ToHexString();
            }

            await _contractHub.Clients.All.SendAsync("UpdateContractInfo", "Contract hash : ", result);
            return Ok();
        }
    }
}

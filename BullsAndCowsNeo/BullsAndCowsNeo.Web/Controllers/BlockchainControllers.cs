using Microsoft.AspNetCore.Mvc;
using Neo.Core;

namespace BullsAndCowsNeo.Web.Controllers
{
    public class BlockchainController : BaseApiController
    {
        public IActionResult GetHeight()
        {
            return this.Ok(Blockchain.Default.Height);
        }
    }
}

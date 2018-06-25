using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BullsAndCowsNeo.Web.Hubs
{
    public interface IContractHub
    {
        Task UpdateContractInfo(string user, string message);
    }

    public class ContractHub : Hub<IContractHub>
    {
        public async Task UpdateContractInfo(string user, string message)
        {
            var asd = Context.ConnectionId;
            await Clients.All.UpdateContractInfo(user, message);
        }
    }
}

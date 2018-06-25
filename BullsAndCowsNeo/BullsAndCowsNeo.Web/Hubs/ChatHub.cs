using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BullsAndCowsNeo.Web.Hubs
{
    public interface IChatHub
    {
        Task SendMessage(string user, string message);
    }

    public class ChatHub : Hub<IChatHub>
    {
        public async Task SendMessage(string user, string message)
        {
            var asd = Context.ConnectionId;
            var dsa = Clients.Caller;
        }
    }
}

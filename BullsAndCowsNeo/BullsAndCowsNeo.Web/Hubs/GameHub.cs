using BullsAndCowsNeo.Web.Infra;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullsAndCowsNeo.Web.Hubs
{
    public class GameHub : Hub
    {
        private readonly GameEngine _gamesEngine;

        public GameHub(GameEngine gamesEngine)
        {
            _gamesEngine = gamesEngine;
        }

        public async Task Join(string test)
        {
            var gameguid = Guid.NewGuid().ToString();
            if (_gamesEngine.GamesWaitingList.Count > 0)
            {
                var waitingItem = _gamesEngine.GamesWaitingList.FirstOrDefault(kvp => kvp.Key == Context.ConnectionId);
                if (waitingItem.Key != null)
                {
                    gameguid = waitingItem.Value;
                }
                else
                {
                    gameguid = _gamesEngine.GamesWaitingList.First().Value;
                    _gamesEngine.GamesWaitingList.Add(new KeyValuePair<string, string>(Context.ConnectionId, gameguid));
                }
            }
            else
            {
                _gamesEngine.GamesWaitingList.Add(
                    new KeyValuePair<string, string>(Context.ConnectionId, gameguid));
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, gameguid);
            await Clients.All.SendAsync("JoinRoom", gameguid);
        }

        public async Task SetUserNumber(string number)
        {
            if (number.Length > 4)
            {
                await Clients.User(Context.ConnectionId).SendAsync("InvalidNumber", "Length");
            }
            else
            {
                var groupId = _gamesEngine.GetGameIdByConnectionId(Context.ConnectionId);
                var result = _gamesEngine.SetUserNumber(Context.ConnectionId, number);

                await Clients.Group(groupId).SendAsync("NumberValidated", result);
            }
        }


        public async Task MakeAGuess(string number)
        {
            if (number.Length > 4)
            {
                await Clients.User(Context.ConnectionId).SendAsync("InvalidNumber", "Length");
            }
            else
            {
                var groupId = _gamesEngine.GetGameIdByConnectionId(Context.ConnectionId);
                var result = _gamesEngine.MakeAGuess(Context.ConnectionId, number);
                if (groupId != null)
                {
                    await Clients.Group(groupId).SendAsync("TurnResult", result);
                }
            }
        }

        public async Task Leave(string test)
        {
            await Clients.All.SendAsync("LeaveRoom", test);
        }

        public async Task SendMessageInGroup(string group, string message)
        {
            await Clients.Group(group).SendAsync("GroupMessage", message);
        }
    }
}

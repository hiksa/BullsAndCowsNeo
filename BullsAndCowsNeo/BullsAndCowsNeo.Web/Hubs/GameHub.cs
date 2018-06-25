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

        public async Task Join(string address)
        {
            if (_gamesEngine.GamesWaitingList.Count > 0)
            {
                if (_gamesEngine.GamesWaitingList.First().Address == address)
                {
                    return;
                }

                var gameguid = Guid.NewGuid().ToString();
                var game = new Game
                {
                    Id = gameguid,
                    Player1 = _gamesEngine.GamesWaitingList.First(),
                    Player2 = new Player
                    {
                        ConnectionId = Context.ConnectionId,
                        Address = address
                    }
                };

                _gamesEngine.GamesWaitingList.Remove(game.Player1);
                _gamesEngine.GamesList.Add(game);

                await Groups.AddToGroupAsync(game.Player1.ConnectionId, gameguid);
                await Groups.AddToGroupAsync(game.Player2.ConnectionId, gameguid);
                await Clients.All.SendAsync("MatchFound", gameguid);
            }
            else
            {
                _gamesEngine.GamesWaitingList.Add(new Player
                {
                    ConnectionId = Context.ConnectionId,
                    Address = address
                });
            }
        }

        public async Task SendMessageInGroup(string group, string message)
        {
            await Clients.Group(group).SendAsync("GroupMessage", message);
        }
    }
}

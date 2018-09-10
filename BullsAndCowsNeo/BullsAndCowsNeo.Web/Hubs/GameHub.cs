using BullsAndCowsNeo.Common.Constants;
using BullsAndCowsNeo.Web.Infra;
using BullsAndCowsNeo.Web.Infra.Game;
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
        private readonly GameEngine gamesEngine;

        public GameHub(GameEngine gamesEngine)
        {
            this.gamesEngine = gamesEngine;
        }

        public async Task Join(string address)
        {
            if (this.gamesEngine.GamesWaitingList.Any())
            {
                if (this.gamesEngine.GamesWaitingList.First().Player1?.Address == address 
                    || this.gamesEngine.GamesWaitingList.First().Player2?.Address == address)
                {
                    return;
                }

                var game = this.gamesEngine.GamesWaitingList.First();
                var player = new Player
                {
                    ConnectionId = Context.ConnectionId,
                    Address = address
                };

                game.Player2 = player;

                this.gamesEngine.GamesWaitingList.Remove(game);
                this.gamesEngine.GamesList.Add(game);
                await Groups.AddToGroupAsync(game.Player2.ConnectionId, game.Id);
                await Clients.Group(game.Id).SendAsync(ClientSideHandlers.MatchFoundHandler, game.Id);
            }
            else
            {
                var player = new Player
                {
                    ConnectionId = Context.ConnectionId,
                    Address = address
                };

                var gameguid = Guid.NewGuid().ToString();
                var game = new Game
                {
                    Id = gameguid,
                    Player1 = player
                };

                this.gamesEngine.GamesWaitingList.Add(game);
                await Groups.AddToGroupAsync(game.Player1.ConnectionId, game.Id);
                await Clients.Groups(game.Id).SendAsync(ClientSideHandlers.GameCreatedHandler, game.Id);
            }
        }

        public async Task Pick(string address, string number)
        {
            var game = this.gamesEngine
                .GamesList
                .Where(x => !x.HaveSelectedNumbers && x.Player1.Address == address || x.Player2.Address == address)
                .FirstOrDefault();

            if (game != null)
            {
                var player = game.GetPlayerByAddressOrDefault(address);
                player.Number = number;
                player.HasSelectedNumber = true;

                if (game.HaveSelectedNumbers)
                {
                    await this.Clients.Group(game.Id).SendAsync(ClientSideHandlers.WaitingForNextBlockHandler);
                }
                else
                {
                    await this.Clients.Group(game.Id).SendAsync(ClientSideHandlers.WaitingForOpponentHandler);
                }
            }
        }

        public async Task Guess(string address, string number)
        {
            var games = this.gamesEngine.GetActiveGamesByPlayerAddress(address);


        }
    }
}

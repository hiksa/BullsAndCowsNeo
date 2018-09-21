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
        private readonly GameEngine games;

        public GameHub(GameEngine games)
        {
            this.games = games;
        }

        public async Task Join(string address)
        {
            if (this.games.Waiting.Any())
            {
                if (this.games.Waiting.First().Player1?.Address == address 
                    || this.games.Waiting.First().Player2?.Address == address)
                {
                    return;
                }

                var game = this.games.Waiting.First();
                var player = new Player
                {
                    ConnectionId = Context.ConnectionId,
                    Address = address
                };

                game.Player2 = player;

                this.games.Waiting.Remove(game);
                this.games.AllGames.Add(game);
                await Groups.AddToGroupAsync(game.Player2.ConnectionId, game.Id);
                await Clients.Group(game.Id).SendAsync(ClientsideHandlers.MatchFoundHandler, game.Id);
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

                this.games.Waiting.Add(game);
                await Groups.AddToGroupAsync(game.Player1.ConnectionId, game.Id);
                await Clients.Groups(game.Id).SendAsync(ClientsideHandlers.GameCreatedHandler, game.Id);
            }
        }

        public async Task Pick(string address, string number)
        {
            var game = this.games
                .AllGames
                .Where(x => !x.HaveSelectedNumbers && x.Player1.Address == address || x.Player2.Address == address)
                .FirstOrDefault();

            if (game != null)
            {
                var player = game.GetPlayerByAddressOrDefault(address);
                player.Number = number;
                player.HasSelectedNumber = true;

                if (game.HaveSelectedNumbers)
                {
                    await this.Clients.Group(game.Id).SendAsync(ClientsideHandlers.WaitingForNextBlockHandler);
                }
                else
                {
                    await this.Clients.Group(game.Id).SendAsync(ClientsideHandlers.WaitingForOpponentHandler);
                }
            }
        }

        public async Task Guess(string address, string number)
        {
            var games = this.games.GetActiveGamesByPlayerAddress(address);


        }
    }
}

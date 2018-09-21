using BullsAndCowsNeo.Web.Infra.Notifications.Models;
using Neo;
using System.Collections.Generic;
using System.Linq;

namespace BullsAndCowsNeo.Web.Infra.Game
{
    public class GameEngine
    {
        public readonly HashSet<Game> Waiting = new HashSet<Game>();
        public readonly List<Game> AllGames = new List<Game>();

        public IEnumerable<Game> ActiveGames => this.AllGames.Where(x => !x.IsFinished);
        
        public IEnumerable<Game> GetActiveGamesByPlayerAddress(string address) =>
            this.ActiveGames.Where(x => x.Player1.Address == address || x.Player2.Address == address);

        public Game HandleGameJoinedSC(GameJoinedNotification notification)
        {
            var game = this.AllGames.FirstOrDefault(g => g.Id == notification.GameId);
            if (game == null)
            {
                return null;
            }

            var addressHex = notification.Address.ToHexString();
            if (game.Player1.Address == addressHex)
            {
                game.Player1.HasJoined = true;
            }
            else if (game.Player2.Address == addressHex)
            {
                game.Player2.HasJoined = true;
            }

            return game;
        }

        public Game HandleNumberSelectedSC(NumberSelectedNotification notification)
        {
            var game = this.AllGames.FirstOrDefault(g => g.Id == notification.GameId);
            if (game == null)
            {
                return null;
            }

            var addressHex = notification.Address.ToHexString();
            if (game.Player1.Address == addressHex)
            {
                game.Player1.HasSelectedNumber = true;
                game.Player1.Number = notification.Number;
            }
            else if (game.Player2.Address == addressHex)
            {
                game.Player2.HasSelectedNumber = true;
                game.Player2.Number = notification.Number;
            }

            return game;
        }

        public Game HandleNumberGuessedSC(NumberGuessedNotification notification)
        {
            var game = this.AllGames.FirstOrDefault(g => g.Id == notification.GameId);
            if (game == null)
            {
                return null;
            }

            var addressHex = notification.Address.ToHexString();
            if (game.Player1.Address == addressHex && notification.Bulls == 4)
            {
                game.Player1.HasWon = true;
            }
            else if (game.Player2.Address == addressHex && notification.Bulls == 4)
            {
                game.Player2.HasWon = true;
            }            

            return game;
        }
    }
}

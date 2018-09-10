using BullsAndCowsNeo.Web.Infra.Notifications.Models;
using Neo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BullsAndCowsNeo.Web.Infra.Game
{
    public class GameEngine
    {
        public readonly HashSet<Game> GamesWaitingList;
        public readonly List<Game> GamesList;

        public IEnumerable<Game> ActiveGames => this.GamesList.Where(x => !x.IsFinished);

        public GameEngine()
        {
            this.GamesWaitingList = new HashSet<Game>();
            this.GamesList = new List<Game>();
        }

        public IEnumerable<Game> GetActiveGamesByPlayerAddress(string address) =>
            this.ActiveGames.Where(x => x.Player1.Address == address || x.Player2.Address == address);

        public Game HandleGameJoinedSC(GameJoinedNotification notification)
        {
            var addressHex = notification.Address.ToHexString();
            var game = this.GamesList.FirstOrDefault(g => g.Id == notification.GameId);
            if (game == null)
            {
                return null;
            }

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
            var addressHex = notification.Address.ToHexString();
            var game = this.GamesList.FirstOrDefault(g => g.Id == notification.GameId);
            if (game == null)
            {
                return null;
            }

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
            var addressHex = notification.Address.ToHexString();
            var game = this.GamesList.FirstOrDefault(g => g.Id == notification.GameId);
            if (game == null)
            {
                return null;
            }

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

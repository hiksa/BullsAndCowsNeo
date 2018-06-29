using BullsAndCowsNeo.Web.Infra.Notifications.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BullsAndCowsNeo.Web.Infra.Game
{
    public class GameEngine
    {
        public readonly HashSet<Player> GamesWaitingList;
        public readonly List<Game> GamesList;

        public GameEngine()
        {
            this.GamesWaitingList = new HashSet<Player>();
            this.GamesList = new List<Game>();
        }

        public Game HandleGameJoined(GameJoinedNotification notification)
        {
            var game = this.GamesList.FirstOrDefault(g => g.Id == notification.GameId);
            if (game != null)
            {
                if (game.Player1.Address == notification.Address)
                {
                    game.Player1.HasJoined = true;
                }
                else if (game.Player2.Address == notification.Address)
                {
                    game.Player2.HasJoined = true;
                }
            }

            return game;
        }

        public Game HandleNumberSelected(NumberSelectedNotification notification)
        {
            var game = this.GamesList.FirstOrDefault(g => g.Id == notification.GameId);
            if (game != null)
            {

            }

            return game;
        }

        public Game HandleNumberGuessed(NumberGuessedNotification notification)
        {
            var game = this.GamesList.FirstOrDefault(g => g.Id == notification.GameId);
            if (game != null)
            {

            }

            return game;
        }
    }
}

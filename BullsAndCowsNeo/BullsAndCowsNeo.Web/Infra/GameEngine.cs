using BullsAndCowsNeo.Web.Infra.NotificationModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BullsAndCowsNeo.Web.Infra
{
    public class GameEngine
    {
        public HashSet<Player> GamesWaitingList;
        public List<Game> GamesList;

        public GameEngine()
        {
            GamesWaitingList = new HashSet<Player>();
            GamesList = new List<Game>();
        }

        public Game JoinGame(GameJoined gameJoined)
        {
            var game = GamesList.FirstOrDefault(g => g.Id == gameJoined.GameId);
            if (game != null)
            {
                if (game.Player1.Address == gameJoined.Address)
                {
                    game.Player1.HasJoined = true;
                }
                else if (game.Player2.Address == gameJoined.Address)
                {
                    game.Player2.HasJoined = true;
                }
                else
                {

                }
            }
            return game;
        }
    }
}

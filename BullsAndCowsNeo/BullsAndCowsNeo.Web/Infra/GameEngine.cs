using System;
using System.Collections.Generic;
using System.Linq;

namespace BullsAndCowsNeo.Web.Infra
{
    public class GameEngine
    {
        public List<KeyValuePair<string, string>> GamesWaitingList;
        private List<GameModel> GamesList;

        public GameEngine()
        {
            GamesWaitingList = new List<KeyValuePair<string, string>>();
            GamesList = new List<GameModel>();
        }

        public bool SetUserNumber(string connectionId, string number)
        {
            var gameId = GetGameIdByConnectionId(connectionId);
            var game = GamesList.FirstOrDefault(g => g.GameId == gameId);
            if (game != null)
            {
                if (game.Player2 != null || game.Player1 == null)
                {
                    return false;
                }
                else
                {
                    game.Player2 = new GameUserNumber
                    {
                        ConnectionId = connectionId,
                        Number = number
                    };

                    return true;
                }
            }
            else
            {
                var newGame = new GameModel
                {
                    GameId = gameId,
                    Player1 = new GameUserNumber
                    {
                        ConnectionId = connectionId,
                        Number = number
                    }
                };

                GamesList.Add(newGame);
                return true;
            }
        }

        public string MakeAGuess(string connectionId, string guessNumber)
        {
            var gameId = GetGameIdByConnectionId(connectionId);
            var game = GamesList.FirstOrDefault(g => g.GameId == gameId);
            if (game != null)
            {
                if (game.Player1.ConnectionId != connectionId)
                {
                    var guessResult = GetGuessResult(guessNumber, game.Player1.Number);
                    return guessResult;
                }
                var result = GetGuessResult(guessNumber, game.Player1.Number);
                return result;
            }

            return null;
        }

        public string GetGameIdByConnectionId(string connectionId)
        {
            var gameId = GamesWaitingList.FirstOrDefault(kvp => kvp.Key == connectionId);
            if (gameId.Key != null && gameId.Value != null)
            {
                return gameId.Value;
            }
            return null;
        }

        private string GetGuessResult(string guessNumber, string number)
        {
            if (guessNumber == number)
            {
                return "You win";
            }
            return "Make a new guess";
        }
    }
}

using BullsAndCowsNeo.GameContract.Models;
using BullsAndCowsNeo.GameContract.Types;
using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;

namespace BullsAndCowsNeo.GameContract
{
    public class GameLogic : SmartContract
    {
        public static bool JoinGame(byte[] gameId, byte[] address)
        {
            if (!Runtime.CheckWitness(address))
            {
                Runtime.Notify(NotificationTypes.InvalidWitness, address);
                return false;
            }

            var game = GameStorage.GetGame(gameId);
            if (game.State != GameState.WaitingForPlayers)
            {
                Runtime.Notify(NotificationTypes.FailedToJoinGame, gameId, address);
                return false;
            }

            if (game.FirstPlayer.Length == 0)
            {
                game.FirstPlayer = address;
                GameStorage.SaveGame(game);
                Runtime.Notify(NotificationTypes.GameJoined, gameId, address);
                return true;
            }

            if (game.SecondPlayer.Length == 0)
            {
                game.SecondPlayer = address;
                game.State = GameState.SelectingNumbers;
                GameStorage.SaveGame(game);
                Runtime.Notify(NotificationTypes.GameJoined, gameId, address);
                return true;
            }

            Runtime.Notify(NotificationTypes.FailedToJoinGame, gameId, address);
            return false;
        }

        public static bool SelectNumber(byte[] gameId, byte[] address, string number)
        {
            if (!Runtime.CheckWitness(address))
            {
                Runtime.Notify(NotificationTypes.InvalidWitness, address);
                return false;
            }

            if (number.Length != 4)
            {
                Runtime.Notify(NotificationTypes.FailedToSelectNumber, gameId, address, number);
                return false;
            }

            var game = GameStorage.GetGame(gameId);
            if (game.State != GameState.SelectingNumbers)
            {
                Runtime.Notify(NotificationTypes.FailedToSelectNumber, gameId, address, number);
                return false;
            }

            if (game.FirstPlayer == address)
            {
                game.FirstPlayerNumber = number;
                if (game.SecondPlayerNumber != null)
                {
                    game.State = GameState.FirstPlayerTurn;
                }

                GameStorage.SaveGame(game);
                Runtime.Notify(NotificationTypes.NumberSelected, gameId, address, number);
                return true;
            }

            if (game.SecondPlayer == address)
            {
                game.SecondPlayerNumber = number;
                if (game.FirstPlayerNumber != null)
                {
                    game.State = GameState.FirstPlayerTurn;
                }

                GameStorage.SaveGame(game);
                Runtime.Notify(NotificationTypes.NumberSelected, gameId, address, number);
                return true;
            }

            Runtime.Notify(NotificationTypes.FailedToSelectNumber, gameId, address, number);
            return false;
        }

        public static bool GuessNumber(byte[] gameId, byte[] address, string number)
        {
            if (!Runtime.CheckWitness(address))
            {
                Runtime.Notify(NotificationTypes.InvalidWitness, address);
                return false;
            }

            if (number.Length != 4)
            {
                Runtime.Notify(NotificationTypes.FailedToGuessNumber, gameId, address, number);
                return false;
            }

            var game = GameStorage.GetGame(gameId);
            if (game.FirstPlayer == address)
            {
                if (game.State != GameState.FirstPlayerTurn)
                {
                    Runtime.Notify(NotificationTypes.FailedToGuessNumber, gameId, address, number);
                    return false;
                }

                var guessResult = GameLogic.GetGuessResult(game.FirstPlayerNumber, number);
                if (game.SecondPlayerNumber == number)
                {
                    game.State = GameState.FirstPlayerWon;
                    GameStorage.SaveGame(game);
                    Runtime.Notify(NotificationTypes.NumberGuessed, gameId, address, number, guessResult.Bulls, guessResult.Cows);
                    Runtime.Notify(NotificationTypes.GameWon, gameId, address, number);
                }
                else
                {
                    game.State = GameState.SecondPlayerTurn;
                    GameStorage.SaveGame(game);
                    Runtime.Notify(NotificationTypes.NumberGuessed, gameId, address, number, guessResult.Bulls, guessResult.Cows);
                }

                return true;
            }
            else if (game.SecondPlayer == address)
            {
                if (game.State != GameState.SecondPlayerTurn)
                {
                    Runtime.Notify(NotificationTypes.FailedToGuessNumber, gameId, address, number);
                    return false;
                }

                var guessResult = GameLogic.GetGuessResult(game.FirstPlayerNumber, number);
                if (game.FirstPlayerNumber == number)
                {
                    game.State = GameState.SecondPlayerWon;
                    GameStorage.SaveGame(game);
                    Runtime.Notify(NotificationTypes.NumberGuessed, gameId, address, number, guessResult.Bulls, guessResult.Cows);
                    Runtime.Notify(NotificationTypes.GameWon, gameId, address, number);
                }
                else
                {
                    game.State = GameState.FirstPlayerTurn;
                    GameStorage.SaveGame(game);
                    Runtime.Notify(NotificationTypes.NumberGuessed, gameId, address, number, guessResult.Bulls, guessResult.Cows);
                }

                return true;
            }
            else
            {
                Runtime.Notify(NotificationTypes.FailedToGuessNumber, gameId, address, number);
                return false;
            }
        }

        private static GuessResult GetGuessResult(string number, string guess)
        {
            var result = new GuessResult();
            var isGuessVisited = new bool[number.Length];
            var isNumVisited = new bool[number.Length];
            for (int i = 0; i < number.Length; i++)
            {
                if (number[i] == guess[i])
                {
                    result.Bulls += 1;
                    isGuessVisited[i] = true;
                    isNumVisited[i] = true;
                }
            }

            for (int i = 0; i < number.Length; i++)
            {
                for (int j = 0; j < guess.Length; j++)
                {
                    if (i != j && !isNumVisited[j] && !isGuessVisited[i] && guess[i] == number[j])
                    {
                        result.Cows += 1;
                        isGuessVisited[i] = true;
                        isNumVisited[j] = true;
                    }
                }
            }

            return result;
        }
    }
}

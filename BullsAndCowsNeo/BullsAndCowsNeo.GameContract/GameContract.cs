using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using System;
using System.Numerics;

namespace BullsAndCowsNeo.GameContract
{
    public class GameContract : SmartContract
    {
        public static object Main(string operation, params object[] args)
        {            
            if (operation == "join")
            {
                if (args.Length != 2) return false;

                var gameId = (string)args[0];
                var address = (string)args[1];

                return GameLogic.JoinGame(gameId.AsByteArray(), address.AsByteArray());
            }
            else if (operation == "pick")
            {
                if (args.Length != 3) return false;

                var gameId = (string)args[0];
                var address = (string)args[1];
                var number = (string)args[2];

                return GameLogic.SelectNumber(gameId.AsByteArray(), address.AsByteArray(), number);
            }
            else if (operation == "guess")
            {
                if (args.Length != 3) return false;

                var gameId = (string)args[0];
                var address = (string)args[1];
                var number = (string)args[2];
                return GameLogic.GuessNumber(gameId.AsByteArray(), address.AsByteArray(), number);
            }            

            return false;
        }
    }
}

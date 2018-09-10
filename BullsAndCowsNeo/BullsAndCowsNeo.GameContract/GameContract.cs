using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using System;
using System.Numerics;

namespace BullsAndCowsNeo.GameContract
{
    public class GameContract : SmartContract
    {
        public static readonly byte[] ContractOwner = "AcKA1A3TRx6ubNzi3Dz2QFW6V9uEkeVasg".ToScriptHash();

        public static object Main(string operation, params object[] args)
        {
            if (Runtime.Trigger == TriggerType.Verification)
            {
                if (ContractOwner.Length == 20)
                {
                    return Runtime.CheckWitness(ContractOwner);
                }
                else if (ContractOwner.Length == 33)
                {
                    byte[] signature = operation.AsByteArray();
                    return SmartContract.VerifySignature(signature, ContractOwner);
                }
            }
            else if (Runtime.Trigger == TriggerType.VerificationR)
            {
                return true;
            }
            else if (Runtime.Trigger == TriggerType.Application)
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

                    return GameLogic.PickNumber(gameId.AsByteArray(), address.AsByteArray(), number);
                }
                else if (operation == "guess")
                {
                    if (args.Length != 3) return false;

                    var gameId = (string)args[0];
                    var address = (string)args[1];
                    var number = (string)args[2];
                    return GameLogic.GuessNumber(gameId.AsByteArray(), address.AsByteArray(), number);
                }
            }

            return false;
        }
    }
}

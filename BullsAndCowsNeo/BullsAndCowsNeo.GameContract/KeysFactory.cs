using Neo.SmartContract.Framework;

namespace BullsAndCowsNeo.GameContract
{
    public class KeysFactory : SmartContract
    {
        public static byte[] GameId(byte[] gameId) => 
            nameof(GameId).AsByteArray().Concat(gameId);
    }
}

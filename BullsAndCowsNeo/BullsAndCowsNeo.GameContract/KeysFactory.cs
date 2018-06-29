using Neo.SmartContract.Framework;

namespace BullsAndCowsNeo.GameContract
{
    public static class KeysFactory
    {
        public static byte[] GameId(byte[] gameId) => 
            nameof(GameId).AsByteArray().Concat(gameId);
    }
}

using BullsAndCowsNeo.GameContract.Models;
using Neo.SmartContract.Framework.Services.Neo;
using Helper = Neo.SmartContract.Framework.Helper;

namespace BullsAndCowsNeo.GameContract
{
    public static class GameStorage
    {
        public static void SaveGame(Game game)
        {
            var key = KeysFactory.GameId(game.Id);
            var serialized = Helper.Serialize(game);
            Storage.Put(Storage.CurrentContext, key, serialized);
        }

        public static Game GetGame(byte[] gameId)
        {
            var key = KeysFactory.GameId(gameId);
            var rawGame = Storage.Get(Storage.CurrentContext, key);
            var deserialized = (object[])Helper.Deserialize(rawGame);
            if (deserialized.Length == 0)
            {
                return new Game { Id = gameId, State = GameState.WaitingForPlayers };
            }
            else
            {
                return (Game)(object)deserialized;
            }
        }
    }
}

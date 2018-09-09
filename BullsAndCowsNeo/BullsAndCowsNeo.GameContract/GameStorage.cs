using BullsAndCowsNeo.GameContract.Models;
using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using Helper = Neo.SmartContract.Framework.Helper;

namespace BullsAndCowsNeo.GameContract
{
    public class GameStorage : SmartContract
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
            var raw = Storage.Get(Storage.CurrentContext, key);
            if (raw.Length == 0)
            {
                return new Game { Id = gameId, State = GameState.WaitingForPlayers };
            }
            else
            {
                var deserialized = (object[])Helper.Deserialize(raw);
                return (Game)(object)deserialized;
            }
        }
    }
}

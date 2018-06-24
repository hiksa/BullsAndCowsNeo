using System;

namespace BullsAndCowsNeo.GameContract.Models
{
    [Serializable]
    public class Game
    {
        public byte[] Id;

        public byte[] FirstPlayer;

        public byte[] SecondPlayer;

        public string FirstPlayerNumber;

        public string SecondPlayerNumber;

        public GameState State;
    }
}

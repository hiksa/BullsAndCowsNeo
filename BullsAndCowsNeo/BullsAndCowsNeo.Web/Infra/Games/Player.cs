namespace BullsAndCowsNeo.Web.Infra.Game
{
    public class Player
    {
        public string ConnectionId { get; set; }

        public string Address { get; set; }

        public string Number { get; set; }

        public bool HasJoined { get; set; }

        public bool HasSelectedNumber { get; set; }

        public bool HasWon { get; set; }
    }
}

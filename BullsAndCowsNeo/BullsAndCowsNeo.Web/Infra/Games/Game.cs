namespace BullsAndCowsNeo.Web.Infra.Game
{
    public class Game
    {
        public string Id { get; set; }

        public Player Player1 { get; set; }

        public Player Player2 { get; set; }

        public bool IsReady => this.Player1.HasJoined && this.Player2.HasJoined;

        public bool HaveSelectedNumbers => this.Player1.HasSelectedNumber && this.Player2.HasSelectedNumber;

        public bool IsFinished => this.Player1.HasWon || this.Player2.HasWon;

        public Player GetPlayerByAddressOrDefault(string address) =>
            this.Player1.Address == address
                ? this.Player1
                : this.Player2.Address == address
                    ? this.Player2
                    : null;

        public string GetConnectionId(string address, bool opponentConnection = false)
        {
            if (this.Player1.Address == address)
            {
                return opponentConnection ? this.Player2.ConnectionId : this.Player1.ConnectionId;
            }
            else if (this.Player2.Address == address)
            {
                return opponentConnection ? this.Player1.ConnectionId : this.Player2.ConnectionId;
            }

            return null;            
        }

        public string GetOpponentsAddress(string address)
        {
            if (this.Player1.Address == address)
            {
                return this.Player2.Address;
            }
            else if (this.Player2.Address == address)
            {
                return this.Player1.Address;
            }
            else
            {
                return null;
            }
        }
    }
}

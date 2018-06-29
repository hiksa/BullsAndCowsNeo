namespace BullsAndCowsNeo.Web.Infra.Game
{
    public class Game
    {
        public string Id { get; set; }

        public Player Player1 { get; set; }

        public Player Player2 { get; set; }

        public bool IsReady => this.Player1.HasJoined && this.Player2.HasJoined;

        public string GetConnectionId(string address)
        {
            if (this.Player1.Address == address)
            {
                return this.Player1.ConnectionId;
            }
            else if (this.Player2.Address == address)
            {
                return this.Player2.ConnectionId;
            }
            else
            {
                return null;
            }
        }

        public string GetOpponentsConnectionId(string address)
        {
            if (this.Player1.Address == address)
            {
                return this.Player2.ConnectionId;
            }
            else if (this.Player2.Address == address)
            {
                return this.Player1.ConnectionId;
            }
            else
            {
                return null;
            }
        }
    }
}

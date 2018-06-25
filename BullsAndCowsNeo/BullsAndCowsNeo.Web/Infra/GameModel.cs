namespace BullsAndCowsNeo.Web.Infra
{
    public class GameModel
    {
        public string GameId { get; set; }
        public GameUserNumber Player1 { get; set; }
        public GameUserNumber Player2 { get; set; }
    }
}

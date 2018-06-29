namespace BullsAndCowsNeo.Web.Infra.Notifications.Models
{
    public class NumberGuessedNotification
    {
        public string GameId { get; set; }

        public string Address { get; set; }

        public string Number { get; set; }
    }
}

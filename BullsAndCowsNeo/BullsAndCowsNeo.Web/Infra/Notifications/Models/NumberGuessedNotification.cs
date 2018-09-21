using BullsAndCowsNeo.GameContract.Types;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace BullsAndCowsNeo.Web.Infra.Notifications.Models
{
    public class NumberGuessedNotification : INotification
    {
        public string GameId { get; set; }

        public byte[] Address { get; set; }

        public string Number { get; set; }

        public BigInteger Bulls { get; set; }

        public BigInteger Cows { get; set; }

        [NotMapped]
        public string Type => NotificationTypes.NumberGuessed;
    }
}

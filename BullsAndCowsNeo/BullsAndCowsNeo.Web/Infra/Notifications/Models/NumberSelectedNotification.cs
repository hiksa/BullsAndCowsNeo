using BullsAndCowsNeo.GameContract.Types;
using System.ComponentModel.DataAnnotations.Schema;

namespace BullsAndCowsNeo.Web.Infra.Notifications.Models
{
    public class NumberSelectedNotification : INotification
    {
        public string GameId { get; set; }

        public byte[] Address { get; set; }

        public string Number { get; set; }

        [NotMapped]
        public string Type => NotificationTypes.NumberSelected;
    }
}

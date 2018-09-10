using BullsAndCowsNeo.GameContract.Types;
using System.ComponentModel.DataAnnotations.Schema;

namespace BullsAndCowsNeo.Web.Infra.Notifications.Models
{
    public class GameJoinedNotification : INotification
    {
        public string GameId { get; set; }

        public byte[] Address { get; set; }

        [NotMapped]
        public string Type => NotificationTypes.GameJoined;
    }
}

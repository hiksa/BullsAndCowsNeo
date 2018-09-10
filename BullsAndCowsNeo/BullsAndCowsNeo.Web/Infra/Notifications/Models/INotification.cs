using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BullsAndCowsNeo.Web.Infra.Notifications.Models
{
    public interface INotification
    {
        string Type { get; }
    }
}

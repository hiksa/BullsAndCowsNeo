using System.Linq;

using Neo;
using Neo.SmartContract;
using Neo.VM.Types;

using BullsAndCowsNeo.Common;

namespace BullsAndCowsNeo.Web.Infra.Notifications
{
    public static class Extensions
    {
        public static T GetNotification<T>(this NotifyEventArgs args) =>
            (args.State as Array).Skip(1).CreateObject<T>();

        public static string GetNotificationType(this NotifyEventArgs args) =>
            (args.State as Array)[0].GetByteArray().ToHexString().HexStringToString();
    }
}

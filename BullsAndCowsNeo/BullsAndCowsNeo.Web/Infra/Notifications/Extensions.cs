﻿using BullsAndCowsNeo.Common;
using Neo;
using Neo.SmartContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BullsAndCowsNeo.Web.Infra.Notifications
{
    public static class Extensions
    {
        public static T GetNotification<T>(this NotifyEventArgs args) =>
            args.State.GetArray().ToStringList().CreateObject<T>();

        public static string GetNotificationType(this NotifyEventArgs args) =>
            args.State.GetArray()[0].GetByteArray().ToHexString().HexStringToString();
    }
}
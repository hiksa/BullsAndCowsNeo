using BullsAndCowsNeo.Common;
using BullsAndCowsNeo.Dtos;
using BullsAndCowsNeo.GameContract.Types;
using BullsAndCowsNeo.Web.Hubs;
using BullsAndCowsNeo.Web.Infra.NotificationModels;
using Microsoft.AspNetCore.SignalR;
using Neo;
using Neo.Core;
using Neo.Network;
using Neo.SmartContract;
using Neo.VM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace BullsAndCowsNeo.Web.Infra
{
    public class NotificationEngine
    {
        private readonly GameEngine _gameEngine;
        private readonly IHubContext<GameHub> _gameHubContext;

        public NotificationEngine(GameEngine gameEngine, IHubContext<GameHub> gameHubContext)
        {
            _gameEngine = gameEngine;
            _gameHubContext = gameHubContext;
        }

        public async void Init()
        {
            Blockchain.Notify += Blockchain_Notify;
        }

        private async void Blockchain_Notify(object sender, BlockNotifyEventArgs e)
        {
            UInt160 script_hash = UInt160.Parse("0x0031e2299a646b9d53ad644e868d84d5b538e8fa");

            var notifications = e.Notifications.Where(n => n.ScriptHash == script_hash);

            foreach (var item in notifications)
            {
                var notificationType = item.State.GetArray()[0].GetByteArray().ToHexString().HexStringToString();
                if (notificationType == NotificationTypes.GameJoined)
                {
                    var gameJoined = item.State.GetArray().ToStringList().CreateObject<GameJoined>();
                    var game = _gameEngine.JoinGame(gameJoined);
                    var connectionId = game.GetConnectionId(gameJoined.Address);

                    await _gameHubContext.Clients.User(connectionId).SendAsync(notificationType, true);

                    if (game.IsReady)
                    {
                        await _gameHubContext.Clients.Group(gameJoined.GameId).SendAsync(NotificationTypes.GameStarted, true);
                    }
                }
            }

            //var firstNotification = notification.FirstOrDefault();

            //if (firstNotification != null)
            //{
            //    result = string.Empty;
            //    var firstNotificationName = firstNotification.State.GetArray()[0].GetByteArray().ToHexString().HexStringToString();
            //    var stringList = firstNotification.State.GetArray().ToStringList();
            //    var stringsListObject = stringList.CreateObject<MultipleParamsActionObject>();
            //    result = JsonConvert.SerializeObject(stringsListObject);
            //}
        }
    }
}

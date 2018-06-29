using BullsAndCowsNeo.Common;
using BullsAndCowsNeo.Dtos;
using BullsAndCowsNeo.GameContract.Types;
using BullsAndCowsNeo.Web.Hubs;
using BullsAndCowsNeo.Web.Infra.Game;
using BullsAndCowsNeo.Web.Infra.Notifications.Models;
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
using System.Linq.Expressions;
using System.Net;

namespace BullsAndCowsNeo.Web.Infra.Notifications
{
    public class NotificationEngine
    {
        private readonly GameEngine gameEngine;
        private readonly IHubContext<GameHub> gameHubContext;
        private readonly string contractHash = "0x0031e2299a646b9d53ad644e868d84d5b538e8fa";

        public NotificationEngine(GameEngine gameEngine, IHubContext<GameHub> gameHubContext)
        {
            this.gameEngine = gameEngine;
            this.gameHubContext = gameHubContext;
        }

        public async void Init()
        {
            Blockchain.Notify += Blockchain_Notify;
        }

        private async void Blockchain_Notify(object sender, BlockNotifyEventArgs e)
        {
            var scriptHash = UInt160.Parse(this.contractHash);
            var notifications = e.Notifications.Where(n => n.ScriptHash == scriptHash);
            foreach (var item in notifications)
            {
                var notificationType = item.GetNotificationType();
                if (notificationType == NotificationTypes.GameJoined)
                {
                    var notification = item.GetNotification<GameJoinedNotification>();
                    var game = this.gameEngine.HandleGameJoined(notification);
                    var connectionId = game.GetConnectionId(notification.Address);

                    await this.gameHubContext.Clients.User(connectionId).SendAsync(notificationType, true);

                    if (game.IsReady)
                    {
                        await this.gameHubContext.Clients.Group(notification.GameId).SendAsync(NotificationTypes.GameStarted, true);
                    }
                }
                else if (notificationType == NotificationTypes.NumberPicked)
                {
                    var notification = item.GetNotification<NumberSelectedNotification>();
                    var game = this.gameEngine.HandleNumberSelected(notification);
                    var connectionId = game.GetConnectionId(notification.Address);

                    await this.gameHubContext.Clients.User(connectionId).SendAsync(notificationType, true);

                    if (game.HaveSelectedNumbers)
                    {
                        await this.gameHubContext.Clients.Group(notification.GameId).SendAsync("Player turn", game.Player1.Address);
                    }
                }
                else if (notificationType == NotificationTypes.NumberGuessed)
                {
                    var notification = item.GetNotification<NumberGuessedNotification>();
                    var game = this.gameEngine.HandleNumberGuessed(notification);
                    var connectionId = game.GetConnectionId(notification.Address);
                    var opponentsConnectionId = game.GetOpponentsConnectionId(notification.Address);

                    await this.gameHubContext.Clients.Group(notification.GameId).SendAsync(notificationType, notification);

                    if (game.IsFinished)
                    {
                        await this.gameHubContext.Clients.Group(notification.GameId).SendAsync("Player won", notification.Address);
                    }
                    else
                    {
                        await this.gameHubContext.Clients.Group(notification.GameId).SendAsync("Player turn", game.GetOpponentsAddress(notification.Address));
                    }
                }
            }
        }
    }
}

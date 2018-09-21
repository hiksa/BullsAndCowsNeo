using BullsAndCowsNeo.Common.Constants;
using BullsAndCowsNeo.GameContract.Types;
using BullsAndCowsNeo.Web.Hubs;
using BullsAndCowsNeo.Web.Infra.Game;
using BullsAndCowsNeo.Web.Infra.Notifications.Models;
using Microsoft.AspNetCore.SignalR;
using Neo;
using Neo.Implementations.Blockchains.LevelDB;
using Neo.SmartContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BullsAndCowsNeo.Web.Infra.Notifications
{
    public class NotificationsHandler : IDisposable
    {
        private readonly GameEngine gameEngine;
        private readonly IHubContext<GameHub> gameHubContext;
        private readonly UInt160 contractHash = UInt160.Parse("0x17a796498ea0abbb5c1d007c72a7f62826fe4560");

        public NotificationsHandler(GameEngine gameEngine, IHubContext<GameHub> gameHubContext)
        {
            this.gameEngine = gameEngine;
            this.gameHubContext = gameHubContext;
        }

        public void Init() => LevelDBBlockchain.ApplicationExecuted += this.HandleNotifications;        

        private async void HandleNotifications(object sender, ApplicationExecutedEventArgs e)
        {
            var gameContractExecutions = e.ExecutionResults
                .Where(x => x.Notifications.Any(n => n.ScriptHash == this.contractHash))
                .ToList();

            foreach (var item in gameContractExecutions)
            {
                await this.ProcessNotifications(item.Notifications);
            }
        }

        private IClientProxy GetGroup(string name) => this.gameHubContext.Clients.Group(name);

        private IClientProxy GetClient(string connectionId) => this.gameHubContext.Clients.User(connectionId);
        
        private async Task ProcessNotifications(IEnumerable<NotifyEventArgs> args)
        {
            foreach (var item in args)
            {
                var notificationType = item.GetNotificationType();
                if (notificationType == NotificationTypes.GameJoined)
                {
                    var notification = item.GetNotification<GameJoinedNotification>();
                    var game = this.gameEngine.HandleGameJoinedSC(notification);
                    if (game != null && game.IsReady)
                    {
                        await this.GetGroup(notification.GameId).SendAsync(ClientsideHandlers.MatchFoundBlockchainHandler);
                    }
                }
                else if (notificationType == NotificationTypes.FailedToJoinGame)
                {
                    // TODO: Handle failed join attempt
                }
                else if (notificationType == NotificationTypes.NumberSelected)
                {
                    var notification = item.GetNotification<NumberSelectedNotification>();
                    var game = this.gameEngine.HandleNumberSelectedSC(notification);
                    if (game != null && game.HaveSelectedNumbers)
                    {
                        await this.GetGroup(notification.GameId).SendAsync(ClientsideHandlers.BothNumbersSelectedBlockchainHandler);
                    }
                }
                else if (notificationType == NotificationTypes.NumberGuessed)
                {
                    var notification = item.GetNotification<NumberGuessedNotification>();
                    var game = this.gameEngine.HandleNumberGuessedSC(notification);
                    if (game == null)
                    {
                        return;
                    }

                    var connectionId = game.GetConnectionId(notification.Address.ToHexString());
                    var opponentsConnectionId = game.GetConnectionId(notification.Address.ToHexString(), true);
                    if (game.IsFinished)
                    {
                        await this.GetGroup(notification.GameId).SendAsync(ClientsideHandlers.GameWon, notification.Address);
                    }
                    else
                    {
                        await this.GetGroup(notification.GameId).SendAsync(NotificationTypes.NumberGuessed, notification);
                    }
                }
            }
        }

        public void Dispose()
        {
            LevelDBBlockchain.ApplicationExecuted -= this.HandleNotifications;
        }
    }
}

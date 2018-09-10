using BullsAndCowsNeo.Common;
using BullsAndCowsNeo.Common.Constants;
using BullsAndCowsNeo.Dtos;
using BullsAndCowsNeo.GameContract.Types;
using BullsAndCowsNeo.Web.Hubs;
using BullsAndCowsNeo.Web.Infra.Game;
using BullsAndCowsNeo.Web.Infra.Notifications.Models;
using Microsoft.AspNetCore.SignalR;
using Neo;
using Neo.Core;
using Neo.Implementations.Blockchains.LevelDB;
using Neo.SmartContract;
using Neo.VM.Types;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BullsAndCowsNeo.Web.Infra.Notifications
{
    public class NotificationEngine
    {
        private readonly GameEngine gameEngine;
        private readonly IHubContext<GameHub> gameHubContext;
        private readonly string contractHash = "0xb789a95d2302068caf1e553f72301fa0f65a0412";

        public NotificationEngine(GameEngine gameEngine, IHubContext<GameHub> gameHubContext)
        {
            this.gameEngine = gameEngine;
            this.gameHubContext = gameHubContext;
        }

        public void Init()
        {
            LevelDBBlockchain.ApplicationExecuted += LevelDBBlockchain_ApplicationExecuted;
            Blockchain.PersistCompleted += Blockchain_PersistCompleted;
        }

        private void Blockchain_PersistCompleted(object sender, Block e)
        {
            var a = e.Index;
        }

        private async void LevelDBBlockchain_ApplicationExecuted(object sender, ApplicationExecutedEventArgs e)
        {
            foreach (var result in e.ExecutionResults)
            {
                var notifications = result.Notifications.Where(x => x.State is Array).ToList();
                await this.ProcessNotifications(notifications);
            }
        }

        private IClientProxy GetGroup(string name) => this.gameHubContext.Clients.Group(name);

        private IClientProxy GetClient(string connectionId) => this.gameHubContext.Clients.User(connectionId);
        
        private async Task ProcessNotifications(IEnumerable<NotifyEventArgs> notifications)
        {
            foreach (var item in notifications)
            {
                var notificationType = item.GetNotificationType();
                if (notificationType == NotificationTypes.GameJoined)
                {
                    var notification = item.GetNotification<GameJoinedNotification>();
                    await this.ProcessGameJoinedNotification(notification);
                }
                else if (notificationType == NotificationTypes.FailedToJoinGame)
                {

                }
                else if (notificationType == NotificationTypes.NumberPicked)
                {
                    var notification = item.GetNotification<NumberSelectedNotification>();
                    await this.ProcessNumberPickedNotification(notification);
                }
                else if (notificationType == NotificationTypes.NumberGuessed)
                {
                    var notification = item.GetNotification<NumberGuessedNotification>();
                    await this.ProcessNumberGuessedNotification(notification);
                }
            }
        }
        private async Task ProcessGameJoinedNotification(GameJoinedNotification notification)
        {
            var game = this.gameEngine.HandleGameJoinedSC(notification);
            if (game != null && game.IsReady)
            {
                await this.GetGroup(notification.GameId).SendAsync(ClientSideHandlers.MatchFoundBlockchainHandler);
            }      
        }

        private async Task ProcessNumberPickedNotification(NumberSelectedNotification notification)
        {
            var game = this.gameEngine.HandleNumberSelectedSC(notification);
            if (game != null && game.HaveSelectedNumbers)
            {
                await this.GetGroup(notification.GameId).SendAsync(ClientSideHandlers.BothNumbersPickedBlockchainHandler);
            }     
        }

        private async Task ProcessNumberGuessedNotification(NumberGuessedNotification notification)
        {
            var game = this.gameEngine.HandleNumberGuessedSC(notification);
            if (game == null)
            {
                return;
            }

            var connectionId = game.GetConnectionId(notification.Address.ToHexString());
            var opponentsConnectionId = game.GetOpponentsConnectionId(notification.Address.ToHexString());

            if (game.IsFinished)
            {
                await this.GetGroup(notification.GameId).SendAsync("Player won", notification.Address);
            }
            else
            {
                await this.GetGroup(notification.GameId).SendAsync(NotificationTypes.NumberGuessed, notification);
            }
        }
    }
}

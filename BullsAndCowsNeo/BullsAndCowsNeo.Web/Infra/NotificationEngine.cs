using BullsAndCowsNeo.Common;
using BullsAndCowsNeo.Dtos;
using BullsAndCowsNeo.Web.Hubs;
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
        private readonly IHubContext<ChatHub> _chatHub;
        private readonly IHubContext<ContractHub> _contractHub;
        private List<IPEndPoint> _allNodes = new List<IPEndPoint>();

        public NotificationEngine(IHubContext<ChatHub> hubContext, IHubContext<ContractHub> contractHub)
        {
            _chatHub = hubContext;
            _contractHub = contractHub;
        }

        public async void Init()
        {
            //LocalNode.InventoryReceived += LocalNode_InventoryReceived;
            //LocalNode.InventoryReceiving += LocalNode_InventoryReceiving;
            //Blockchain.Notify += Blockchain_Notify;
            //Blockchain.PersistCompleted += UpdateBlockCount_Completed;
            //Blockchain.PersistCompleted += UpdateContractValue_Completed;
        }

        private async void LocalNode_InventoryReceiving(object sender, InventoryReceivingEventArgs e)
        {
            var badpeers = Startup.localNode.GetBadPeers();
            var nodes = Startup.localNode.GetRemoteNodes();
            var unconnected = Startup.localNode.GetUnconnectedPeers();
            foreach (var node in nodes)
            {
                var existingNode = _allNodes.FirstOrDefault(n => n.Address == node.RemoteEndpoint.Address && n.Port == node.RemoteEndpoint.Port);
                if (existingNode == null)
                {
                    _allNodes.Add(node.RemoteEndpoint);
                }
            }

            await _chatHub.Clients.All.SendAsync("ReceiveMessage", "[SYSTEM] : ", $"{DateTime.Now.ToString("hh:ss")} -> InventoryReceivingEventArgs : {e.ToString()}");
        }

        private async void LocalNode_InventoryReceived(object sender, IInventory e)
        {
            await _chatHub.Clients.All.SendAsync("ReceiveMessage", "[SYSTEM] : ", $"{DateTime.Now.ToString("hh:ss")} -> IInventory {e.ToString()}");
        }

        private async void Blockchain_Notify(object sender, BlockNotifyEventArgs e)
        {
            string result = "no result";

            UInt160 script_hash = UInt160.Parse("0x7056c04071babb493c0e7a1eb24fd28207300ddf");
            var notifications = e.Notifications;
            var notification = notifications.Where(n => n.ScriptHash == script_hash);
            var firstNotification = notification.FirstOrDefault();

            if (firstNotification != null)
            {
                result = string.Empty;
                var firstNotificationName = firstNotification.State.GetArray()[0].GetByteArray().ToHexString().HexStringToString();
                var stringList = firstNotification.State.GetArray().ToStringList();
                var stringsListObject = stringList.CreateObject<MultipleParamsActionObject>();
                result = JsonConvert.SerializeObject(stringsListObject);
            }

            await _contractHub.Clients.All.SendAsync("UpdateContractInfo", "Contract result : ", result.Trim());
        }

        private async void UpdateBlockCount_Completed(object sender, Block e)
        {
            await _chatHub.Clients.All.SendAsync("ReceiveMessage", "[SYSTEM] : ", $"{DateTime.Now.ToString("hh:ss")} -> ${Blockchain.Default.Height}");
        }
    }
}

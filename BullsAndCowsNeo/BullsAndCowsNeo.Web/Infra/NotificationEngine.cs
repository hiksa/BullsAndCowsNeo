using BullsAndCowsNeo.Common;
using BullsAndCowsNeo.Web.Hubs;
using Microsoft.AspNetCore.SignalR;
using Neo;
using Neo.Core;
using Neo.SmartContract;
using Neo.VM;
using System;
using System.Linq;

namespace BullsAndCowsNeo.Web.Infra
{
    public class NotificationEngine
    {
        private readonly IHubContext<ChatHub> _chatHub;
        private readonly IHubContext<ContractHub> _contractHub;

        public NotificationEngine(IHubContext<ChatHub> hubContext, IHubContext<ContractHub> contractHub)
        {
            _chatHub = hubContext;
            _contractHub = contractHub;
        }

        public async void Init()
        {
            Blockchain.Notify += Blockchain_Notify;
            Blockchain.PersistCompleted += UpdateBlockCount_Completed;
            //Blockchain.PersistCompleted += UpdateContractValue_Completed;
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

                foreach (var item in firstNotification.State.GetArray())
                {
                    var text = item.GetByteArray().ToHexString().HexStringToString();
                    if (text != firstNotificationName)
                    {
                        result += $"[{text}] ";
                    }
                }
            }

            await _contractHub.Clients.All.SendAsync("UpdateContractInfo", "Contract result : ", result.Trim());
        }

        private async void UpdateContractValue_Completed(object sender, Block e)
        {
            //string result = null;
            //UInt160 script_hash = UInt160.Parse("0xc89c876fdeebe661686e816658d124808d84688e");
            //ContractState contract = Blockchain.Default.GetContract(script_hash);
            //var parameters = contract.ParameterList.Select(p => new ContractParameter(p)).ToArray();

            //parameters[0].Value = "get";
            //parameters[1].Value = "op1";
            //using (ScriptBuilder sb = new ScriptBuilder())
            //{
            //    sb.EmitAppCall(script_hash, parameters);
            //    result = sb.ToArray().ToHexString();
            //}

            //await _contractHub.Clients.All.SendAsync("UpdateContractInfo", "Contract result : ", result);
        }

        private async void UpdateBlockCount_Completed(object sender, Block e)
        {
            await _chatHub.Clients.All.SendAsync("ReceiveMessage", "[SYSTEM] : ", $"{DateTime.Now.ToString("hh:ss")} -> ${Blockchain.Default.Height}");
        }
    }
}

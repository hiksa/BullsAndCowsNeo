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
            Blockchain.PersistCompleted += UpdateContractValue_Completed;
        }

        private void Blockchain_Notify(object sender, BlockNotifyEventArgs e)
        {
            var notifications = e.Notifications;
        }

        private async void UpdateContractValue_Completed(object sender, Block e)
        {
            string result = null;
            UInt160 script_hash = UInt160.Parse("0x27e6fd6a60288c0392bd108a9d7cd4358f4e22ef");
            ContractState contract = Blockchain.Default.GetContract(script_hash);
            var parameters = contract.ParameterList.Select(p => new ContractParameter(p)).ToArray();

            parameters[0].Value = "op1";
            using (ScriptBuilder sb = new ScriptBuilder())
            {
                sb.EmitAppCall(script_hash, parameters);
                result = sb.ToArray().ToHexString();
            }

            await _contractHub.Clients.All.SendAsync("UpdateContractInfo", "Contract result : ", result);
        }

        private async void UpdateBlockCount_Completed(object sender, Block e)
        {
            await _chatHub.Clients.All.SendAsync("ReceiveMessage", "[SYSTEM] : ", $"{DateTime.Now.ToString("hh:ss")} -> ${Blockchain.Default.Height}");
        }
    }
}

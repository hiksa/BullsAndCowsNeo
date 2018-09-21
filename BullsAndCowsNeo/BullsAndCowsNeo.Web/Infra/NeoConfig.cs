using System.Threading.Tasks;

using Neo.Core;
using Neo.Implementations.Blockchains.LevelDB;
using Neo.Network;

namespace BullsAndCowsNeo.Web.Infra
{
    public class NeoConfig
    {
        private static LocalNode localNode = new LocalNode();

        public void Init()
        {
            Blockchain.RegisterBlockchain(new LevelDBBlockchain(NeoSettings.Default.DataDirectoryPath));

            Task.Run(() => localNode.Start(NeoSettings.Default.NodePort, NeoSettings.Default.WsPort));
        }
    }
}

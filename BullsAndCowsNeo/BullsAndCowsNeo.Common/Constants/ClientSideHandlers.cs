using System;
using System.Collections.Generic;
using System.Text;

namespace BullsAndCowsNeo.Common.Constants
{
    public class ClientSideHandlers
    {
        public const string WaitingForNextBlockHandler = nameof(WaitingForNextBlockHandler);
        public const string WaitingForOpponentHandler = nameof(WaitingForOpponentHandler);

        public const string GameCreatedHandler = "GameCreatedHandler";
        public const string GameCreatedBlockchainHandler = nameof(GameCreatedBlockchainHandler);
        public const string MatchFoundHandler = nameof(MatchFoundHandler);
        public const string MatchFoundBlockchainHandler = nameof(MatchFoundBlockchainHandler);

        public const string OpponentNumberPickedHandler = nameof(OpponentNumberPickedHandler);
        public const string OpponentNumberPickedBlockchainHandler = nameof(OpponentNumberPickedBlockchainHandler);
        public const string OwnNumberPickedHandler = nameof(OwnNumberPickedHandler);
        public const string OwnNumberPickedBlockchainHandler = nameof(OwnNumberPickedBlockchainHandler);
        public const string BothNumbersPickedHandler = nameof(BothNumbersPickedHandler);
        public const string BothNumbersPickedBlockchainHandler = nameof(BothNumbersPickedBlockchainHandler);
    }
}

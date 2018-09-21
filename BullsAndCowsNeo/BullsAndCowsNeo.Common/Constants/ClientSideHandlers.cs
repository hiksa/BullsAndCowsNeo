
namespace BullsAndCowsNeo.Common.Constants
{
    public class ClientsideHandlers
    {
        public const string WaitingForNextBlockHandler = nameof(WaitingForNextBlockHandler);
        public const string WaitingForOpponentHandler = nameof(WaitingForOpponentHandler);

        public const string GameCreatedHandler = nameof(GameCreatedHandler);
        public const string GameCreatedBlockchainHandler = nameof(GameCreatedBlockchainHandler);
        public const string MatchFoundHandler = nameof(MatchFoundHandler);
        public const string MatchFoundBlockchainHandler = nameof(MatchFoundBlockchainHandler);

        public const string OpponentNumberPickedHandler = nameof(OpponentNumberPickedHandler);
        public const string OpponentNumberPickedBlockchainHandler = nameof(OpponentNumberPickedBlockchainHandler);
        public const string OwnNumberPickedHandler = nameof(OwnNumberPickedHandler);
        public const string OwnNumberPickedBlockchainHandler = nameof(OwnNumberPickedBlockchainHandler);
        public const string BothNumbersSelectedHandler = nameof(BothNumbersSelectedHandler);
        public const string BothNumbersSelectedBlockchainHandler = nameof(BothNumbersSelectedBlockchainHandler);

        public const string GameWon = nameof(GameWon);
    }
}

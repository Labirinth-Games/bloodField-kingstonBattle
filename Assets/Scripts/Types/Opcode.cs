namespace BloodField.Types
{
    static class OpCodeType
    {
        // In Game
        public const long GAME_START = 11;
        public const long GAME_PAUSE = 12;

        // Deck
        public const long DECK_DRAW_CARD = 21;
        public const long DECK_RECEIVE_CARDS = 22;

        // Turn
        public const long NEW_TURN = 31;
        public const long TURN_PLAYERS = 32;
        public const long TURN_PHASE_PREPARATION_READY = 33;
        public const long TURN_CARD_USED = 34;

        // Miniatures
        public const long MINIATURE_MOVE = 41;
        public const long MINIATURE_ATTACK = 42;
        public const long MINIATURE_CREATE = 43;
        public const long MINIATURE_HIT = 44;
        public const long MINIATURE_DEATH = 45;

        // match
        public const long MATCH_STATE = 51;
        public const long MATCH_LOAD = 52;
        public const long MATCH_FINISH = 53;

    }
}
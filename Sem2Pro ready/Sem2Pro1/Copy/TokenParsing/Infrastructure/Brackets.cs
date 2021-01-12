using System;
using System.Collections.Generic;

namespace TokenParsing.Infrastructure
{
    public static class Brackets
    {
        private static Dictionary<BracketType, string> BracketsByType { get; }
            = new Dictionary<BracketType, string>
            {
                [BracketType.Round] = "()",
                [BracketType.Square] = "[]",
                [BracketType.Curly] = "{}"
            };

        private static Dictionary<char, BracketType> TypeByBracket { get; }
            = new Dictionary<char, BracketType>
            {
                ['('] = BracketType.Round, [')'] = BracketType.Round,
                ['['] = BracketType.Square, [']'] = BracketType.Square,
                ['{'] = BracketType.Curly, ['}'] = BracketType.Curly
            };

        private static Dictionary<char, BracketPosition> PosByBracket { get; }
            = new Dictionary<char, BracketPosition>
            {
                ['('] = BracketPosition.Opening, ['['] = BracketPosition.Opening, ['{'] = BracketPosition.Opening,
                [')'] = BracketPosition.Closing, [']'] = BracketPosition.Closing, ['}'] = BracketPosition.Closing
            };

        public static char GetBracket(BracketType type, BracketPosition position)
        {
            var bracketsOfType = BracketsByType[type];
            return position == BracketPosition.Opening ? bracketsOfType[0] : bracketsOfType[1];
        }

        public static bool TryGetBracketInfo(char bracket, out (BracketType type, BracketPosition position) info)
        {
            if (!TypeByBracket.TryGetValue(bracket, out var type))
            {
                info = default;
                return false;
            }

            info = (type, PosByBracket[bracket]);
            return true;
        }

        public static bool AreMatching(char possibleOpening, char possibleClosing)
        {
            if (!TryGetBracketInfo(possibleOpening, out var openingInfo)
                || !TryGetBracketInfo(possibleClosing, out var closingInfo))
            {
                return false;
            }

            return openingInfo.type == closingInfo.type
                   && openingInfo.position == BracketPosition.Opening
                   && closingInfo.position == BracketPosition.Closing;
        }
    }
}
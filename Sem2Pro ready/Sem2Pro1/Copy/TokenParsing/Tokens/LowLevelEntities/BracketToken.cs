namespace TokenParsing
{
    public class BracketToken : IToken
    {
        public static BasePriority BracketDelta => BasePriority.BracketDelta;

        public int Priority { get; }

        public BracketType Type { get; }
        public BracketPosition Position { get; }

        public BracketToken(BracketType type, BracketPosition position, int bracketPriority)
        {
            Type = type;
            Position = position;
            Priority = bracketPriority + (
                           position == BracketPosition.Opening ? (int)BracketDelta : 0
                       );
        }

        public void Parse(TokenToExprParser parser, int startInd, out int endInd)
        {
            //parser.ParseFrom(startInd, Priority, out endInd); // IF ...
            parser.ParseBracket(startInd, Type, out endInd);
        }
    }
}
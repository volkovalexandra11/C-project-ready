namespace TokenParsing
{
    public class AbsToken : IToken
    {
        public static BasePriority PriorityDelta = BasePriority.AbsDelta;

        public int BracketPriority { get; }

        public BracketPosition Position { get; }

        public AbsToken(BracketPosition position, int bracketPriority)
        {
            Position = position;
            BracketPriority = bracketPriority + (
                                  position == BracketPosition.Opening ? (int)PriorityDelta : 0
                              );
        }

        public void Parse(TokenToExprParser parser, int startInd, out int endInd)
        {
            throw new System.NotImplementedException();
        }
    }
}
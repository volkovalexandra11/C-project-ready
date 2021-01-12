namespace TokenParsing
{
    public interface IToken
    {
        void Parse(TokenToExprParser parser, int startInd, out int endInd);
    }
}
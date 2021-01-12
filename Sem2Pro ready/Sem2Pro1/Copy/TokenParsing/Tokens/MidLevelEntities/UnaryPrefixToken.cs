using System.Linq.Expressions;

namespace TokenParsing.Tokens
{
    public abstract class UnaryPrefixToken : IToken
    {
        public abstract Expression MakeExpression(Expression operand);

        public void Parse(TokenToExprParser parser, int startInd, out int endInd)
        {
            parser.ParseFrom(startInd + 1, (int)BasePriority.Argument, out endInd);
            var operand = parser.ExpressionStack.Pop();
            parser.ExpressionStack.Push(MakeExpression(operand));
        }
    }
}
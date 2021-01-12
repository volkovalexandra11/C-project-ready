using System;
using System.Linq.Expressions;

namespace TokenParsing
{
    public abstract class BinaryInfixToken : IToken, IHaveBasePriority
    {
        public abstract BasePriority BasePriority { get; }

        public int Priority { get; protected set; }

        public abstract Expression MakeExpression(Expression left, Expression right);

        public void Parse(TokenToExprParser parser, int startInd, out int endInd)
        {
            parser.ParseFrom(startInd + 1, Priority, out endInd);
            var (rightArg, leftArg) = (parser.ExpressionStack.Pop(), parser.ExpressionStack.Pop());
            parser.ExpressionStack.Push(MakeExpression(leftArg, rightArg));
        }
    }
}
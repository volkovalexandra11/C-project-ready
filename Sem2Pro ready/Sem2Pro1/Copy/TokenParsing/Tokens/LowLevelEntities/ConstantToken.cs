using System;
using System.Linq.Expressions;

namespace TokenParsing
{
    public class ConstantToken : IToken
    {
        public double Value { get; }

        public ConstantToken(double value) => Value = value;

        public void Parse(TokenToExprParser parser, int startInd, out int endInd)
        {
            var constExpr = Expression.Constant(Value);
            parser.ExpressionStack.Push(constExpr);
            endInd = startInd + 1;
        }
    }
}
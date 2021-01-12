using System;
using System.Linq.Expressions;

namespace TokenParsing
{
    public class ParameterToken : IToken
    {
        public ParameterExpression Parameter { get; }

        public ParameterToken(ParameterExpression parameter) => Parameter = parameter;

        public void Parse(TokenToExprParser parser, int startInd, out int endInd)
        {
            parser.ExpressionStack.Push(Parameter);
            endInd = startInd + 1;
        }
    }
}
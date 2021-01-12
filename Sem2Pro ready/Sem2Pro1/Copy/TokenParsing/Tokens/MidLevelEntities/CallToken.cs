using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace TokenParsing.Tokens.MidLevelEntities
{
    class CallToken : IToken
    {
        public BasePriority ArgumentDelta => BasePriority.Argument;

        public MethodInfo Method { get; }
        public int Priority { get; }
        public int ArgCount { get; }

        public CallToken(MethodInfo method, int bracketPriority, int argCount = 1)
        {
            Method = method;
            Priority = bracketPriority + (int)ArgumentDelta;
            ArgCount = argCount;
        }

        public void Parse(TokenToExprParser parser, int startInd, out int endInd)
        {
            if (ArgCount != 1)
                throw new NotImplementedException();

            parser.ParseFrom(startInd + 1, Priority, out endInd);

            var arguments = new List<Expression>(ArgCount);
            for (var argInd = 0; argInd < ArgCount; argInd++)
            {
                arguments.Add(parser.ExpressionStack.Pop());
            }
            arguments.Reverse();

            parser.ExpressionStack.Push(Expression.Call(Method, arguments));
        }
    }
}

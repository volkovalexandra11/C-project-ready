using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructure.TopDowns
{
    public class PowerParser : BinaryInfixParser
    {
        public override ParserOrder Order { get; } = ParserOrder.Power;
        protected override string Symbol { get; } = "^";

        private static MethodInfo PowerMethod { get; }
            = typeof(Math).GetMethod("Pow");

        public PowerParser(ParserCombinator combinator) : base(combinator)
        {
        }

        protected override Expression GetExpression(Expression[] arguments)
        {
            var reversedArgs = arguments.Reverse().ToArray();
            return reversedArgs
                .Skip(2)
                .Aggregate(
                    Expression.Call(PowerMethod, reversedArgs[1], reversedArgs[0]),
                    (lastPowers, prevArg) => Expression.Call(PowerMethod, prevArg, lastPowers)
                );
        }
    }
}
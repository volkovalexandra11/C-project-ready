using System;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.TopDowns
{
    public class MultiplicationParser : BinaryInfixParser
    {
        public override ParserOrder Order { get; } = ParserOrder.Multiplication;
        protected override string Symbol { get; } = "*";

        public MultiplicationParser(Lazy<ParserCombinator> lazyCombinator)
            : base(lazyCombinator)
        {
        }

        protected override Expression GetExpression(Expression[] arguments)
        {
            return arguments
                .Skip(2)
                .Aggregate(
                    Expression.Multiply(arguments[0], arguments[1]),
                    Expression.Multiply
                );
        }
    }
}
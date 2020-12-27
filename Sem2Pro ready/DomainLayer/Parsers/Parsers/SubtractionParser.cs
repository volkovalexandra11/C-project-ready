using System;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.TopDowns
{
    public class SubtractionParser : BinaryInfixParser
    {
        public override ParserOrder Order { get; } = ParserOrder.Subtraction;
        protected override string Symbol { get; } = "-";

        public SubtractionParser(Lazy<ParserCombinator> lazyCombinator)
            : base(lazyCombinator)
        {
        }

        protected override Expression GetExpression(Expression[] arguments)
        {
            return arguments
                .Skip(2)
                .Aggregate(
                    Expression.Subtract(arguments[0], arguments[1]),
                    Expression.Subtract
                );
        }
    }
}
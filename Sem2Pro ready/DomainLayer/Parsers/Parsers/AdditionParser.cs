using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.TopDowns
{
    public class AdditionParser : BinaryInfixParser
    {
        public override ParserOrder Order { get; } = ParserOrder.Addition;
        protected override string Symbol { get; } = "+";

        public AdditionParser(ParserCombinator combinator) : base(combinator)
        {
        }

        protected override Expression GetExpression(Expression[] arguments)
        {
            return arguments
                .Skip(2)
                .Aggregate(
                    Expression.Add(arguments[0], arguments[1]),
                    Expression.Add
                );
        }
    }
}
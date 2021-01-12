using System.Linq;
using System.Linq.Expressions;

namespace TokenParsing.TopDowns1
{
    public class DivisionParser : BinaryInfixParser
    {
        public override ParserOrder Order { get; } = ParserOrder.Division;
        protected override string Symbol { get; } = "/";

        public DivisionParser(ParserCombinator combinator) : base(combinator)
        {
        }

        protected override Expression GetExpression(Expression[] arguments)
        {
            return arguments
                .Skip(2)
                .Aggregate(
                    Expression.Divide(arguments[0], arguments[1]),
                    Expression.Divide
                );
        }
    }
}
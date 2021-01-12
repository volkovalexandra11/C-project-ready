using System.Linq;
using System.Linq.Expressions;

namespace TokenParsing.TopDowns1
{
    public class SubtractionParser : BinaryInfixParser
    {
        public override ParserOrder Order { get; } = ParserOrder.Subtraction;
        protected override string Symbol { get; } = "-";

        public SubtractionParser(ParserCombinator combinator) : base(combinator)
        {
        }

        protected override Expression GetExpression(Expression[] arguments)
        {
            var reverseArguments = arguments.Reverse().ToArray();
            return reverseArguments
                .Skip(2)
                .Aggregate(
                    Expression.Subtract(reverseArguments[1], reverseArguments[0]),
                    (subtrExpr, prevArg) => Expression.Subtract(prevArg, subtrExpr)
                );
        }
    }
}
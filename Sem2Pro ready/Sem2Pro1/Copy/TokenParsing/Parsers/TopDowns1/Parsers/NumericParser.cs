using System.Linq.Expressions;

namespace TokenParsing.TopDowns1
{
    public class NumericParser : IParser
    {
        public ParserCombinator Combinator { get; }
        public ParserOrder Order { get; } = ParserOrder.Numeric;

        public NumericParser(ParserCombinator combinator)
        {
            Combinator = combinator;
        }

        public bool TryParse(PrioritizedString expr, UserInput input, out Expression parsed)
        {
            expr = expr.Trim();
            
            if (double.TryParse(expr.Input, out var num))
            {
                parsed = Expression.Constant(num);
                return true;
            }

            parsed = default;
            return false;
        }
    }
}
using System;
using System.Globalization;
using System.Linq.Expressions;

namespace Infrastructure.TopDowns
{
    public class NumericParser : IParser
    {
        public ParserCombinator Combinator => LazyCombinator.Value;
        public ParserOrder Order { get; } = ParserOrder.Numeric;

        protected readonly Lazy<ParserCombinator> LazyCombinator;

        public NumericParser(Lazy<ParserCombinator> combinator)
        {
            LazyCombinator = combinator;
        }

        public bool TryParse(PrioritizedString expr, ParameterInfo paramInfo, out Expression parsed)
        {
            expr = expr.Trim();
            if (expr.Length == 0)
                throw new ParseException("Empty bracket expression was given");

            if (double.TryParse(
                expr.Input,
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture,
                out var num)
            )
            {
                parsed = Expression.Constant(num);
                return true;
            }

            parsed = default;
            return false;
        }
    }
}
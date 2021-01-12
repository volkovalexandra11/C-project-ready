using System;
using System.Linq.Expressions;

namespace Infrastructure.TopDowns
{
    public class UnaryMinusParser : IParser
    {
        public ParserCombinator Combinator => LazyCombinator.Value;
        public ParserOrder Order { get; } = ParserOrder.UnaryMinus;

        protected readonly Lazy<ParserCombinator> LazyCombinator;

        public UnaryMinusParser(Lazy<ParserCombinator> combinator)
        {
            LazyCombinator = combinator;
        }

        public bool TryParse(PrioritizedString expr, ParameterInfo paramInfo, out Expression parsed)
        {
            expr = expr.Trim();
            if (!expr.Input.StartsWith("-"))
            {
                parsed = null;
                return false;
            }
            var arg = expr.Substring(1, expr.Length - 1).Trim();
            if (arg.Length == 0)
            {
                throw new ParseException("Found minus sign without argument");
            }
            if (arg.Input[0] == '-' || arg.Input[0] == '+')
            {
                throw new ParseException("Unary minus/plus cannot be followed by unary minus/plus");
            }
            parsed = Expression.Negate(Combinator.Parse(arg, paramInfo, Order));
            return true;
        }
    }
}
using System;
using System.Linq.Expressions;

namespace Infrastructure.TopDowns
{
    public class UnarySignParser : IParser
    {
        public ParserCombinator Combinator => LazyCombinator.Value;
        public ParserOrder Order { get; } = ParserOrder.UnarySign;

        protected readonly Lazy<ParserCombinator> LazyCombinator;

        public UnarySignParser(Lazy<ParserCombinator> combinator)
        {
            LazyCombinator = combinator;
        }

        public bool TryParse(PrioritizedString expr, ParameterInfo paramInfo, out Expression parsed)
        {
            expr = expr.Trim();

            bool isUnaryPlus;
            if (expr.Input.StartsWith("+"))
            {
                isUnaryPlus = true;
            }
            else if (expr.Input.StartsWith("-"))
            {
                isUnaryPlus = false;
            }
            else
            {
                parsed = default;
                return false;
            }

            var arg = expr.Substring(1, expr.Length - 1).Trim();
            if (arg.Length == 0)
            {
                throw new ParseException("Found plus sign without argument");
            }
            if (arg.Input[0] == '-' || arg.Input[0] == '+')
            {
                throw new ParseException("Unary minus/plus cannot be followed by unary minus/plus");
            }
            parsed = isUnaryPlus
                ? Combinator.Parse(arg, paramInfo, Order)
                : Expression.Negate(Combinator.Parse(arg, paramInfo, Order));

            return true;
        }
    }
}
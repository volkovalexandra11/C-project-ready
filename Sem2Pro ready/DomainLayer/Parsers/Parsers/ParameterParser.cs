using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Infrastructure.TopDowns
{
    public class ParameterParser : IParser
    {
        public ParserCombinator Combinator => LazyCombinator.Value;
        public ParserOrder Order { get; } = ParserOrder.Parameter;

        protected readonly Lazy<ParserCombinator> LazyCombinator;

        private static Regex ParameterRe { get; } = new Regex(@"^[a-zA-Z]+\d*$");

        public ParameterParser(Lazy<ParserCombinator> combinator)
        {
            LazyCombinator = combinator;
        }

        public bool TryParse(PrioritizedString expr, ParameterInfo paramInfo, out Expression parsed)
        {
            expr = expr.Trim();
            if (expr.Length == 0)
                throw new ParseException("Empty bracket expression was given");

            var exprStr = expr.Input;
            if (paramInfo.Parameters.TryGetValue(exprStr, out var param))
            {
                parsed = param;
                return true;
            }

            if (paramInfo.MainParameter is null && ParameterRe.IsMatch(exprStr))
            {
                paramInfo.MainParameter = exprStr;
                var mainParam = Expression.Parameter(typeof(double), exprStr);
                paramInfo.Parameters[exprStr] = mainParam;
                parsed = mainParam;
                return true;
            }

            parsed = default;
            return false;
        }
    }
}
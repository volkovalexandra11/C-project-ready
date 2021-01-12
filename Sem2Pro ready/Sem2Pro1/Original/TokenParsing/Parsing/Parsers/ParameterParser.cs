using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace TokenParsing.TopDowns1
{
    public class ParameterParser : IParser
    {
        public ParserCombinator Combinator { get; }
        public ParserOrder Order { get; } = ParserOrder.Parameter;

        private static Regex ParameterRe { get; } = new Regex(@"^[a-zA-Z]+\d*$");

        public ParameterParser(ParserCombinator combinator)
        {
            Combinator = combinator;
        }

        public bool TryParse(PrioritizedString expr, UserInput input, out Expression parsed)
        {
            expr = expr.Trim();
            var exprStr = expr.Input;
            if (input.Parameters.TryGetValue(exprStr, out var param))
            {
                parsed = param;
                return true;
            }

            if (input.MainParameter is null && ParameterRe.IsMatch(exprStr))
            {
                input.MainParameter = exprStr;
                var mainParam = Expression.Parameter(typeof(double), exprStr);
                input.Parameters[exprStr] = mainParam;
                parsed = mainParam;
                return true;
            }

            parsed = default;
            return false;
        }
    }
}
using System;
using System.Linq;
using System.Linq.Expressions;
using Infrastructure.Infrastructure;

namespace Infrastructure.TopDowns
{
    public abstract class BinaryInfixParser : IParser
    {
        public abstract ParserOrder Order { get; }
        protected abstract string Symbol { get; }
        public ParserCombinator Combinator => LazyCombinator.Value;

        protected readonly Lazy<ParserCombinator> LazyCombinator;

        protected BinaryInfixParser(Lazy<ParserCombinator> lazyCombinator)
        {
            LazyCombinator = lazyCombinator;
        }

        public bool TryParse(
            PrioritizedString expr, ParameterInfo paramInfo, out Expression parsed)
        {
            var minPriorityInds = expr.Priorities.GetMinIndexes();
            var splitBySymbol = expr.SplitOnSubset(Symbol, minPriorityInds);

            if (splitBySymbol.Length > 1 && splitBySymbol[0].Trim().Input == "")
            {
                splitBySymbol[1] = new PrioritizedString(Symbol).Concat(splitBySymbol[1]);
                splitBySymbol = splitBySymbol.Skip(1).ToArray();
            }
            if (splitBySymbol.Length == 1)
            {
                parsed = null;
                return false;
            }
            parsed = ParseToExpression(splitBySymbol, paramInfo);
            return true;
        }

        protected abstract Expression GetExpression(Expression[] arguments);

        private Expression ParseToExpression(PrioritizedString[] arguments, ParameterInfo input)
        {
            var argumentExpressions = arguments
                .Select(argument => Combinator.Parse(argument, input, Order))
                .ToArray();

            return GetExpression(argumentExpressions);
        }
    }
}
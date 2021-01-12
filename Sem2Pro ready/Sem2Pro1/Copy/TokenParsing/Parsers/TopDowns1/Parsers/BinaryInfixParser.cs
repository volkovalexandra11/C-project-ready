using System.Linq;
using System.Linq.Expressions;
using TokenParsing.Infrastructure;

namespace TokenParsing.TopDowns1
{
    public abstract class BinaryInfixParser : IParser
    {
        public abstract ParserOrder Order { get; }
        protected abstract string Symbol { get; }
        public ParserCombinator Combinator { get; }

        protected BinaryInfixParser(ParserCombinator combinator)
        {
            Combinator = combinator;
        }

        public bool TryParse(PrioritizedString expr, UserInput input, out Expression parsed)
        {
            var minPriorityInds = expr.Priorities.GetMinIndexes();
            var splitBySymbol = expr.SplitOnSubset(Symbol, minPriorityInds);
            if (splitBySymbol.Length == 1)
            {
                parsed = null;
                return false;
            }
            parsed = ParseToExpression(splitBySymbol, input);
            return true;
        }

        protected abstract Expression GetExpression(Expression[] arguments);

        private Expression ParseToExpression(PrioritizedString[] arguments, UserInput input)
        {
            var argumentExpressions = arguments
                .Select(argument => Combinator.Parse(argument, input, Order))
                .ToArray();

            return GetExpression(argumentExpressions);
        }
    }
}
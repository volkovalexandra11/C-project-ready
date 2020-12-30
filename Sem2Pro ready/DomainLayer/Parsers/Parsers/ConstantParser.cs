using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.TopDowns
{
    public class ConstantParser : IParser
    {
        public ParserCombinator Combinator => LazyCombinator.Value;
        public ParserOrder Order { get; } = ParserOrder.Constant;

        protected readonly Lazy<ParserCombinator> LazyCombinator;

        private static Dictionary<string, double> Constants { get; }
            = new Dictionary<string, double>
            {
                ["e"] = Math.E,
                ["pi"] = Math.PI
            };

        private static List<string> ConstantsSorted { get; }
            = Constants.Keys.OrderByDescending(c => c.Length).ToList();

        public ConstantParser(Lazy<ParserCombinator> lazyCombinator)
        {
            LazyCombinator = lazyCombinator;
        }

        public bool TryParse(PrioritizedString expr, ParameterInfo paramInfo, out Expression parsed)
        {
            expr = expr.Trim();

            foreach (var constName in ConstantsSorted.Where(constName => expr.Input == constName))
            {
                parsed = Expression.Constant(Constants[constName]);
                return true;
            }
            parsed = null;
            return false;
        }
    }
}
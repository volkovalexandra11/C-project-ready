using System;
using System.Linq;
using System.Linq.Expressions;
using Infrastructure.Infrastructure;

namespace Infrastructure.TopDowns
{
    public class BracketParser : IParser
    {
        public ParserCombinator Combinator => LazyCombinator.Value;
        public ParserOrder Order { get; } = ParserOrder.Bracket;

        protected readonly Lazy<ParserCombinator> LazyCombinator;


        public BracketParser(Lazy<ParserCombinator> combinator)
        {
            LazyCombinator = combinator;
        }

        public bool TryParse(PrioritizedString expr, ParameterInfo paramInfo, out Expression parsed)
        {
            expr = expr.Trim();
            var exprStr = expr.Input;
            if (!Brackets.AreMatching(exprStr.First(), exprStr.Last()))
            {
                parsed = null;
                return false;
            }

            var exprWithoutBrackets = expr.Substring(1, expr.Input.Length - 2);
            parsed = Combinator.ParseFunctionalExpression(exprWithoutBrackets, paramInfo);
            return true;
        }
    }
}
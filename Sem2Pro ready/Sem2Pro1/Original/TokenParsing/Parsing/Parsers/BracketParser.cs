using System.Linq;
using System.Linq.Expressions;
using TokenParsing.Infrastructure;

namespace TokenParsing.TopDowns1
{
    public class BracketParser : IParser
    {
        public ParserCombinator Combinator { get; }
        public ParserOrder Order { get; } = ParserOrder.Bracket;

        public BracketParser(ParserCombinator combinator)
        {
            Combinator = combinator;
        }

        public bool TryParse(PrioritizedString expr, UserInput input, out Expression parsed)
        {
            expr = expr.Trim();
            var exprStr = expr.Input;
            if (!Brackets.AreMatching(exprStr.First(), exprStr.Last()))
            {
                parsed = null;
                return false;
            }

            var exprWithoutBrackets = expr.Substring(1, expr.Input.Length - 2);
            parsed = Combinator.ParseFunctionalExpression(exprWithoutBrackets, input);
            return true;
        }
    }
}
using System.Linq.Expressions;

namespace TokenParsing.TopDowns1
{
    public interface IParser
    {
        ParserOrder Order { get; }
        ParserCombinator Combinator { get; }
        bool TryParse(PrioritizedString expr, UserInput input, out Expression parsed);
    }
}
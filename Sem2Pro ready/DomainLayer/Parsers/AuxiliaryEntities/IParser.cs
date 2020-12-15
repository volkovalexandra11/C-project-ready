using System.Linq.Expressions;

namespace Infrastructure.TopDowns
{
    public interface IParser
    {
        ParserOrder Order { get; }
        ParserCombinator Combinator { get; }
        bool TryParse(PrioritizedString expr, UserInput input, out Expression parsed);
    }
}
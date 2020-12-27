using System.Linq.Expressions;

namespace Infrastructure.TopDowns
{
    public interface IParser
    {
        ParserOrder Order { get; }
        ParserCombinator Combinator { get; }
        bool TryParse(PrioritizedString expr, ParameterInfo paramInfo, out Expression parsed);
    }
}
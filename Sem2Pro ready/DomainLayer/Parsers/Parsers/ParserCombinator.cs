using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.TopDowns
{
    public class ParserCombinator
    {
        private SortedDictionary<ParserOrder, IParser> OrderedParsers { get; }

        public ParserCombinator(IEnumerable<IParser> parsers)
        {
            OrderedParsers = new SortedDictionary<ParserOrder, IParser>();
            foreach (var parser in parsers)
            {
                OrderedParsers[parser.Order] = parser;
            }
        }

        public Func<double, double> Parse(string input)
        {
            if (input is null)
                throw new ArgumentNullException(nameof(input));

            var paramInfo = new ParameterInfo();
            var funcBody = Parse(new PrioritizedString(input), paramInfo);
            var mainParam = paramInfo.MainParameter is null
                ? Expression.Parameter(typeof(double), "x")
                : paramInfo.Parameters[paramInfo.MainParameter];
            return Expression.Lambda<Func<double, double>>(funcBody, mainParam).Compile();
        }

        public Expression Parse(PrioritizedString expr, ParameterInfo input)
        {
            foreach (var parser in OrderedParsers
                .Select(keyValue => keyValue.Value))
            {
                if (parser.TryParse(expr, input, out var parsed))
                    return parsed;
            }
            throw new ParseException($"Unknown expression {expr.Input}");
        }

        public Expression ParseFunctionalExpression(PrioritizedString expr, ParameterInfo input)
        {
            return Parse(expr, input, ParserOrder.Where);
        }

        public Expression Parse(PrioritizedString expr, ParameterInfo input, ParserOrder fromOrder)
        {
            foreach (var parser in OrderedParsers
                .Where(keyValue => keyValue.Key > fromOrder)
                .Select(keyValue => keyValue.Value))
            {
                if (parser.TryParse(expr, input, out var parsed))
                    return parsed;
            }
            throw new ParseException($"Unknown expression {expr.Input}");
        }
    }
}
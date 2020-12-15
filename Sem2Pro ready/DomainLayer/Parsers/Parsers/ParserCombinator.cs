using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.TopDowns
{
    public class ParserCombinator
    {
        private SortedDictionary<ParserOrder, IParser> OrderedParsers { get; }

        public ParserCombinator()
        {
            OrderedParsers = new SortedDictionary<ParserOrder, IParser>
            {
                [ParserOrder.Where] = new WhereParser(this),
                [ParserOrder.Addition] = new AdditionParser(this),
                [ParserOrder.Subtraction] = new SubtractionParser(this),
                [ParserOrder.Multiplication] = new MultiplicationParser(this),
                [ParserOrder.Division] = new DivisionParser(this),
                [ParserOrder.Call] = new CallParser(this),
                [ParserOrder.Power] = new PowerParser(this),
                [ParserOrder.Numeric] = new NumericParser(this),
                [ParserOrder.Parameter] = new ParameterParser(this),
                [ParserOrder.Bracket] = new BracketParser(this)
            };
        }

        public Func<double, double> Parse(string input)
        {
            var userInput = new UserInput(input);
            var mainFunc = Parse(userInput.FullInput, userInput);
            var mainParam = userInput.MainParameter is null
                ? Expression.Parameter(typeof(double), "x")
                : userInput.Parameters[userInput.MainParameter];
            return Expression.Lambda<Func<double, double>>(mainFunc, mainParam).Compile();
        }

        public Expression Parse(PrioritizedString expr, UserInput input)
        {
            foreach (var parser in OrderedParsers
                .Select(keyValue => keyValue.Value))
            {
                if (parser.TryParse(expr, input, out var parsed))
                    return parsed;
            }
            throw new ArgumentException($"Unknown expression {expr.Input}");
        }

        public Expression ParseFunctionalExpression(PrioritizedString expr, UserInput input)
        {
            return Parse(expr, input, ParserOrder.Where);
        }

        public Expression Parse(PrioritizedString expr, UserInput input, ParserOrder fromOrder)
        {
            foreach (var parser in OrderedParsers
                .Where(keyValue => keyValue.Key > fromOrder)
                .Select(keyValue => keyValue.Value))
            {
                if (parser.TryParse(expr, input, out var parsed))
                    return parsed;
            }
            throw new ArgumentException($"Unknown expression {expr.Input}");
        }
    }
}
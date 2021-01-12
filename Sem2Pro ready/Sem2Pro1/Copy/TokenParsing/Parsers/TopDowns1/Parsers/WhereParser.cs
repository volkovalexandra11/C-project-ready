using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using TokenParsing.Infrastructure;

namespace TokenParsing.TopDowns1
{
    public class WhereParser : IParser
    {
        public ParserCombinator Combinator { get; }
        public ParserOrder Order { get; } = ParserOrder.Where;

        private static string ParamNamePattern { get; }
            = @"(?<paramName>[a-zA-Z]+\d*)";
        private static Regex ParameterRe { get; } = new Regex(
            $@"{ParamNamePattern}\s*=\s*(?<paramExpr>.+?)(?=(,\s*([a-zA-Z]+\d*)\s*=\s*)|$)"
        );

        public WhereParser(ParserCombinator combinator)
        {
            Combinator = combinator;
        }

        public bool TryParse(PrioritizedString expr, UserInput input, out Expression parsed)
        {
            if (input.MainExpression != null || input.ParameterExpressions.Any())
                throw new InvalidOperationException("Where has already been found");

            var minPriorityInds = expr.Priorities.GetMinIndexes();
            var whereParts = expr.SplitOnSubset("where", minPriorityInds);
            if (whereParts.Length > 2)
                throw new ArgumentException("Too many 'where' parts!");
            if (whereParts.Length == 1)
            {
                parsed = null;
                input.MainExpression = whereParts[0];
                return false;
            }

            var mainFunc = whereParts[0];
            if (mainFunc.Input.Last() == ',')
                mainFunc = mainFunc.Substring(0, mainFunc.Input.Length - 1);
            input.MainExpression = mainFunc;

            foreach (Match match in ParameterRe.Matches(whereParts[1].Input))
            {
                var (paramName, paramExpr) = (match.Groups["paramName"].Value, match.Groups["paramExpr"].Value);
                if (input.Parameters.ContainsKey(paramName))
                    throw new ArgumentException($"Parameter {paramName} is defined multiple times");

                input.Parameters[paramName] = Expression.Parameter(typeof(double), paramName);
                input.ParameterExpressions[paramName] = new PrioritizedString(paramExpr);
            }
            var parameterExprs = input.ParameterExpressions
                .ToDictionary(
                    nameWithExpr => nameWithExpr.Key,
                    nameWithExpr => Combinator.ParseFunctionalExpression(nameWithExpr.Value, input)
                );
            var mainExpr = Combinator.ParseFunctionalExpression(mainFunc, input);

            var paramReplacer = new ParameterReplacerVisitor(parameterExprs);
            parsed = paramReplacer.Visit(mainExpr);
            return true;
        }
    }
}
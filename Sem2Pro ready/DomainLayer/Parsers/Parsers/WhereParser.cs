using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Infrastructure.Infrastructure;

namespace Infrastructure.TopDowns
{
    public class WhereParser : IParser
    {
        public ParserCombinator Combinator => LazyCombinator.Value;
        public ParserOrder Order { get; } = ParserOrder.Where;

        protected readonly Lazy<ParserCombinator> LazyCombinator;

        private static string ParamNamePattern { get; }
            = @"(?<paramName>[a-zA-Z]+\d*)";
        private static Regex ParameterRe { get; } = new Regex(
            $@"{ParamNamePattern}\s*=\s*(?<paramExpr>.+?)(?=(,\s*([a-zA-Z]+\d*)\s*=\s*)|$)"
        );

        public WhereParser(Lazy<ParserCombinator> combinator)
        {
            LazyCombinator = combinator;
        }
        
        public bool TryParse(PrioritizedString expr, ParameterInfo paramInfo, out Expression parsed)
        {
            if (expr.Length == 0)
                throw new ParseException("Empty bracket expression was given");

            var minPriorityInds = expr.Priorities.GetMinIndexes();
            var whereParts = expr.SplitOnSubset("where", minPriorityInds);
            if (whereParts.Length > 2)
                throw new ParseException("Too many 'where'!");

            if (whereParts.Length == 1)
            {
                parsed = null;
                return false;
            }

            var mainFunc = whereParts[0];
            if (mainFunc.Input.Last() == ',')
                mainFunc = mainFunc.Substring(0, mainFunc.Input.Length - 1);

            var parameterExpressions = new Dictionary<string, PrioritizedString>();
            foreach (Match match in ParameterRe.Matches(whereParts[1].Input))
            {
                var (paramName, paramExpr) = (match.Groups["paramName"].Value, match.Groups["paramExpr"].Value);
                if (paramInfo.Parameters.ContainsKey(paramName))
                    throw new ParseException($"Parameter {paramName} is defined multiple times");

                paramInfo.Parameters[paramName] = Expression.Parameter(typeof(double), paramName);
                parameterExpressions[paramName] = new PrioritizedString(paramExpr);
            }
            var parameterExprs = parameterExpressions
                .ToDictionary(
                    nameWithExpr => nameWithExpr.Key,
                    nameWithExpr => Combinator.ParseFunctionalExpression(nameWithExpr.Value, paramInfo)
                );
            var mainExpr = Combinator.ParseFunctionalExpression(mainFunc, paramInfo);

            var paramReplacer = new ParameterReplacerVisitor(parameterExprs);
            parsed = paramReplacer.Visit(mainExpr);
            return true;
        }
    }
}
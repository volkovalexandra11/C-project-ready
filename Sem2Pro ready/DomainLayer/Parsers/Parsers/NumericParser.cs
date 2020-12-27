﻿using System;
using System.Linq.Expressions;

namespace Infrastructure.TopDowns
{
    public class NumericParser : IParser
    {
        public ParserCombinator Combinator => LazyCombinator.Value;
        public ParserOrder Order { get; } = ParserOrder.Numeric;

        protected readonly Lazy<ParserCombinator> LazyCombinator;

        public NumericParser(Lazy<ParserCombinator> combinator)
        {
            LazyCombinator = combinator;
        }

        public bool TryParse(PrioritizedString expr, ParameterInfo paramInfo, out Expression parsed)
        {
            expr = expr.Trim();
            
            if (double.TryParse(expr.Input, out var num))
            {
                parsed = Expression.Constant(num);
                return true;
            }

            parsed = default;
            return false;
        }
    }
}
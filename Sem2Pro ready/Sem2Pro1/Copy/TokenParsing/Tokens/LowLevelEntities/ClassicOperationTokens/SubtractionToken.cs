using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace TokenParsing
{
    class SubtractionToken : BinaryInfixToken
    {
        public sealed override BasePriority BasePriority => BasePriority.Subtraction;

        public SubtractionToken(int bracketPriority)
        {
            Priority = bracketPriority + (int)BasePriority;
        }

        public override Expression MakeExpression(Expression left, Expression right)
        {
            return Expression.Subtract(left, right);
        }
    }
}

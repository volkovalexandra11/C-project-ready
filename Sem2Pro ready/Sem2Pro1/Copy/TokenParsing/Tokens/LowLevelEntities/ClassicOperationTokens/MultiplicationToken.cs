using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace TokenParsing
{
    class MultiplicationToken : BinaryInfixToken
    {
        public sealed override BasePriority BasePriority => BasePriority.Multiplication;

        public MultiplicationToken(int bracketPriority)
        {
            Priority = bracketPriority + (int)BasePriority;
        }

        public override Expression MakeExpression(Expression left, Expression right)
        {
            return Expression.Multiply(left, right);
        }
    }
}

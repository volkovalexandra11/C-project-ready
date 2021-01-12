using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace TokenParsing
{
    class AdditionToken : BinaryInfixToken
    {
        public sealed override BasePriority BasePriority => BasePriority.Addition;

        public AdditionToken(int bracketPriority)
        {
            Priority = bracketPriority + (int)BasePriority;
        }

        public override Expression MakeExpression(Expression left, Expression right)
        {
            return Expression.Add(left, right);
        }
    }
}

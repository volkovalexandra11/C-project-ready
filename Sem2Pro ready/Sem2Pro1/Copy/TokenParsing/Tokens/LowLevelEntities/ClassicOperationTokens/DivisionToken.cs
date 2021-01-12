using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace TokenParsing
{
    class DivisionToken : BinaryInfixToken
    {
        public sealed override BasePriority BasePriority => BasePriority.Division;

        public DivisionToken(int bracketPriority)
        {
            Priority = bracketPriority + (int)BasePriority;
        }

        public override Expression MakeExpression(Expression left, Expression right)
        {
            return Expression.Divide(left, right);
        }
    }
}

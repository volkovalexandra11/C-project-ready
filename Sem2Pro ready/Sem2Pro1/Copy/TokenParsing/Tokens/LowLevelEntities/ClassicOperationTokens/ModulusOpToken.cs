using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace TokenParsing.Tokens.LowLevelEntities.ClassicOperationTokens
{
    class ModulusOpToken : BinaryInfixToken
    {
        public sealed override BasePriority BasePriority => BasePriority.Modulo;

        public ModulusOpToken(int bracketPriority)
        {
            Priority = bracketPriority + (int)BasePriority;
        }

        public override Expression MakeExpression(Expression left, Expression right)
        {
            return Expression.Modulo(left, right);
        }
    }
}

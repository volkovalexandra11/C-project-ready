using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace TokenParsing.Tokens.LowLevelEntities.ClassicOperationTokens
{
    class PowerToken : BinaryInfixToken
    {
        public sealed override BasePriority BasePriority => BasePriority.Power;

        public static MethodInfo PowerMethod
            = typeof(Math).GetMethod("Pow", new[] { typeof(double), typeof(double) });

        public PowerToken(int bracketPriority)
        {
            Priority = bracketPriority + (int)BasePriority;
        }

        public override Expression MakeExpression(Expression left, Expression right)
        {
            return Expression.Call(PowerMethod, left, right);
        }
    }
}

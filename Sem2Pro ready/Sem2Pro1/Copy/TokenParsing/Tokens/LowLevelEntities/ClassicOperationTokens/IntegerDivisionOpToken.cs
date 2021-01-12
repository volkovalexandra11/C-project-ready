using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace TokenParsing.Tokens.LowLevelEntities.ClassicOperationTokens
{
    class IntegerDivisionOpToken : BinaryInfixToken
    {
        public sealed override BasePriority BasePriority => BasePriority.IntegerDivision;

        public IntegerDivisionOpToken(int bracketPriority)
        {
            Priority = bracketPriority + (int)BasePriority;
        }

        public override Expression MakeExpression(Expression left, Expression right)
        {
            return Expression.Divide(
                BuildRoundExpression(left),
                BuildRoundExpression(right)
            );
        }

        private static Expression BuildRoundExpression(Expression operand)
        {
            var roundMethod = typeof(Math).GetMethod("Round", new [] { typeof(double) });
            if (roundMethod is null)
                throw new InvalidOperationException("Couldn't find round method");
            return Expression.Call(roundMethod, operand);
        }
    }
}

using System.Linq.Expressions;
using TokenParsing.Tokens.MidLevelEntities;

namespace TokenParsing.Tokens
{
    public class NegateToken : UnaryPrefixToken
    {
        public override Expression MakeExpression(Expression operand)
        {
            return Expression.Negate(operand);
        }
    }
}
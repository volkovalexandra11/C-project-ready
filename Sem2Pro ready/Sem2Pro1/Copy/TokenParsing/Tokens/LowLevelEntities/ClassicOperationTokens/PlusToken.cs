using System.Linq.Expressions;
using TokenParsing.Tokens.MidLevelEntities;

namespace TokenParsing.Tokens
{
    public class PlusToken : UnaryPrefixToken
    {
        public override Expression MakeExpression(Expression operand) => operand;
    }
}
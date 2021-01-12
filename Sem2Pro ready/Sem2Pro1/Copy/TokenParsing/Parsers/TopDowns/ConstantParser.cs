using System.Linq.Expressions;

namespace TokenParsing
{
    public class ConstantParser
    {
        public bool TryParse(string constant, out ConstantExpression constantExpr)
        {
            if (double.TryParse(constant.Trim(), out var res))
            {
                constantExpr = Expression.Constant(res);
                return true;
            }
            constantExpr = default;
            return false;
        }
    }
}
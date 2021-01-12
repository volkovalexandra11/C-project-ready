using System.Linq;
using System.Linq.Expressions;

namespace TokenParsing
{
    public class ParameterParser
    {
        public bool TryParse(string input, out ParameterExpression parameterExpr)
        {
            parameterExpr = default;
            input = input.Trim();
            if (input.Length == 0)
                return false;
            if (input.Contains(' '))
                return false;

            if (char.IsDigit(input[0]))
                return false;
            return input.All(c => char.IsLetterOrDigit(c));
        }
        // countcos5xcount
    }
}
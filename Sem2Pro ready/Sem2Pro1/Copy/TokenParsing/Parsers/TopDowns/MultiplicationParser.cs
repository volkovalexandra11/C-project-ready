using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;

namespace TokenParsing
{
    public class MultiplicationParser
    {
        private readonly ConstantParser constParser;
        private readonly ParameterParser paramParser;

        public MultiplicationParser(ConstantParser constParser, ParameterParser paramParser)
        {
            this.constParser = constParser;
            this.paramParser = paramParser;
        }

        public bool TryParse(string input, out BinaryExpression binaryExpr)
        {
            binaryExpr = default;
            input = input.Trim();

            var parts = input.Split('*');
            if (parts.Length != 2)
                return false;

            if (TryParseOperand(parts[0], out var left) && TryParseOperand(parts[1], out var right))
            {
                binaryExpr = Expression.Add(left, right);
                return true;
            }
            return false;
        }

        private bool TryParseOperand(string input, out Expression operand)
        {
            if (constParser.TryParse(input, out var operand1))
            {
                operand = operand1;
                return true;
            }
            if (paramParser.TryParse(input, out var operand2))
            {
                operand = operand2;
                return true;
            }
            operand = default;
            return false;
        }
        // countcos5xcount
    }
}
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;

namespace TokenParsing
{
    public class OperandParser
    {
        private readonly SumParser sumParser;
        private readonly MultiplicationParser multiplicationParser;

        public OperandParser(SumParser sumParser, MultiplicationParser multiplicationParser)
        {
            this.sumParser = sumParser;
            this.multiplicationParser = multiplicationParser;
        }

        public bool TryParse(string input, out BinaryExpression multiplicationExpr)
        {
            if (multiplicationParser.TryParse(input, out var operand2))
            {
                multiplicationExpr = operand2;
                return true;
            }
            if (sumParser.TryParse(input, out var operand1))
            {
                multiplicationExpr = operand1;
                return true;
            }
            multiplicationExpr = default;
            return false;
        }
        // countcos5xcount
    }
}
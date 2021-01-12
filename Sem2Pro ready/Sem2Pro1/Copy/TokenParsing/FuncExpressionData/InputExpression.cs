using System.Collections.Generic;

namespace TokenParsing.FuncExpressionData
{
    public class InputExpression
    {
        public string[] MainFunction { get; }
        public string MainParameter { get; }

        public Dictionary<string, string[]> ParameterExpressions { get; }

        public InputExpression(
            string[] mainFunction,
            string mainParameter,
            Dictionary<string, string[]> parameterExpressions)
        {
            MainFunction = mainFunction;
            MainParameter = mainParameter;
            ParameterExpressions = parameterExpressions;
        }
    }
}
using System.Collections.Generic;

namespace TokenParsing.FuncExpressionData
{
    public class TokenizedExpression
    {
        public IToken[] MainFunction;

        public ParameterToken MainParameter;
        public Dictionary<ParameterToken, IToken[]> Parameters;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using TokenParsing.FuncExpressionData;
using TokenParsing.Tokens.MidLevelEntities;

namespace TokenParsing
{
    public class StringToTokenParser
    {
        public TokenizedExpression Result { get; }
        
        private InputExpression ParsedInput { get; }

        private ParameterToken MainParameter { get; }
        private Dictionary<string, ParameterToken> Parameters { get; }

        private int ThisBracketPriority { get; set; }
        private bool LastTokenIsEndOfExpression { get; set; }

        public StringToTokenParser(InputExpression parsedInput)
        {
            ParsedInput = parsedInput;

            var mainParamAsExpr = ParsedInput.MainParameter is null
                ? Expression.Parameter(typeof(double), "x")
                : Expression.Parameter(typeof(double), ParsedInput.MainParameter);
            MainParameter = new ParameterToken(mainParamAsExpr);

            Parameters = ParsedInput.ParameterExpressions
                .Keys
                .Select(paramName => Expression.Parameter(typeof(double), paramName))
                .ToDictionary(
                    param => param.Name,
                    param => new ParameterToken(param)
                );

            Result = new TokenizedExpression()
            {
                MainFunction = ParseTokens(parsedInput.MainFunction),
                MainParameter = MainParameter,
                Parameters = ParsedInput.ParameterExpressions
                    .ToDictionary(
                        paramNameWithExpr => Parameters[paramNameWithExpr.Key],
                        paramNameWithExpr => ParseTokens(paramNameWithExpr.Value)
                    )
            };
        }

        public IToken[] ParseTokens(string[] tokens)
        {
            return tokens
                .Select(BuildToken)
                .ToArray();
        }

        private IToken BuildToken(string token)
        {
            if (token == ParsedInput.MainParameter)
            {
                LastTokenIsEndOfExpression = true;
                return MainParameter;
            }

            if (Parameters.TryGetValue(token, out var parameter))
            {
                LastTokenIsEndOfExpression = true;
                return parameter;
            }

            if (TokenSyntax.UnaryFactories.TryGetValue(token, out var unaryFactory))
            {
                LastTokenIsEndOfExpression = false;
                return unaryFactory();
            }

            if (LastTokenIsEndOfExpression)
            {
                if (TokenSyntax.BinaryFactories.TryGetValue(token, out var binaryFactory))
                {
                    LastTokenIsEndOfExpression = false;
                    return binaryFactory(ThisBracketPriority);
                }
            }

            if (TokenSyntax.Constants.TryGetValue(token, out var constant))
            {
                LastTokenIsEndOfExpression = true;
                return new ConstantToken(constant);
            }

            if (TokenSyntax.BracketFactories.TryGetValue(token, out var bracketFactory))
            {
                var bracket = bracketFactory(ThisBracketPriority);
                var isOpening = bracket.Position == BracketPosition.Opening;

                ThisBracketPriority += (isOpening
                    ? (int)BracketToken.BracketDelta
                    : -(int)BracketToken.BracketDelta);
                LastTokenIsEndOfExpression = !isOpening;
                return bracket;
            }

            if (TokenSyntax.Methods.TryGetValue(token, out var method))
            {
                LastTokenIsEndOfExpression = false;
                return new CallToken(method, ThisBracketPriority);
            }

            throw new InvalidOperationException("Unknown token");
        }
    }
}
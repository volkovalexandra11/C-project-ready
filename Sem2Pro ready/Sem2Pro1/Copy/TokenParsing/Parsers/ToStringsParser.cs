//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Runtime.InteropServices.ComTypes;
//using TokenParsing.Infrastructure;
//using System.Text.RegularExpressions;
//using TokenParsing.FuncExpressionData;

//namespace TokenParsing
//{

//    public class ToStringsParser
//    {
//        private static readonly Regex inputRe = new Regex(@"(?<func>.+)(\s*where\s*(?<parameters>.+))?");

//        private static readonly string parameterName
//            = @"(?<paramName>[a-zA-Z]+(\d+)?)";

//        private static readonly Regex parameterRe = new Regex(
//            $@"{parameterName}\s*=\s*(?<paramExpr>.+?)(?=(,\s*{parameterName}\s*=\s*)|$)"
//        );

//        private string Input { get; }

//        private string[] MainFunction { get; set; }
//        private Dictionary<string, string[]> ParameterExpressions { get; set; }

//        private string MainParameter { get; set; }
//        private HashSet<string> Parameters { get; set; }

//        private HashSet<string> PossibleTokens { get; }
//            = new HashSet<string>(TokenSyntax.AllTokens);

//        public ToStringsParser(string input)
//        {
//            Input = input.ToLower().Replace(" ", "");
//            ParameterExpressions = new Dictionary<string, string[]>();
//            Parameters = new HashSet<string>();
//            Parse();
//        }

//        public void Parse()
//        {
//            var (mainFunc, parameters) = GetMainFuncAndParameters(Input);

//            if (parameters != null)
//            {
//                ParameterExpressions = parameterRe.Matches(parameters)
//                    .ToDictionary(
//                        m => m.Groups["paramName"].Value,
//                        m => ParseFunctionalExpression(m.Groups["paramExpr"].Value)
//                    );
//                Parameters = ParameterExpressions.Keys.ToHashSet();
//                PossibleTokens.UnionWith(Parameters);
//                foreach (var param in Parameters)
//                    AssertNoCollusionInParamName(param);
//            }

//            MainFunction = ParseFunctionalExpression(mainFunc);
//        }

//        public InputExpression GetResult()
//        {
//            return new InputExpression(MainFunction, MainParameter, ParameterExpressions);
//        }

//        private (string, string) GetMainFuncAndParameters(string input)
//        {
//            var parts = Input.Split("where");
//            if (parts.Length > 2)
//                throw new InvalidOperationException("Too many 'where' blocks");
//            var mainFuncStr = parts.First();

//            var parameters = (string)null;
//            if (parts.Length == 2)
//            {
//                parameters = parts[2];
//            }

//            return (mainFuncStr, parameters);
//        }

//        private string[] ParseFunctionalExpression(string expr)
//        {
//            var tokens = new List<string>();
//            var currInd = 0;
//            while (currInd < expr.Length)
//            {
//                var nextChar = expr[currInd];
//                if (TokenSyntax.BracketFactories.ContainsKey(nextChar.ToString()))
//                {
//                    tokens.Add(nextChar.ToString());
//                }
//                else if (TokenSyntax.BinaryFactories.ContainsKey(nextChar.ToString()))
//                {
//                    tokens.Add(ParseBinaryOp(expr, currInd, out currInd));
//                }
//                else if (char.IsDigit(nextChar))
//                {
//                    tokens.Add(GetNumericFrom(expr, currInd, out currInd));
//                }
//                else if (char.IsLetter(nextChar))
//                {
//                    tokens.AddRange(ParseLiteralFrom(expr, currInd, out currInd));
//                }
//            }
//            return tokens.ToArray();
//        }

//        private string GetNumericFrom(string source, int startInd, out int endInd)
//        {
//            endInd = startInd + 1;
//            var foundDecimalSign = false;
//            while (endInd < source.Length)
//            {
//                if (!foundDecimalSign && source[endInd].IsDecimalSign())
//                {
//                    foundDecimalSign = true;
//                }
//                else if (!char.IsDigit(source[endInd]))
//                {
//                    break;
//                }
//                endInd++;
//            }
//            return source[startInd..endInd];
//        }

//        private string[] ParseLiteralFrom(string source, int startInd, out int endInd)
//        {
//            var foundDigit = false;
//            var currInd = startInd;

//            while (currInd < source.Length)
//            {
//                var currChar = source[currInd];
//                if (char.IsLetter(currChar))
//                {
//                    if (foundDigit)
//                        break;
//                }
//                else if (char.IsDigit(currChar))
//                {
//                    foundDigit = true;
//                }
//                else
//                    break;

//                currInd++;
//            }

//            endInd = currInd;
//            return ParseLiteral(source[startInd..endInd]);
//        }

//        private string[] ParseLiteral(string literal)
//        {
//            var knownTokens = TokenSyntax.AllTokensByLength // TODO
//                .Concat(Parameters.OrderByDescending(param => param.Length))
//                .ToArray();

//            var partitioner = new PriorityPartitioner(literal, knownTokens);

//            var emptyParts = partitioner.Partition.EmptyParts.ToArray();
//            foreach (var emptyPart in emptyParts)
//            {
//                var substring = literal.Substring(emptyPart.Start, emptyPart.Length);
//                if (char.IsDigit(substring.First()))
//                {
//                    var num = GetNumericFrom(literal, emptyPart.Start, out var endInd);
//                    partitioner.Partition.AddNewPart(num, emptyPart.Start, endInd - emptyPart.Start);
//                }
//            }

//            TryGetMainParameter(literal, partitioner.Partition);

//            return partitioner.GetPartitionResult(withEmptyParts: true);
//        }

//        private void TryGetMainParameter(string literal, Partition<string> literalPartition)
//        {
//            if (!literalPartition.EmptyParts.Any()) return;

//            if (literalPartition.EmptyParts.Count > 1)
//            {
//                var freeParams = literalPartition.EmptyParts
//                    .Take(2)
//                    .Select(part => part.Id);
//                throw new InvalidOperationException(
//                    $"Found multiple free parameters: {string.Join("and", freeParams)}");
//            }

//            var mainParamPart = literalPartition.EmptyParts.Single();
//            var mainParam = literal.Substring(mainParamPart.Start, mainParamPart.Length);
//            if (MainParameter is null || MainParameter == mainParam)
//            {
//                MainParameter = mainParam;
//            }
//            else
//            {
//                throw new InvalidOperationException(
//                    $"Found multiple free parameters: {string.Join("and", MainParameter, mainParam)}");
//            }
//        }

//        private string ParseBinaryOp(string source, int startInd, out int endInd)
//        {
//            endInd = startInd;
//            while (TokenSyntax.BinaryFactories.ContainsKey(source[startInd..(endInd + 1)]))
//                endInd++;
//            var operation = source[startInd..endInd];
//            if (TokenSyntax.BinaryFactories.ContainsKey(operation))
//                return operation;
//            throw new InvalidOperationException($"Unknown expression {operation}");
//        }

//        private void AssertNoCollusionInParamName(string paramName)
//        {
//            if (TokenSyntax.TokensMoreImportantThanParams.Any(paramName.Contains))
//                throw new ArgumentException($"Parameter {paramName} shares name with built-in function or symbol!");
//            if (Parameters.Count(param => param == paramName) > 1)
//                throw new ArgumentException($"Multiple parameters named {paramName}!");
//        }
//    }
//}
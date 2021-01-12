using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using TokenParsing.Infrastructure;
using TokenParsing.Tokens;
using TokenParsing.Tokens.LowLevelEntities.ClassicOperationTokens;
using TokenParsing.Tokens.MidLevelEntities;

namespace TokenParsing
{
    public static class TokenSyntax
    {
        public static Dictionary<string, Func<int, BinaryInfixToken>> BinaryFactories
            = new Dictionary<string, Func<int, BinaryInfixToken>>
            {
                ["+"] = bracketPriority => new AdditionToken(bracketPriority),
                ["-"] = bracketPriority => new SubtractionToken(bracketPriority),
                ["*"] = bracketPriority => new MultiplicationToken(bracketPriority),
                ["/"] = bracketPriority => new DivisionToken(bracketPriority),
                ["//"] = bracketPriority => new IntegerDivisionOpToken(bracketPriority),
                ["%"] = bracketPriority => new ModulusOpToken(bracketPriority),
                ["^"] = bracketPriority => new PowerToken(bracketPriority)
            };

        public static Dictionary<string, Func<UnaryPrefixToken>> UnaryFactories
            = new Dictionary<string, Func<UnaryPrefixToken>>
            {
                ["-"] = () => new NegateToken(),
                ["+"] = () => new PlusToken()
            };

        public static Dictionary<string, Func<int, BracketToken>> BracketFactories
            = new Dictionary<string, Func<int, BracketToken>>
            {
                [")"] = outerPriority => new BracketToken(BracketType.Round, BracketPosition.Closing, outerPriority),
                ["("] = outerPriority => new BracketToken(BracketType.Round, BracketPosition.Opening, outerPriority),
                ["["] = outerPriority => new BracketToken(BracketType.Square, BracketPosition.Opening, outerPriority),
                ["]"] = outerPriority => new BracketToken(BracketType.Square, BracketPosition.Closing, outerPriority),
                ["{"] = outerPriority => new BracketToken(BracketType.Curly, BracketPosition.Opening, outerPriority),
                ["}"] = outerPriority => new BracketToken(BracketType.Curly, BracketPosition.Closing, outerPriority),
            };

        public static Dictionary<string, double> Constants
            = new Dictionary<string, double>
            {
                ["pi"] = Math.PI,
                ["e"] = Math.E
            };

        public static Dictionary<string, MethodInfo> Methods
            = new Dictionary<string, MethodInfo>
            {
                ["abs"] = GetFromMath("Abs"),
                ["sqrt"] = GetFromMath("Sqrt"),
                ["cbrt"] = GetFromMath("Cbrt"),
                //["min"] = GetFromMath("Min"),
                //["max"] = GetFromMath("Max"),

                ["sin"] = GetFromMath("Sin"),
                ["cos"] = GetFromMath("Cos"),
                ["tg"] = GetFromMath("Tan"),
                ["tan"] = GetFromMath("Tan"),
                ["ctg"] = GetFromMathExt("Cot"),
                ["cot"] = GetFromMathExt("Cot"),

                ["arcsin"] = GetFromMath("Asin"),
                ["arccos"] = GetFromMath("Acos"),
                ["arctg"] = GetFromMath("Atan"),
                ["arctan"] = GetFromMath("Atan"),
                ["arcctg"] = GetFromMathExt("Acot"),
                ["arccot"] = GetFromMathExt("Acot"),

                //["lcm"] = GetFromMathExt("Gcd"),
                //["gcd"] = GetFromMathExt("Gcd"),
                ["floor"] = GetFromMath("Floor"),
                ["round"] = GetFromMath("Round"),

                ["exp"] = GetFromMath("Exp"),
                ["ln"] = GetFromMath("Log"),
                ["log"] = GetFromMath("Log"), // !1!!!1
                ["sign"] = GetFromMath("Sign")
            };

        public static HashSet<string> TokensMoreImportantThanParams
            = BinaryFactories.Keys
                .Concat(UnaryFactories.Keys)
                .Concat(BracketFactories.Keys)
                .Concat(Methods.Keys)
                .ToHashSet();

        public static HashSet<string> AllTokens
            = BinaryFactories.Keys
                .Concat(UnaryFactories.Keys)
                .Concat(BracketFactories.Keys)
                .Concat(Methods.Keys)
                .Concat(Constants.Keys)
                .ToHashSet();

        public static List<string> AllTokensByLength
            = AllTokens
                .GroupBy(token => token.Length)
                .OrderByDescending(group => group.Key)
                .SelectMany(group => group)
                .ToList();
        
        public static IEnumerable<string> GetTokensInParsePriority(IEnumerable<string> parameterNames)
        {
            var parametersOrdered = parameterNames.OrderByDescending(param => param.Length);

            return BinaryFactories.Keys
                .Concat(BracketFactories.Keys)
                .Concat(UnaryFactories.Keys)
                .Concat(Methods.Keys)
                .OrderByDescending(token => token.Length)
                .Concat(parametersOrdered)
                .Concat(Constants.Keys.OrderByDescending(constant => constant.Length));
        }

        public static IToken GetCallToken(string methodName, int bracketPriority)
        {
            return new CallToken(Methods[methodName], bracketPriority);
        }

        private static MethodInfo GetFromMath(string name)
        {
            return typeof(Math).GetMethod(name, new[] { typeof(double) })
                   ?? throw new InvalidOperationException(
                       $"Coulnd't find {nameof(Math)} method {name}"
                   );
        }

        private static MethodInfo GetFromMathExt(string name)
        {
            return typeof(MathExtensions).GetMethod(name, new[] { typeof(double) })
                   ?? throw new InvalidOperationException(
                       $"Coulnd't find {nameof(MathExtensions)} method {name}"
                   );
        }
    }
}
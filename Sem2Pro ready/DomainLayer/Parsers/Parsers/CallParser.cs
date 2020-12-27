using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Infrastructure.Infrastructure;

namespace Infrastructure.TopDowns
{
    public class CallParser : IParser
    {
        public ParserCombinator Combinator => LazyCombinator.Value;
        public ParserOrder Order { get; } = ParserOrder.Call;

        protected readonly Lazy<ParserCombinator> LazyCombinator;

        private static Dictionary<string, MethodInfo> OneArgMethods { get; }
            = new Dictionary<string, MethodInfo>
            {
                ["abs"] = GetFromMath("Abs"),
                ["sqrt"] = GetFromMath("Sqrt"),
                //["cbrt"] = GetFromMath("Cbrt"),

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
                ["log2"] = GetFromMathExt("Log2"),
                ["sign"] = GetFromMath("Sign")
            };

        private static Dictionary<string, (MethodInfo method, int argCount)> MultipleArgMethods { get; }
            = new Dictionary<string, (MethodInfo, int)>
            {
                ["log"] = (GetFromMath("Log", 2), 2),
                ["min"] = (GetFromMath("Min", 2), 2),
                ["max"] = (GetFromMath("Max", 2), 2)
            };

        private static Dictionary<string, Dictionary<int, MethodInfo>> MethodsByArgCount { get; }
            = OneArgMethods
                .Select(nameWithMethod => (methodName: nameWithMethod.Key, method: nameWithMethod.Value, argCount: 1))
                .Concat(MultipleArgMethods
                    .Select(nameWithMethodAndData => (
                        nameWithMethodAndData.Key,
                        nameWithMethodAndData.Value.method,
                        nameWithMethodAndData.Value.argCount
                    ))
                )
                .GroupBy(methods => methods.Item1)
                .Select(sameNameMethods => (
                    sameNameMethods.Key,
                    sameNameMethods.ToDictionary(method => method.argCount, method => method.method)
                ))
                .ToDictionary(sameNameDicts => sameNameDicts.Key, sameNameDicts => sameNameDicts.Item2);

        private static string[] MethodNamesSorted { get; }
            = MethodsByArgCount.Keys.OrderByDescending(key => key.Length).ToArray();

        public CallParser(Lazy<ParserCombinator> combinator)
        {
            LazyCombinator = combinator;
        }

        public bool TryParse(PrioritizedString expr, ParameterInfo paramInfo, out Expression parsed)
        {
            expr = expr.Trim();
            foreach (var methodName in MethodNamesSorted)
            {
                if (!expr.Input.StartsWith(methodName))
                    continue;
                if (char.IsWhiteSpace(expr.Input[methodName.Length]))
                {
                    throw new ParseException($"Whitespace after call of {methodName}");
                }
                if (expr.Input[methodName.Length].IsOpeningBracket() && !expr.Input[expr.Length - 1].IsClosingBracket()
                    || !expr.Input[methodName.Length].IsOpeningBracket() && expr.Input[expr.Length - 1].IsClosingBracket())
                {
                    throw new ParseException($"No matching call brackets in {methodName} call: {expr.Input}");
                }
                var (argsStart, argsEnd) = expr.Input[methodName.Length].IsOpeningBracket()
                    ? (methodName.Length + 1, expr.Length - 2)
                    : (methodName.Length, expr.Length - 1);
                var funcArgs = expr.Substring(argsStart, argsEnd - argsStart + 1);
                var arguments = SplitArguments(funcArgs);
                if (!MethodsByArgCount[methodName].TryGetValue(arguments.Length, out var method))
                {
                    throw new ParseException($"No known {methodName} functions with {arguments.Length} arguments");
                }
                var argumentExprs = arguments.Select(arg => Combinator.ParseFunctionalExpression(arg, paramInfo)).ToList();
                parsed = Expression.Call(method, argumentExprs);
                return true;
            }
            parsed = null;
            return false;
        }

        private static PrioritizedString[] SplitArguments(PrioritizedString funcArgs)
        {
            var minPriorityInds = funcArgs.Priorities.GetMinIndexes();
            return funcArgs.SplitOnSubset(",", minPriorityInds);
        }

        private static MethodInfo GetStaticMethodFromType(Type type, string methodName, int argCount = 1)
        {
            var argTypes = Enumerable.Repeat(typeof(double), argCount).ToArray();
            return type.GetMethod(methodName, argTypes)
                   ?? throw new InvalidOperationException(
                       $"Coulnd't find {type.Name} method {methodName}"
                   );
        }

        private static MethodInfo GetFromMath(string name, int argCount = 1)
        {
            return GetStaticMethodFromType(typeof(Math), name, argCount);
        }

        private static MethodInfo GetFromMathExt(string name, int argCount = 1)
        {
            return GetStaticMethodFromType(typeof(MathExtensions), name, argCount);
        }
    }
}
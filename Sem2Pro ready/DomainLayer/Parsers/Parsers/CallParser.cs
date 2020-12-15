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
        public ParserCombinator Combinator { get; }
        public ParserOrder Order { get; } = ParserOrder.Call;

        private static Dictionary<string, MethodInfo> Methods { get; }
            = new Dictionary<string, MethodInfo>
            {
                ["abs"] = GetFromMath("Abs"),
                ["sqrt"] = GetFromMath("Sqrt"),
                //["cbrt"] = GetFromMath("Cbrt"),
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
                ["log2"] = GetFromMathExt("Log2"),
                ["sign"] = GetFromMath("Sign")
            };

        private static string[] MethodNamesSorted { get; }
            = Methods.Keys.OrderByDescending(key => key.Length).ToArray();

        public CallParser(ParserCombinator combinator)
        {
            Combinator = combinator;
        }

        public bool TryParse(PrioritizedString expr, UserInput input, out Expression parsed)
        {
            expr = expr.Trim();
            foreach (var methodName in MethodNamesSorted)
            {
                if (!expr.Input.StartsWith(methodName))
                    continue;
                var method = Methods[methodName];
                var argument = expr.Substring(methodName.Length, expr.Input.Length - methodName.Length);
                var argumentExpr = Combinator.Parse(argument, input, Order);
                parsed = Expression.Call(method, argumentExpr);
                return true;
            }
            parsed = null;
            return false;
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
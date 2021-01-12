using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using Infrastructure.Infrastructure;
using Infrastructure.TopDowns;
using NUnit.Framework;

namespace TokenParsing_Tests
{
    public class TopDownTests
    {
        private static IEnumerable<object[]> SimpleCaseSource()
        {
            var inp = "2 * x + 1";
            Expression<Func<double, double>> exp = x => 2 * x + 1;
            yield return new object[] { inp, exp.Body };

            inp = "2 + 3 + 4";
            var body = Expression.Add(Expression.Add(GetConst(2), GetConst(3)), GetConst(4));
            yield return new object[] { inp, body };

            inp = "(2*x + 1) + 3";
            exp = x => (2 * x + 1) + 3;
            yield return new object[] { inp, exp.Body };

            inp = "1";
            exp = x => 1;
            yield return new object[] { inp, exp.Body };

            inp = "(2+(x*7 - 4)) / (5 + (6*x*(x / 7)))";
            exp = x => (2 + (x * 7 - 4)) / (5 + (6 * x * (x / 7)));
            yield return new object[] { inp, exp.Body };

            inp = "-x - 1 + 7 / x * 5 + x - 4";
            exp = x => (-x - 1) + ((7 / x) * 5) + (x - 4);
            yield return new object[] { inp, exp.Body };

            inp = "-x^7 + 3*x^6 - 5*x^3 + ln(x)*(-x^2) - min(4, 5)";
            exp = x => -Math.Pow(x, 7)
                       + (3 * Math.Pow(x, 6) - 5 * Math.Pow(x, 3))
                       + (Math.Log(x) * (-Math.Pow(x, 2)) - Math.Min(4.0, 5.0));
            yield return new object[] { inp, exp.Body };
        }

        private static ConstantExpression GetConst(double x)
        {
            return Expression.Constant(x, typeof(double));
        }

        [Test, TestCaseSource(nameof(SimpleCaseSource))]
        public void TestSimple(string input, Expression expected)
        {
            PerformTest(input, expected);
        }

        private static IEnumerable<object[]> WhereTestSource()
        {
            var inp = "t + 7 where t = 0";
            var body = Expression.Add(GetConst(0), GetConst(7));
            yield return new object[] { inp, body };

            inp = "t^2 + x^2 + t + x + 2 where t = 2 / x + 9";
            Expression<Func<double, double>> exp
                = x => Math.Pow(2.0 / x + 9, 2) + Math.Pow(x, 2) + (2 / x + 9) + x + 2;
            yield return new object[] { inp, exp.Body };
        }

        [Test, TestCaseSource(nameof(WhereTestSource))]
        public void TestWithWhere(string input, Expression expected)
        {
            PerformTest(input, expected);
        }

        private static IEnumerable<object[]> CallTestSource()
        {
            var inp = "sinx";
            Expression<Func<double, double>> expr = x => Math.Sin(x);
            yield return new object[] { inp, expr.Body };

            inp = "sin(2*x + 1)";
            expr = x => Math.Sin(2 * x + 1);
            yield return new object[] { inp, expr.Body };

            inp = "x * tg(3/x + ln(x/t)*t) + t where t = arccosx^2";
            expr = x => x * Math.Tan(
                            3 / x + Math.Log(
                                x / Math.Acos(Math.Pow(x, 2))
                            ) * Math.Acos(Math.Pow(x, 2)
                            )
                        ) + Math.Acos(Math.Pow(x, 2));
            yield return new object[] { inp, expr.Body };
        }


        [Test, TestCaseSource(nameof(CallTestSource))]
        public void TestCall(string input, Expression expected)
        {
            PerformTest(input, expected);
        }

        private static IEnumerable<object[]> UnaryMinusSource()
        {
            var inp = "-x - 7 - x";
            Expression<Func<double, double>> exp = x => -x - 7 - x;
            yield return new object[] { inp, exp.Body };
        }

        [Test, TestCaseSource(nameof(UnaryMinusSource))]
        public void TestUnaryMinus(string input, Expression expected)
        {
            PerformTest(input, expected);
        }

        private static IEnumerable<object[]> MultipleArgsCallSource()
        {
            var inp = "log(2, x) * x";
            Expression<Func<double, double>> exp = x => Math.Log(2, x) * x;
            yield return new object[] { inp, exp.Body };

            inp = "-min(-x^2 + 7, -(7 * x^2))";
            exp = x => -Math.Min(-Math.Pow(x, 2) + 7, -(7 * Math.Pow(x, 2)));
            yield return new object[] { inp, exp.Body };
        }

        [TestCaseSource(nameof(MultipleArgsCallSource))]
        public void TestMultipleArgsCall(string input, Expression expected)
        {
            PerformTest(input, expected);
        }

        private void PerformTest(string input, Expression expected)
        {
            var actual = GetCombinator().Parse(new PrioritizedString(input), new ParameterInfo());
            Assert.True(ExpressionComparer.AreEqual(actual, expected));
        }


        private ParserCombinator combinator;
        private ParserCombinator GetCombinator()
        {
            if (combinator != null) return combinator;

            var lazyComb = new Lazy<ParserCombinator>(() => combinator);
            var parsers = new IParser[]
            {
                new WhereParser(lazyComb),
                new AdditionParser(lazyComb),
                new SubtractionParser(lazyComb),
                new MultiplicationParser(lazyComb),
                new DivisionParser(lazyComb),
                new ConstantParser(lazyComb), 
                new UnarySignParser(lazyComb),
                new CallParser(lazyComb),
                new PowerParser(lazyComb),
                new NumericParser(lazyComb),
                new ParameterParser(lazyComb),
                new BracketParser(lazyComb)
            };
            combinator = new ParserCombinator(parsers);
            return combinator;
        }
    }
}
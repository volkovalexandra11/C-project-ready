using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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

        //public void TestUnaryMinus(){}

        private void PerformTest(string input, Expression expected)
        {
            var combinator = new ParserCombinator();
            var userInput = new UserInput(input);
            var actual = combinator.Parse(userInput.FullInput, userInput);
            Assert.True(ExpressionComparer.AreEqual(actual, expected));
        }
    }
}
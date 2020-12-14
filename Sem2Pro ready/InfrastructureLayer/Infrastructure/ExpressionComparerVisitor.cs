using System;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Infrastructure
{
    public static class ExpressionComparer
    {
        public static bool AreEqual(Expression first, Expression second)
        {
            if (first is BinaryExpression firstBin)
            {
                return second is BinaryExpression secondBin
                       && firstBin.NodeType == secondBin.NodeType
                       && AreEqual(firstBin.Left, secondBin.Left)
                       && AreEqual(firstBin.Right, secondBin.Right);
            }
            if (first is ConstantExpression firstConst)
            {
                return second is ConstantExpression secondConst
                       && firstConst.Value.Equals(secondConst.Value);
            }
            if (first is ParameterExpression firstParam)
            {
                return second is ParameterExpression secondParam
                       && firstParam.Name == secondParam.Name;
            }
            if (first is UnaryExpression firstUnary)
            {
                return second is UnaryExpression secondUnary
                       && AreEqual(firstUnary.Operand, secondUnary.Operand);
            }
            if (first is MethodCallExpression firstCall)
            {
                return second is MethodCallExpression secondCall
                       && firstCall.Method.Equals(secondCall.Method)
                       && firstCall.Arguments.Count == secondCall.Arguments.Count
                       && firstCall.Arguments
                           .Zip(secondCall.Arguments, (firstArg, secondArg) => (firstArg, secondArg))
                           .All(args => AreEqual(args.firstArg, args.secondArg));
            }
            if (first is LambdaExpression firstLambda)
            {
                return second is LambdaExpression secondLambda
                       && firstLambda.Parameters.Count == secondLambda.Parameters.Count
                       && firstLambda.Parameters
                           .Zip(secondLambda.Parameters, (param1, param2) => (param1, param2))
                           .All(parameters => AreEqual(parameters.param1, parameters.param2))
                       && AreEqual(firstLambda.Body, secondLambda.Body);
            }
            throw new NotImplementedException($"Unknown node type {first.NodeType}");
        }
    }
}
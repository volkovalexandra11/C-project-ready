using System.Collections.Generic;
using System.Linq.Expressions;

namespace TokenParsing.Infrastructure
{
    public class ParameterReplacerVisitor : ExpressionVisitor
    {
        private Dictionary<string, Expression> Parameters { get; }

        public ParameterReplacerVisitor(Dictionary<string, Expression> parameters)
        {
            Parameters = parameters;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (Parameters.TryGetValue(node.Name, out var paramExpr))
            {
                return paramExpr;
            }
            return node;
        }
    }
}
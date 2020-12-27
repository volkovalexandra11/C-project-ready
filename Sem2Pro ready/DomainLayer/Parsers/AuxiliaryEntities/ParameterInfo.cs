using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastructure.TopDowns
{
    public class ParameterInfo
    {
        public string MainParameter { get; set; }
        public Dictionary<string, ParameterExpression> Parameters { get; }
            = new Dictionary<string, ParameterExpression>();
    }
}
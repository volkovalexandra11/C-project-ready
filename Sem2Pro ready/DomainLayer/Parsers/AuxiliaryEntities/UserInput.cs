using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastructure.TopDowns
{
    public class UserInput
    {
        public PrioritizedString FullInput { get; }

        public PrioritizedString MainExpression { get; set; }
        public Dictionary<string, PrioritizedString> ParameterExpressions { get; }

        public string MainParameter { get; set; }
        public Dictionary<string, ParameterExpression> Parameters { get; }

        public UserInput(string input)
        {
            FullInput = new PrioritizedString(input);
            ParameterExpressions = new Dictionary<string, PrioritizedString>();
            Parameters = new Dictionary<string, ParameterExpression>();
        }
    }
}
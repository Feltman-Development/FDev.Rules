using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace FDEV.Rules.Demo.Domain.Rules.Expressions
{
    /// <summary>
    /// CompiledParam class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RuleExpressionParameter
    {
        public ParameterExpression ParameterExpression { get; set; }

        public Expression ValueExpression { get; set; }

    }
}

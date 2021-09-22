using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FDEV.Rules.Demo.Domain.Rules.Results;

namespace FDEV.Rules.Demo.Domain.Rules.Actions
{
    [ExcludeFromCodeCoverage]
    public class ActionRuleResult : ActionResult
    {
        public List<RuleResultTree> Results { get; set; }
    }
}

using System.Threading.Tasks;

namespace FDEV.Rules.Demo.Domain.Rules.Base
{
    public static class RuleExtensions
    {
        public static T To<T>(this object @object) => @object != null ? (T)@object : default;

        public static T To<T>(this Task<object> @object) => @object != null ? (T)@object.Result : default;

        //public static Guid GetRuleEngineId<T>(this Rule<T> rule) where T : class, new() =>
        //    rule.Configuration.To<RuleEngineConfiguration<T>>().RuleEngineId;

        //public static string GetRuleName<T>(this Rule<T> rule) where T : class, new() => rule.GetType().Name;

        //public static IRuleResult FindRuleResult<T>(this IEnumerable<IRuleResult> ruleResults) =>
        //    ruleResults.FirstOrDefault(r => string.Equals(r.Name, typeof(T).Name, StringComparison.OrdinalIgnoreCase));

        //public static IEnumerable<IRuleResult> FindRuleResults<T>(this IEnumerable<IRuleResult> ruleResults) =>
        //    ruleResults.Where(r => string.Equals(r.Name, typeof(T).Name, StringComparison.OrdinalIgnoreCase));

        //public static IRuleResult FindRuleResult(this IEnumerable<IRuleResult> ruleResults, string ruleName) =>
        //    ruleResults.FirstOrDefault(r => string.Equals(r.Name, ruleName, StringComparison.OrdinalIgnoreCase));

        //public static IEnumerable<IRuleResult> FindRuleResults(this IEnumerable<IRuleResult> ruleResults, string ruleName) =>
        //    ruleResults.Where(r => string.Equals(r.Name, ruleName, StringComparison.OrdinalIgnoreCase));

        //public static RuleEngine<T> ApplyRules<T>(this RuleEngine<T> ruleEngineExecutor,
        //    params IGeneralRule<T>[] rules) where T : class, new()
        //{
        //    ruleEngineExecutor.AddRules(rules);

        //    return ruleEngineExecutor;
        //}

        //public static RuleEngine<T> ApplyRules<T>(this RuleEngine<T> ruleEngineExecutor,
        //    params Type[] rules) where T : class, new()
        //{
        //    ruleEngineExecutor.AddRules(rules);

        //    return ruleEngineExecutor;
        //}

        //public static IEnumerable<IRuleResult> GetErrors(this IEnumerable<IRuleResult> ruleResults)
        //    => ruleResults.Where(r => r.Error != null);

        //public static bool AnyError(this IEnumerable<IRuleResult> ruleResults) => ruleResults.Any(r => r.Error != null);


    }


}

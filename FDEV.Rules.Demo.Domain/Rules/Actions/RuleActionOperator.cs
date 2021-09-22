namespace FDEV.Rules.Demo.Domain.Rules
{
    public enum RuleActionOperator
    {
        Unknown,
        Custom,
        ReplaceWithParameter,
        SubtractParameter,
        AddParameter,
        RenameToParameter,
        Clear,
        Delete,
    }

    public enum RuleStatus
    {
        Unknown,
        InActive,
        Active,
        Success,
        Failed
    }
}

using System.Collections.Generic;

namespace FDEV.Rules.Demo.Domain.Rules.Results
{
    public interface IRuleResult
    {
        string Name { get; set; }

        object Result { get; set; }

        Dictionary<string, object> Data { get; set; }

        IError Error { get; set; }
    }
}
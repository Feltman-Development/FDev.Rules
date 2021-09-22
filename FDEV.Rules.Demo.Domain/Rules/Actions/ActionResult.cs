using System;
using System.Diagnostics.CodeAnalysis;

namespace FDEV.Rules.Demo.Domain.Rules.Actions
{
    [ExcludeFromCodeCoverage]
    public class ActionResult
    {
        public object Output { get; set; }
        public Exception Exception { get; set; }
    }
}

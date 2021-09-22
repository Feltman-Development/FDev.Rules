using System;

namespace FDEV.Rules.Demo.Domain.Rules.Results
{
    public class RuleException : Exception
    {
        public RuleException(string message) : base(message)
        {
        }

        public RuleException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RuleException() : base()
        {
        }
    }
}

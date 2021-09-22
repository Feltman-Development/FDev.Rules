using System;

namespace FDEV.Rules.Demo.Domain.Rules.Utility
{
    [Serializable]
    public sealed class AssertionException : Exception
    {
        public AssertionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AssertionException(string message) : base(message)
        {
        }

        public AssertionException() : base()
        {
        }
    }
}

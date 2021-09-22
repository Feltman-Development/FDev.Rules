using System;

namespace FDEV.Rules.Demo.Domain.Rules.Results
{
    public class Error : IError
    {
        public string Message { get; set; }

        public Exception Exception { get; set; }
    }
}

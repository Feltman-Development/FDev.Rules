using System;

namespace FDEV.Rules.Demo.Domain.Rules.Results
{
    public interface IError
    {
        string Message { get; set; }

        Exception Exception { get; set; }
    }
}
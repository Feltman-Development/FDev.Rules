using System;

namespace FDEV.Rules.Demo.Domain.Rules.Expressions
{
    public class ExpressionParserException : Exception
    {
        public ExpressionParserException(string message, string expression) : base(message)
        {
            Data.Add("Expression", expression);
        }

        public ExpressionParserException() : base()
        {
        }

        public ExpressionParserException(string message) : base(message)
        {
        }

        public ExpressionParserException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

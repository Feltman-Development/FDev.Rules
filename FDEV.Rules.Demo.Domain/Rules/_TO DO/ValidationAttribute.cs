using System;

namespace FDEV.Rules.Demo.Domain.Rules.Context
{
    public abstract class ValidationAttribute : Attribute
    {
        public string Name { get; set; }
        public string Message { get; set; }

        public ValidationAttribute()
        {

        }

        public ValidationAttribute(string name, string message)
        {
            Name = name;
            Message = message;
        }

        //public abstract BrokenRule Validate(object value, ValidationContext context);
    }

}

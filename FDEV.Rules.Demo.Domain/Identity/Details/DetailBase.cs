using System;

namespace FDEV.Rules.Demo.Domain.Identity.Details
{
    /// <summary>
    /// Adding basic meta data (name and description) to the base DetailDates record
    /// </summary>
    public abstract class DetailBase
    {
        protected DetailBase()
        {
            Id = Guid.NewGuid();
        }

        protected DetailBase(string name, string description)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }
    }
}

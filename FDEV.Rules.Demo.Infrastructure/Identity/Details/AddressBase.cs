using System.Collections.Generic;
using FRules.Demo.Engine.Domain;

namespace FSites.Domain.Identity
{
    public record AddressBase(IEnumerable<string> StreetAddress, string City, string ZipCode, AddressType Type, string Name = "")
    {
        public virtual List<string> GetFullAddress() => new List<string>();
    }
}
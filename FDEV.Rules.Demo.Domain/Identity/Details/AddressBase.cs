using System.Collections.Generic;

namespace FDEV.Rules.Demo.Domain.Identity.Details
{
    public record AddressBase(IEnumerable<string> StreetAddress, string City, string ZipCode, AddressType Type, string Name = "")
    {
        public virtual List<string> GetFullAddress() => new List<string>();
    }
}
using System.Collections.Generic;
using FDEV.Rules.Demo.Domain.Identity.Details;

namespace FDEV.Rules.Demo.Domain.Identity.Details.Identity
{
    public record AddressBase(IEnumerable<string> StreetAddress, string City, string ZipCode, AddressType Type, string Name = "")
    {
        public virtual List<string> GetFullAddress() => new List<string>();
    }
}
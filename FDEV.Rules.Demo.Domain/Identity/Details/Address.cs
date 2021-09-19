using System.Collections.Generic;

namespace FDEV.Rules.Demo.Domain.Identity.Details
{
    public class Address : DetailBase
    {
        public Address(AddressType addressType, string name, string description, string addressLine1, string addressLine2, 
                       string houseNumber, string floorNumber, string city, string postalCode, string countyRegionMunicipality, string countryOrState)
                        : base(name, description)
        {
            AddressType = addressType;
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            HouseNumber = houseNumber;
            FloorNumber = floorNumber;
            City = city;
            PostalCode = postalCode;
            CountyRegionMunicipality = countyRegionMunicipality;
            CountryOrState = countryOrState;
        }


        /// <summary>
        /// Get or set one of the listed address types
        /// </summary>
        public AddressType AddressType { get; set; }

        /// <summary>
        /// Gets or sets the first line of the address. 
        /// Save house number and floor level for designated fields.
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Gets or sets the second line of the address. 
        /// Continue first line or use for highlighting a C/O address or Postal Box Number.
        /// </summary>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Gets or sets the house number or building name.
        /// </summary>
        public string HouseNumber { get; set; }

        /// <summary>
        /// Gets or sets the floor level, if an apartment building.
        /// </summary>
        public string FloorNumber { get; set; }

        /// <summary>
        /// Gets or sets the name of the city.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the county (US), region or Municipality (most often not needed).
        /// </summary>
        public string CountyRegionMunicipality { get; set; }

        /// <summary>
        /// Gets or sets the country if not in the US, or a state if in the US.
        /// </summary>
        public string CountryOrState { get; set; }
    }

    public class Addresses : List<Address> {}
}

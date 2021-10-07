using System.Collections.Generic;
using Fluency;

namespace FDEV.Rules.Demo.Domain.Identity.Details
{
    public class Phone : DetailBase
    {
        public Phone(PhoneType phoneType, string phoneName, string countryCode, string number, string extension = "") : base(phoneType.ToString(), phoneName)
        {
            PhoneType = phoneType;
            PhoneName = phoneName;
            CountryCode = countryCode;
            Number = number;
            Extension = extension;
        }

        /// <summary>
        /// Get or set one of the listed address types
        /// </summary>
        public PhoneType PhoneType { get; set; }

        /// <summary>
        /// A friendly name for user to quickly recognise address in the UI, like ' Home'.
        /// </summary>
        public string PhoneName { get; set; }

        public string CountryCode { get; set; }
        
        public string Number { get; set; }

        public string Extension { get; set; }
    }

    public class Phones : List<Phone> {}
}

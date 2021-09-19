using System.Collections.Generic;

namespace FDEV.Rules.Demo.Domain.Identity.Details
{
    public class Phone : DetailBase
    {
        public Phone(PhoneType phoneType, string phoneName,string description, int contryCode, int number, int extension) : base(phoneName, description)
        {
            PhoneType = phoneType;
            PhoneName = phoneName;
            ContryCode = contryCode;
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

        public int ContryCode { get; set; }
        
        public int Number { get; set; }

        public int Extension { get; set; }
    }

    public class Phones : List<Phone> {}
}

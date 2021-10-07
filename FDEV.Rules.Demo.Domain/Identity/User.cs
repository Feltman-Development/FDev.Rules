using System;
using FDEV.Rules.Demo.Domain.Identity.Details;
using FDEV.Rules.Demo.Domain.Common;

namespace FDEV.Rules.Demo.Domain.Identity
{
    /// <summary>
    /// The user entity,, Create instance by using the build functionality.
    /// </summary>
    public class User : Entity, IUser
    {
        public class Build
        {
            private User _user;

            public Build(string loginName, string firstName, string lastName)
            {
                 _user.LoginName = loginName;
                 _user.FirstName = firstName;
                 _user.LastName = lastName;
            }

            public Build BirthDate(DateTime birthDate)
            {
                _user.BirthDate = birthDate; 
                return this;
            }

            public Build HomeAddress(string description, string addressLine1, string houseNumber, string floorNumber, string addressLine2, string city, string postalCode, string country)
            {
                var postal = new Address(AddressType.Postal, "Home", description, addressLine1, addressLine2,houseNumber, floorNumber, city, postalCode, "", country);
                _user.Addresses.Add(postal);
                return this;
            }

            public Build AddShipppingAddress(string description, string addressLine1, string houseNumber, string floorNumber, string addressLine2, string city, string postalCode, string country)
            {
                var postal = new Address(AddressType.Shipping, "Shipping", description, addressLine1, addressLine2,houseNumber, floorNumber, city, postalCode, "", country);
                _user.Addresses.Add(postal);
                return this;
            }

           public Build MobilePhone(string nameForPhone, string countryCode, string phoneNumber)
           {
                var mobilePhone = new Phone(PhoneType.PrivateMobile, "nameForPhone", countryCode, phoneNumber);
                return this;
           }
        }

        private User(string loginName, string firstName, string lastName)
        {
            LoginName = loginName;
            FirstName = firstName;
            LastName = lastName;
        }

        public string LoginName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Addresses Addresses { get; set; }

        public Phones Phones { get; set; }

        public Emails Emails { get; set; }

        public DateTime BirthDate { get; set; }

        public int Age => GetAge();

        private int GetAge()
        {
            int age = DateTime.Now.Year - BirthDate.Year;
            if (DateTime.Now.Month < BirthDate.Month || (DateTime.Now.Month == BirthDate.Month && DateTime.Now.Day < BirthDate.Day)) age--;
            return age;
        }
    }

    
}

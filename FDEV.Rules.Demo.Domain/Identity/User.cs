using System;
using FDEV.Rules.Demo.Domain.Identity.Details;
using FDEV.Rules.Demo.Domain.Common;

namespace FDEV.Rules.Demo.Domain.Identity
{
    public class User : Entity, IEntity, IUser
    {
        public User()
        {
        }

        public User(string loginName, string firstName, string lastName, DateTime birthDate)
        {
            LoginName = loginName;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
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

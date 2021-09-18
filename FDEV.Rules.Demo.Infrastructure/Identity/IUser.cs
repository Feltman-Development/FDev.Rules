using System;

namespace FRules.Demo.Engine.Domain
{
    public interface IUser
    {

        string LoginName { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

        DateTime BirthDate { get; }

        int Age { get; }

        Addresses Addresses { get; set; }

        Emails Emails { get; set; }
       
        Phones Phones { get; set; }
    }
}
using System;
using FDEV.Rules.Demo.Domain.Identity.Details;
using FDEV.Rules.Demo.Domain.Common;

namespace FDEV.Rules.Demo.Domain.Identity
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
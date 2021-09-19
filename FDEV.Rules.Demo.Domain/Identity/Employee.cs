using System;

namespace FDEV.Rules.Demo.Domain.Identity
{
    public class Employee : User
    {
        public Employee() {}

        public Employee(string loginName, string firstName, string lastName, DateTime birthDate, DateTime dateOfHire = default) : base(loginName, firstName, lastName, birthDate)
        {
            if (dateOfHire == default) 
                DateOfHire = DateTime.Now; 
            else
                DateOfHire = dateOfHire;
        }

        public int EmployeeNumber { get; set; }

        public DateTime DateOfHire { get; set; }
    }
}

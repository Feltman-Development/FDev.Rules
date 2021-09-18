using System.Collections.Generic;

namespace FRules.Demo.Engine.Domain
{
    public class Email : DetailBase
    {
        public Email() {}

        public Email(string emailAddress, string emailName, string description) : base(emailName, description)
        {
            EmailAddress = emailAddress;
        }

        /// <summary>
        /// Get or set one of the listed address types
        /// </summary>
        public EmailType EmailType { get; set; }

        /// <summary>
        /// A friendly name for user to quickly recognise address in the UI, like ' Home'.
        /// </summary>
        public string EmailAddress { get; set; }
    }

    public class Emails : List<Email> {}
}

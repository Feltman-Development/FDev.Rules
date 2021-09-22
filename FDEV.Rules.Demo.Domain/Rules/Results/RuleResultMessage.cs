// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FDEV.Rules.Demo.Domain.Rules.Results
{
    /// <summary>
    /// This class will hold the error messages
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RuleResultMessage
    {
        /// <summary>
        /// Constructor will initialize the List 
        /// </summary>
        public RuleResultMessage()
        {
            ErrorMessages = new List<string>();
            WarningMessages = new List<string>();
        }

        /// <summary>
        /// This will hold the list of error messages
        /// </summary>
        public List<string> ErrorMessages { get; set; }

        /// <summary>
        /// This will hold the list of warning messages
        /// </summary>
        public List<string> WarningMessages { get; set; }
    }
}

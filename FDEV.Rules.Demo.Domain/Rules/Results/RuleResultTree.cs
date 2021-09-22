// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FDEV.Rules.Demo.Domain.Rules.Actions;

namespace FDEV.Rules.Demo.Domain.Rules.Results
{
    /// <summary>
    /// Rule result class with child result heirarchy
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RuleResultTree
    {
        /// <summary>
        /// Gets or sets the rule.
        /// </summary>
        public IRule Rule { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is success.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets the child result.
        /// </summary>
        public IEnumerable<RuleResultTree> ChildResults { get; set; }

        /// <summary>
        /// Gets or sets the input object
        /// </summary>
        public Dictionary<string, object> Inputs { get; set; }

        public ActionResult ActionResult { get; set; }

        /// <summary>
        /// Gets the exception message in case an error is thrown during rules calculation.
        /// </summary>
        public string ExceptionMessage { get; set; }
    }
}

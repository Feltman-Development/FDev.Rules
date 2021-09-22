using System;
using System.Collections.Generic;
using FDEV.Rules.Demo.Core.Utility;

namespace FDEV.Rules.Demo.Domain.Rules.Actions
{
    internal class ActionFactory
    {
        private readonly IDictionary<string, Func<ActionBase>> _actionRegistry;

        internal ActionFactory() => _actionRegistry = new Dictionary<string, Func<ActionBase>>(StringComparer.OrdinalIgnoreCase);

        internal ActionFactory(IDictionary<string, Func<ActionBase>> actionRegistry) : this() => actionRegistry.Each(x => _actionRegistry.Add(x.Key, x.Value));

        internal ActionBase Get(string name)
        {
            return _actionRegistry.ContainsKey(name)
                ? _actionRegistry[name]()
                : throw new KeyNotFoundException($"Action with name: {name} does not exist");
        }
    }
}

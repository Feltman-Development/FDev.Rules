using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core.CustomTypeProviders;

namespace FDEV.Rules.Demo.Domain.Rules.Utility
{
    public class CustomTypeProvider : DefaultDynamicLinqCustomTypeProvider
    {
        public CustomTypeProvider(Type[] types) => _types = new HashSet<Type>(types ?? Array.Empty<Type>()) {typeof(ExpressionUtils)};

        public override HashSet<Type> GetCustomTypes() => _types;

        private HashSet<Type> _types;
    }
}

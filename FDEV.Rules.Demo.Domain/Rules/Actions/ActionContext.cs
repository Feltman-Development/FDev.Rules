using System;
using System.Collections.Generic;
using FDEV.Rules.Demo.Domain.Rules.Results;
using Newtonsoft.Json;

namespace FDEV.Rules.Demo.Domain.Rules.Actions
{
    public class ActionContext
    {
        private readonly IDictionary<string, string> _context;
        private readonly RuleResultTree _parentResult;

        public ActionContext(IDictionary<string, object> context, RuleResultTree parentResult)
        {
            _context = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var kv in context)
            {
                string key = kv.Key;
                string value = kv.Value.GetType().Name switch
                {
                    "String" or "JsonElement" => kv.Value.ToString(),
                    _ => JsonConvert.SerializeObject(kv.Value),
                };
                _context.Add(key, value);
            }
            _parentResult = parentResult;
        }

        public RuleResultTree GetParentRuleResult() => _parentResult;

        public T GetContext<T>(string name)
        {
            try
            {
                return typeof(T) == typeof(string)
                    ? (T)Convert.ChangeType(_context[name], typeof(T))
                    : JsonConvert.DeserializeObject<T>(_context[name]);
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException($"Argument `{name}` was not found in the action context");
            }
            catch (JsonException)
            {
                throw new ArgumentException($"Failed to convert argument `{name}` to type `{typeof(T).Name}` in the action context");
            }
        }
    }
}

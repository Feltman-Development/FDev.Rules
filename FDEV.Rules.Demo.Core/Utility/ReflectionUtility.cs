using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FDEV.Rules.Demo.Core.Utility
{
    public static class ReflectionUtility
    {
        /// <summary>
        /// Return currently loaded assembly where the specified type is defined
        /// </summary>
        public static Assembly GetAssemblyContaining(Type type) => Assembly.GetAssembly(type);

        /// <summary>
        /// Return all types derived directly from 'baseType' in the specified assembly 
        /// </summary>
        public static IEnumerable<Type> GetDerivedTypes(Type baseType, Assembly assembly) => assembly.GetTypes().Where(t => t.BaseType != null && t.BaseType == baseType);

        /// <summary> 
        /// Return all types defined on the specified generic type
        /// </summary>
        public static IEnumerable<Type> GetGenericTypes(Type genericType, Assembly assembly) => assembly.GetTypes().Where(t => t.BaseType != null && t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == genericType);

        /// <summary>
        /// Retrieve a property value from an object dynamically.
        /// This is a simple version that doesn't support indexers.
        /// </summary>
        public static object GetProperty(object instance, string property) => instance.GetType().GetProperty(property, MemberAccess)?.GetValue(instance, null);

        /// <summary>
        /// Retrieve a field dynamically from an object.
        /// This is a simple implementation that doesn't support indexers.
        /// </summary>
        public static object GetField(object Object, string property) => Object.GetType().GetField(property, MemberAccess)?.GetValue(Object);

        /// <summary>
        /// Set property value on an object.
        /// </summary>
        public static void SetProperty(object obj, string property, object value) => obj.GetType().GetProperty(property, MemberAccess)?.SetValue(obj, value, null);

        /// <summary>
        /// Set field value on an object.
        /// </summary>
        public static void SetField(object obj, string property, object value) => obj.GetType().GetField(property, MemberAccess)?.SetValue(obj, value);

        /// <summary>
        /// Calls a method on an object dynamically. This version requires explicit specification of the parameter type signatures.
        /// </summary>
        /// <param name="instance">Instance of object to call method on</param>
        /// <param name="method">The method to call as a stringToTypedValue</param>
        /// <param name="parameterTypes">Specify each of the types for each parameter passed.</param> 
        /// <param name="parms">any variable number of parameters.</param>        
        public static object CallMethod(object instance, string method, Type[] parameterTypes, params object[] parms)
        {
            return parameterTypes == null && parms.Length > 0
                ? instance.GetType().GetMethod(method, MemberAccess | BindingFlags.InvokeMethod)?.Invoke(instance, parms)
                : instance.GetType().GetMethod(method, MemberAccess | BindingFlags.InvokeMethod, null, parameterTypes, null)?.Invoke(instance, parms);
        }

        /// <summary>
        /// Calls a method on an object dynamically. Doesn't require specific parameter signatures to be passed. 
        /// Instead parameter types are inferred based on types passed. Note that if you pass a null parameter, type inferrance cannot occur and if overloads
        /// exist the call may fail. If so use the more detailed overload of this method.
        /// </summary> 
        /// <param name="instance">Instance of object to call method on</param>
        /// <param name="method">The method to call as a stringToTypedValue (using "nameOf()" is good practice)</param>
        /// <param name="parms">Any variable number of parameters.</param>        
        /// <returns>object</returns>
        public static object CallMethod(object instance, string method, params object[] parms)
        {
            // Pick up parameter types so we can match the method properly
            Type[] parameterTypes = null;
            if (parms != null)
            {
                parameterTypes = new Type[parms.Length];
                for (var x = 0; x < parms.Length; x++)
                {
                    // if we have null parameters we can't determine parameter types - exit
                    if (parms[x] == null)
                    {
                        parameterTypes = null;  // clear out - don't use types        
                        break;
                    }
                    parameterTypes[x] = parms[x].GetType();
                }
            }
            return CallMethod(instance, method, parameterTypes, parms);
        }

        /// <summary>
        /// Calls a method on an object with extended . syntax (object: this Method: Entity.CalculateOrderTotal)
        /// </summary>
        public static object CallMethodQualified(object parent, string method, object[] parameters)
        {
            while (true)
            {
                var lnAt = method.IndexOf(".", StringComparison.Ordinal);
                if (lnAt < 0)
                {
                    return CallMethod(parent, method, parameters);
                }

                // Walk the . syntax
                var main = method.Substring(0, lnAt);
                var subs = method.Substring(lnAt + 1);
                var sub = GetPropertyInternal(parent, main);

                parent = sub;
                method = subs;
            }
        }

        /// <summary>
        /// Gets a type with given name from the executing assembly collection.
        /// </summary>
        public static Type GetTypeFromName(string typeName)
        {
            Type type = null;
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = assembly.GetType(typeName, false);
                if (type != null) break;
            }
            return type;
        }

        //TODO: Refac member
        /// <summary>
        /// Parses Properties and Fields including Array and Collection references. Used internally for the extended (xxxEx) Reflection methods.
        /// </summary>
        private static object GetPropertyInternal(object parent, string property)
        {
            if (property == "this" || property == "me") return parent;

            object result = null;
            var pureProperty = property;
            string indexes = null;
            var isArrayOrCollection = false;

            if (property.IndexOf("[", StringComparison.Ordinal) > -1)
            {
                pureProperty = property.Substring(0, property.IndexOf("[", StringComparison.Ordinal));
                indexes = property.Substring(property.IndexOf("[", StringComparison.Ordinal));
                isArrayOrCollection = true;
            }

            var member = parent.GetType().GetMember(pureProperty, MemberAccess)[0];
            result = member.MemberType == MemberTypes.Property ? ((PropertyInfo)member).GetValue(parent, null) : ((FieldInfo)member).GetValue(parent);
            if (!isArrayOrCollection) return result;

            indexes = indexes.Replace("[", string.Empty).Replace("]", string.Empty);
            if (result is Array)
            {
                int.TryParse(indexes, out var index);
                return CallMethod(result, "GetValue", index);
            }
            else if (result is ICollection)
            {
                if (indexes.StartsWith("\""))
                {
                    indexes = indexes.Trim('\"');
                    return CallMethod(result, "get_Item", indexes);
                }
                else
                {
                    int.TryParse(indexes, out var index);
                    return CallMethod(result, "get_Item", index);
                }
            }
            return result;
        }

        public static IEnumerable<Type> FindDerivedTypes(Assembly assembly, Type baseType)
        {
            var baseTypeInfo = baseType.GetTypeInfo();
            bool isClass = baseTypeInfo.IsClass, isInterface = baseTypeInfo.IsInterface;

            return from type in assembly.DefinedTypes
                   where isClass
                            ? type.IsSubclassOf(baseType)
                            : isInterface && type.ImplementedInterfaces.Contains(baseTypeInfo.AsType())
                   select type.AsType();
        }

        /// <summary>
        /// Binding Flags constant to be reused for Reflection access methods.
        /// </summary>
        private const BindingFlags MemberAccess = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.IgnoreCase;
    }
}

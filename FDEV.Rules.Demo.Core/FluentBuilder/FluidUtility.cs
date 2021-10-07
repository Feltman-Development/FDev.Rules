using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FDEV.Rules.Demo.Core.FluentBuilder
{
    internal static class FluidUtility
    {
        public static List<string> GetParameterNames(ParameterInfo[] parameters) => parameters.Select(parameter => parameter.Name).ToList();

        #region String Manipulation

        /// <summary>
        /// converts FieldName to _fieldName
        /// </summary>
        public static string PublicNameToPrivate(string publicName) => "_" + PublicNameToVariableName(publicName);

        /// <summary>
        /// converts to camelCase from PascalCase
        /// </summary>
        public static string PublicNameToVariableName(string publicName) => char.ToLower(publicName[0]) + publicName[1..];

        /// <summary>
        /// converts PascalCase to camelCase
        /// </summary>
        public static string VariableNameToPublicName(string variableName) => char.ToUpper(variableName[0]) + variableName[1..];

        public static string RemoveWith(string methodName) => methodName.Replace("With", "");

        #endregion


        /// <summary>
        /// for use when a constructor parameter doesn't have a user-defined default value
        /// </summary>
        public static object DefaultValueForType(Type t) => t == typeof(string) ? null : Activator.CreateInstance(t);
    }
    }

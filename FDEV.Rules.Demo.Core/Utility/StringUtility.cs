using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace FDEV.Rules.Demo.Core.Utility
{
    public static class StringUtility
    {
        #region Utilities
        
        /// <summary>
        /// Extracts a string from between a pair of delimiters. Only the first instance is found.
        /// </summary>
        public static string ExtractString(string source, string beginDelimiter, string endDelimiter, bool caseSensitive = false, bool allowMissingEndDelimiter = false, bool returnDelimiters = false)
        {
            int beginIndex, endIndex;
            if (string.IsNullOrEmpty(source)) return string.Empty;

            if (caseSensitive)
            {
                beginIndex = source.IndexOf(beginDelimiter, StringComparison.Ordinal);
                if (beginIndex == -1) return string.Empty;

                endIndex = !returnDelimiters ? source.IndexOf(endDelimiter, beginIndex + beginDelimiter.Length, StringComparison.Ordinal) : source.IndexOf(endDelimiter, beginIndex, StringComparison.Ordinal);
            }
            else
            {
                beginIndex = source.IndexOf(beginDelimiter, 0, source.Length, StringComparison.OrdinalIgnoreCase);
                if (beginIndex == -1) return string.Empty;

                endIndex = !returnDelimiters ? source.IndexOf(endDelimiter, beginIndex + beginDelimiter.Length, StringComparison.OrdinalIgnoreCase) : source.IndexOf(endDelimiter, beginIndex, StringComparison.OrdinalIgnoreCase);
            }

            if (allowMissingEndDelimiter && endIndex == -1) return source.Substring(beginIndex + beginDelimiter.Length);
            if (beginIndex > -1 && endIndex > 1)
            {
                return !returnDelimiters ? source.Substring(beginIndex + beginDelimiter.Length, endIndex - beginIndex - beginDelimiter.Length) : source.Substring(beginIndex, endIndex - beginIndex + endDelimiter.Length);
            }   
            return string.Empty;
        }

        /// <summary>
        /// String replace function that support
        /// </summary>
        /// <returns>updated string or original string if no matches</returns>
        public static string ReplaceStringInstance(string origString, string findString, string replaceWith, int instance, bool caseInsensitive)
        {
            if (instance == -1) return ReplaceString(origString, findString, replaceWith, caseInsensitive);

            var index = 0;
            for (var i = 0; i < instance; i++)
            {
                index = caseInsensitive ? origString.IndexOf(findString, index, origString.Length - index, StringComparison.OrdinalIgnoreCase) : origString.IndexOf(findString, index);

                if (index == -1) return origString;
                else if (i < instance - 1) index += findString.Length;
            }
            return origString.Substring(0, index) + replaceWith + origString.Substring(index + findString.Length);
        }

        /// <summary>
        /// Convert string into a typed value. NOTE: Also available as '.ToTypedValue()' extension method on string. 
        /// Explicitly assigns common types and falls back on using type converters for unhandled types.         
        /// Use to consistently convert strings in UI to a type, or as help in other parsing functions.
        /// If sourcestring is null or empty, the default value of the targettype (cast to an object) will be returned;
        /// </summary>
        /// <param name="sourceString">input string value to be converted</param>
        /// <param name="culture">Culture applied to conversion of dates. Default is CurrentCulture. </param>
        /// <param name="targetType">Type to be converted to</param>
        public static object StringToTypedValue(string sourceString, Type targetType, CultureInfo culture = null)
        {
            if (string.IsNullOrEmpty(sourceString)) return (object) Activator.CreateInstance(targetType);
           
            if (culture == null) culture = CultureInfo.CurrentCulture;
            if (targetType == typeof(string)) return sourceString;
            else if (targetType == typeof(int)) return int.Parse(sourceString, NumberStyles.Any, culture.NumberFormat);
            else if (targetType == typeof(long)) return long.Parse(sourceString, NumberStyles.Any, culture.NumberFormat);
            else if (targetType == typeof(short)) return short.Parse(sourceString, NumberStyles.Any, culture.NumberFormat);
            else if (targetType == typeof(decimal)) return decimal.Parse(sourceString, NumberStyles.Any, culture.NumberFormat);
            else if (targetType == typeof(DateTime)) return DateTime.Parse(sourceString, culture.DateTimeFormat);
            else if (targetType == typeof(byte)) return ConvertValue.ToBytes(sourceString);
            else if (targetType == typeof(double)) return double.Parse(sourceString, NumberStyles.Any, culture.NumberFormat);
            else if (targetType == typeof(float)) return float.Parse(sourceString, NumberStyles.Any, culture.NumberFormat);
            else if (targetType == typeof(bool)) return sourceString.ToLower() == "true" || sourceString.ToLower() == "on" || sourceString == "1";
            else if (targetType == typeof(Guid)) return new Guid(sourceString);
            else if (targetType.IsEnum) return Enum.Parse(targetType, sourceString);
            else if (targetType == typeof(byte[])) return null; // TODO: Convert HexBinary string to byte array

            // Handle nullables explicitly since type converter won't handle conversions properly for things like decimal separators, currency formats etc.
            // Grab underlying type and pass value
            else if (targetType.Name.StartsWith("Nullable`"))
            {
                if (sourceString?.ToLower() == "null" || sourceString == string.Empty) return null;
                else return StringToTypedValue(sourceString, Nullable.GetUnderlyingType(targetType));
            }
            else
            {
                var converter = TypeDescriptor.GetConverter(targetType);
                if (!converter.CanConvertFrom(typeof(string)))
                {
                    Debug.Assert(false, $"Type Conversion not handled in StringToTypedValue for {targetType.Name} {sourceString}");
                    throw (new InvalidCastException("StringToTypedValueValueTypeConversionFailed" + targetType.Name));
                }

                // ReSharper disable once AssignNullToNotNullAttribute (it is actually allowed on the used overload. CFE, 19-05-2019)
                return converter.ConvertFromString(null, culture, sourceString);
            }
        }

        /// <summary>
        /// Replaces a substring within a string with another substring with optional case sensitivity turned off.
        /// </summary>
        /// <returns>updated string or original string if no matches</returns>
        public static string ReplaceString(string fullString, string partToReplace, string newStringPart, bool caseInsensitive)
        {
            var index = 0;
            while (true)
            {
                index = caseInsensitive ? fullString.IndexOf(partToReplace, index, fullString.Length - index, StringComparison.OrdinalIgnoreCase) : fullString.IndexOf(partToReplace, index, StringComparison.Ordinal);
                if (index == -1) break;

                fullString = fullString.Substring(0, index) + newStringPart + fullString.Substring(index + partToReplace.Length);
                index += newStringPart.Length;
            }
            return fullString;
        }

        #endregion Utilities

        #region Extensions

        /// <summary>
        /// Strip characters from left of string
        /// </summary>
        public static string StripFromLeft(this string value, int numberOfCharacters) => value.Substring(numberOfCharacters, value.Length - numberOfCharacters);

        /// <summary>
        /// Strip characters from right of string
        /// </summary>
        public static string StripFromRight(this string value, int numberOfCharacters) => value.Substring(0, value.Length - numberOfCharacters);

        /// <summary>
        /// Reverse substring. Select index from end and how many characters to walk back from there.
        /// </summary>
        public static string SubstringFromRight(this string value, int reverseStartIndex, int numberOfCharacters) => value.Substring(value.Length - reverseStartIndex - numberOfCharacters, value.Length - reverseStartIndex);

        /// <summary>
        /// Returns the string with the given prefix removed. If the prefix isn't present, the original string is returned.
        /// </summary>
        public static string RemovePrefix(this string value, string prefix) => value.Substring(prefix.Length, value.Length - prefix.Length);

        /// <summary>
        /// Automatic conversion of string into a typed value.
        /// Explicitly assigns common types and falls back on using type converters for unhandled types.         
        /// Use to consistently convert strings in UI to a type, or as help in other parsing functions.
        /// </summary>
        /// <typeparam name="T">Type to be converted to</typeparam>
        /// <param name="sourceString">input string value to be converted</param>
        /// <param name="culture">Culture applied to conversion. Default is CurrentCulture. </param>
        public static T ToTypedValue<T>(this string sourceString, CultureInfo culture = null) => (T)StringToTypedValue(sourceString, typeof(T), culture);

        #endregion Extensions
    }
}

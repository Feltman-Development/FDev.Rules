using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace FDEV.Rules.Demo.Core.Utility
{
    /// <summary>
    /// Some handy conversion methods to handle cases I often see and guarantee same result by not having multiple implementations.
    /// </summary>
    public static class ConvertValue
    {
        //TODO: Sort conversion methods between ordinary and scientific
        /// <summary>
        /// Converts to double, reading multiplicator suffixes from p (pico) to T (Tera).
        /// You can input cultureInfo. The default value is CurrentCulture, which uses the culture in the UI thread.
        /// </summary>
        public static double ToDouble(object valueIn, CultureInfo cultureInfo = null)
        {
            if (valueIn is double d) return d;

            cultureInfo ??= CultureInfo.CurrentCulture;

            var valueAsText = (string)valueIn;
            if (double.TryParse(valueAsText, NumberStyles.Float, cultureInfo, out var parsedValue)) return parsedValue;
            if (double.TryParse(valueAsText.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out parsedValue)) return parsedValue;

            // last character might be a multiplicator
            var multiplicator = valueAsText.Substring(valueAsText.Length - 1, 1);
            var multiplicatorIsCharacter = Regex.IsMatch(multiplicator, @"^[a-zA-Zµ]+$");
            if (!multiplicatorIsCharacter) throw new ArgumentException($"Can't convert {valueAsText} to double");

            var numberpart = valueAsText.Substring(0, valueAsText.Length - 1).Trim();
            if (!double.TryParse(numberpart, NumberStyles.Float, cultureInfo, out var number))
            {
                if (!double.TryParse(numberpart.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out number))
                    throw new ArgumentException($"Can't convert {valueAsText} to double");
            }

            if (multiplicator == "p") return number / 1000000000000;
            if (multiplicator == "n") return number / 1000000000;
            if (multiplicator == "µ") return number / 1000000;
            if (multiplicator == "u") return number / 1000000;
            if (multiplicator == "m") return number / 1000;
            if (multiplicator == "k") return number * 1000;
            if (multiplicator == "K") return number * 1000;
            if (multiplicator == "M") return number * 1000000;
            if (multiplicator == "G") return number * 1000000000;
            if (multiplicator == "T") return number * 1000000000000;

            throw new ArgumentException($"Can't convert {valueAsText} to double");
        }

        /// <summary>
        /// Converts a string or object to a boolean.
        /// String conversion includes true when value starts with y (yes), o (oui), t (true), e (enabled), x and 1.
        /// </summary>
        /// <param name="valueIn">Object, will be converted to string if not a bool</param>
        public static bool ToBool(object valueIn)
        {
            if (valueIn is bool b) return b;

            var valueAsText = valueIn as string;
            if (string.IsNullOrEmpty(valueAsText)) return true;
            if (bool.TryParse(valueAsText, out var result)) return result;
            return valueAsText.ToLower().StartsWith("y") ||     // yes/no
                   valueAsText.ToLower().StartsWith("o") ||     // oui/non
                   valueAsText.ToLower().StartsWith("t") ||     // true/false
                   valueAsText.ToLower().StartsWith("x") ||     // x/_
                   valueAsText.ToLower().StartsWith("e") ||     // Enabled/Disabled
                   valueAsText.ToLower().StartsWith("1");       // 1/0
        }

        public static string ToEnabledDisabled(object valueIn) => ToBool(valueIn) ? "Enabled" : "Disabled";

        public static string ToYesNo(object valueIn) => ToBool(valueIn) ? "Yes" : "No";

        public static string ToXorEmptyString(object valueIn) => ToBool(valueIn) ? "x" : string.Empty;

        /// <summary>
        /// Converts to bytes, reading multiplicator suffixes from k (kilo) to T (Tera).
        /// You can input cultureInfo. The default value is CurrentCulture, which uses the culture in the UI thread.
        /// </summary>
        public static long ToBytes(object valueIn, CultureInfo cultureInfo = null)
        {
            if (valueIn is double d) return (long)d;
            if (valueIn is long l) return l;

            cultureInfo ??= CultureInfo.CurrentCulture;

            var valueAsText = (string)valueIn;
            if (double.TryParse(valueAsText, NumberStyles.Float, cultureInfo, out var parsedValue)) return (long)parsedValue;
            if (double.TryParse(valueAsText.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out parsedValue)) return (long)parsedValue;

            // last character might be a multiplicator
            var multiplicator = valueAsText.Substring(valueAsText.Length - 1, 1);
            var multiplicatorIsCharacter = Regex.IsMatch(multiplicator, @"^[a-zA-Z]+$");
            if (!multiplicatorIsCharacter) throw new ArgumentException($"Can't convert {valueAsText} to Bytes");

            var numberpart = valueAsText.Substring(0, valueAsText.Length - 1).Trim();
            if (!double.TryParse(numberpart, NumberStyles.Float, cultureInfo, out var number))
            {
                if (!double.TryParse(numberpart.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out number))
                    throw new ArgumentException($"Can't convert {valueAsText} to Bytes");
            }

            if (multiplicator == "k") return (long)(number * 1024);
            if (multiplicator == "K") return (long)(number * 1024);
            if (multiplicator == "M") return (long)(number * 1024 * 1024);
            if (multiplicator == "G") return (long)(number * 1024 * 1024 * 1024);
            if (multiplicator == "T") return (long)(number * 1024 * 1024 * 1024 * 1024);

            throw new ArgumentException($"Can't convert {valueAsText} to Bytes");
        }

        /// <summary>
        /// Converts from bytes value to string, writing with multiplicator suffixes from K (kilo) to T (Tera).
        /// You can input cultureInfo. The default value is CurrentCulture, which uses the culture in the UI thread.
        /// </summary>
        public static string ToStringFromBytes(object valueIn, CultureInfo cultureInfo = null)
        {
            if (valueIn is not long lbytes) return string.Empty;

            cultureInfo ??= CultureInfo.CurrentCulture;
            double dbytes = lbytes;
            string[] multiplicators = new string[] { "", "K", "M", "G", "T" };
            int multiplicatorIndex = 0;
            while (dbytes > 10240 && multiplicatorIndex < multiplicators.Length - 1)
            {
                multiplicatorIndex++;
                dbytes /= 1024f;
            }
            return $"{dbytes.ToString(cultureInfo)}{multiplicators[multiplicatorIndex]}";
        }

        public static object ToType(object value, Type desiredType, object defaultValue = null) => ToTypeCoerce(desiredType, value, defaultValue);

        public static T ToType<T>(object value) => (T)ToTypeCoerce(typeof(T), value, default(T));

        private static object ToTypeCoerce(Type desiredType, object value, object defaultValue)
        {
            if (value == null) return defaultValue;

            var valueType = value.GetType();
            if (desiredType.IsAssignableFrom(valueType)) return value;

            if (desiredType.IsGenericType)
            {
                if (desiredType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    if (valueType == typeof(string) && string.IsNullOrEmpty(value.ToString())) return null;
            }

            desiredType = GetNullableUnderlyingType(desiredType);

            try
            {
                if (desiredType == typeof(string)) return value.ToString();
                if (desiredType == typeof(double)) return ToDouble(value);
                if (desiredType == typeof(bool)) return ToBool(value);
                return Convert.ChangeType(value, desiredType);
            }
            catch
            {
                var cnv = TypeDescriptor.GetConverter(desiredType);
                return cnv.CanConvertFrom(valueType) ? cnv.ConvertFrom(value) : value;
            }
        }
        private static Type GetNullableUnderlyingType(Type type) 
            => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(type) : type;
    }
}

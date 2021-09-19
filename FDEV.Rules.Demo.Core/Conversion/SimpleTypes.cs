using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace FDEV.Rules.Demo.Core.Conversion
{
    /// <summary>
    /// Some handy conversion methods to handle cases I often see and guarantee same result by not having multiple implementations.
    /// </summary>
    public static class SimpleTypes
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
            if (double.TryParse(valueAsText?.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out parsedValue)) return parsedValue;

            // last character might be a multiplicator
            if (valueAsText == null) throw new ArgumentException($"Can't convert {valueAsText} to double");

            var multiplicator = valueAsText.Substring(valueAsText.Length - 1, 1);
            var multiplicatorIsCharacter = Regex.IsMatch(multiplicator, @"^[a-zA-Zµ]+$");
            if (!multiplicatorIsCharacter) throw new ArgumentException($"Can't convert {valueAsText} to double");

            var numberPart = valueAsText[..^1].Trim();
            if (!double.TryParse(numberPart, NumberStyles.Float, cultureInfo, out var number) && !double.TryParse(numberPart.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out number))
            {
                throw new ArgumentException($"Can't convert {valueAsText} to double");
            }

            return multiplicator switch
            {
                "p" => number / 1000000000000,
                "n" => number / 1000000000,
                "µ" => number / 1000000,
                "u" => number / 1000000,
                "m" => number / 1000,
                "k" => number * 1000,
                "K" => number * 1000,
                "M" => number * 1000000,
                "G" => number * 1000000000,
                "T" => number * 1000000000000,
                _ => throw new ArgumentException($"Can't convert {valueAsText} to double")
            };
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
            return valueAsText.StartsWith("y", StringComparison.OrdinalIgnoreCase) ||     // yes/no
                   valueAsText.StartsWith("o", StringComparison.OrdinalIgnoreCase) ||     // oui/non
                   valueAsText.StartsWith("t", StringComparison.OrdinalIgnoreCase) ||     // true/false
                   valueAsText.StartsWith("x", StringComparison.OrdinalIgnoreCase) ||     // x/_
                   valueAsText.StartsWith("e", StringComparison.OrdinalIgnoreCase) ||     // Enabled/Disabled
                   valueAsText.StartsWith("1", StringComparison.OrdinalIgnoreCase);       // 1/0
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
            if (double.TryParse(valueAsText?.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out parsedValue)) return (long)parsedValue;

            // last character might be a multiplicator
            var multiplicator = valueAsText?.Substring(valueAsText.Length - 1, 1);
            var multiplicatorIsCharacter = Regex.IsMatch(multiplicator ?? string.Empty, @"^[a-zA-Z]+$");
            if (!multiplicatorIsCharacter) throw new ArgumentException($"Can't convert {valueAsText} to Bytes");

            var numberPart = valueAsText?[..^1].Trim();
            if (!double.TryParse(numberPart, NumberStyles.Float, cultureInfo, out var number) && !double.TryParse(numberPart?.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out number)) throw new ArgumentException($"Can't convert {valueAsText} to Bytes");

            return multiplicator switch
            {
                "k" => (long)(number * 1024),
                "K" => (long)(number * 1024),
                "M" => (long)(number * 1024 * 1024),
                "G" => (long)(number * 1024 * 1024 * 1024),
                "T" => (long)(number * 1024 * 1024 * 1024 * 1024),
                _ => throw new ArgumentException($"Can't convert {valueAsText} to Bytes")
            };
        }

        /// <summary>
        /// Converts from bytes value to string, writing with multiplicator suffixes from K (kilo) to T (Tera).
        /// You can input cultureInfo. The default value is CurrentCulture, which uses the culture in the UI thread.
        /// </summary>
        public static string ToStringFromBytes(object valueIn, CultureInfo cultureInfo = null)
        {
            if (valueIn is not long lBytes) return string.Empty;

            cultureInfo ??= CultureInfo.CurrentCulture;
            double dBytes = lBytes;
            string[] multiplicators = { "", "K", "M", "G", "T" };
            var multiplicatorIndex = 0;
            while (dBytes > 10240 && multiplicatorIndex < multiplicators.Length - 1)
            {
                multiplicatorIndex++;
                dBytes /= 1024f;
            }
            return $"{dBytes.ToString(cultureInfo)}{multiplicators[multiplicatorIndex]}";
        }

        public static object ToType(object value, Type desiredType, object defaultValue = null) => ToTypeCoerce(desiredType, value, defaultValue);

        public static T ToType<T>(object value) => (T)ToTypeCoerce(typeof(T), value, default(T));

        private static object ToTypeCoerce(Type desiredType, object value, object defaultValue)
        {
            if (value == null) return defaultValue;

            var valueType = value.GetType();
            if (desiredType.IsAssignableFrom(valueType)) return value;

            if (desiredType.IsGenericType && desiredType.GetGenericTypeDefinition() == typeof(Nullable<>) 
                                          && valueType == typeof(string) 
                                          && string.IsNullOrEmpty(value.ToString())) return null;

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

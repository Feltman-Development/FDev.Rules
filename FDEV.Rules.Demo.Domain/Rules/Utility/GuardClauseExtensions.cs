using System;
using System.Collections;
using System.Collections.Generic;

namespace FDEV.Rules.Demo.Domain.Rules.Utility
{
    public static class GenericObjectExtensions
    {
        public static bool IsNull(this object Object)
        {
            return Object == null || Convert.IsDBNull(Object);
        }
    }

    public static class GuardClauseExtensions
    {
        public static void NotNull<T>(T param, string paramName) => Assert(!param.IsNull(), _message, new ArgumentNullException(paramName));


        public static void NotDefault<T>(T param, string paramName)
        {
            NotNull(param, paramName);
            Assert(!param.Equals(default(T)), _message, new ArgumentNullException(paramName));
        }

        public static void NotEmpty<T>(T param, string paramName) where T : IEnumerable
        {
            NotNull(param, paramName);
            Assert(param.GetEnumerator().MoveNext(), _message, new ArgumentNullException(paramName));
        }

        public static void NotEnum<TEnum>(object param, string paramName)
        {
            Assert(typeof (TEnum).IsEnum, _message, new NotSupportedException());
            Assert(Enum.IsDefined(typeof (TEnum), param), _message, new ArgumentOutOfRangeException(paramName));
        }

        public static void NotOutOfRange<T>(T param, string paramName, T minValue, T maxValue, bool includeBound = true, IComparer<T> comparer = null) where T : IComparable<T>
        {
            Assert(param.IsBetween(minValue, maxValue, includeBound, comparer), _message, new ArgumentOutOfRangeException(paramName));
        }

        /// <summary> Assertion Exception </summary>
        /// <exception cref="AssertionException"></exception>
        public static void Assert(bool assertion)
        {
            if (!assertion)
                throw new AssertionException(_message);
        }

        /// <summary> Assertion Exception </summary>
        /// <exception cref="AssertionException"></exception>
        public static void Assert(bool assertion, string message)
        {
            if (!assertion)
                throw new AssertionException(message);
        }

        /// <summary> Assertion Exception </summary>
        /// <exception cref="AssertionException"></exception>
        public static void Assert(bool assertion, string message, Exception innerException)
        {
            if (!assertion)
                throw new AssertionException(message, innerException);
        }

        /// <summary> Assertion Exception </summary>
        /// <exception cref="AssertionException"></exception>
        public static void Assert(bool assertion, Exception innerException)
        {
            if (!assertion)
                throw new AssertionException(_message, innerException);
        }

        private static readonly string _message;
    }
}

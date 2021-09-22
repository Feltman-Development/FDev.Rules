using System;
using System.Collections.Generic;

namespace FDEV.Rules.Demo.Domain.Rules.Utility
{
    public static class ComparableExtensions
    {
        /// <summary>
        /// Check if calue is between min and max
        /// </summary>
        public static bool IsBetween<T>(this T value, T minValue, T maxValue, bool includeBound = false, IComparer<T> comparer = null) where T : IComparable<T>
        {
            comparer ??= Comparer<T>.Default;
            int minMaxCompare = comparer.Compare(minValue, maxValue);

            if (minMaxCompare < 0)
            {
                return includeBound
                    ? comparer.Compare(value, minValue) >= 0 && comparer.Compare(value, maxValue) <= 0
                    : comparer.Compare(value, minValue) > 0 && comparer.Compare(value, maxValue) < 0;
            }

            if (minMaxCompare == 0)
            {
                return includeBound ? comparer.Compare(value, minValue) == 0 : false;
            }

            return includeBound
                ? comparer.Compare(value, maxValue) >= 0 && comparer.Compare(value, minValue) <= 0
                : comparer.Compare(value, maxValue) > 0 && comparer.Compare(value, minValue) < 0;
        }
    }
}

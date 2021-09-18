using System;
using System.Collections.Generic;
using System.Linq;

namespace FDEV.Rules.Demo.Core.Utility
{
    public static class CollectionUtility
    {
        /// <summary>
        /// Set property value. If the property doesn't exist, it is created and value set.
        /// </summary>
        public static void Set<T>(this Dictionary<string, T> values, string key, T value)
        {
            if (values.ContainsKey(key))
            {
                if (values[key] == null && value == null) return;
                if (values[key]?.Equals(value) == true) return;

                values[key] = value;
            }
            else
            {
                values.Add(key, value);
            }
        }

        public static void Remove(this Dictionary<string, object> values, string key)
        {
            if (values.ContainsKey(key))
            {
                values.Remove(key);
            }
        }

        public static void Each<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }

        public static void AddIfNotNull<TItem>(this IList<TItem> items, TItem item)
        {
            if (item != null) items.Add(item);
        }

        public static void Add<TItem>(this IList<TItem> list, TItem item, int quantity) where TItem : IComparable<TItem>, IEquatable<TItem>
        {
            for (int i = 0; i < quantity; i++)
            {
                list.Add(item);
            }


            list.Add(item);
            list.Sort();
        }

        public static void AddAndSort<TItem>(this IList<TItem> list, TItem item) where TItem : IComparable<TItem>, IEquatable<TItem>
        {
            if (list.Contains(item)) return;

            list.Add(item);
            list.Sort();
        }

        public static void Sort<TItem>(this IList<TItem> list) where TItem : IComparable<TItem>, IEquatable<TItem>
        {
            var sorted = list.OrderBy(x => x).ToList();
            var pointer = 0;
            while (pointer < sorted.Count)
            {
                if (!list[pointer].Equals(sorted[pointer]))
                {
                    var t = list[pointer];
                    list.RemoveAt(pointer);
                    list.Insert(sorted.IndexOf(t), t);
                }
                else
                {
                    pointer++;
                }
            }
        }
    }
}

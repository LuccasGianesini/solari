using System;
using System.Collections.Generic;

namespace Solari.Triton.Utils
{
    internal static class EnumExt
    {
        public static IEnumerable<T> WithMax<T>(this IEnumerable<T> enumerable, Func<T, int?> valueGetter)
        {
            int? max = null;
            var maxItem = new List<T>();
            foreach (T item in enumerable)
            {
                int? val = valueGetter(item);
                if (max == null)
                {
                    max = val;
                    maxItem.Clear();
                    maxItem.Add(item);
                }
                else if (max < val)
                {
                    max = val;
                    maxItem.Clear();
                    maxItem.Add(item);
                }
                else if (max == val)
                {
                    maxItem.Add(item);
                }
            }

            return maxItem;
        }
    }
}

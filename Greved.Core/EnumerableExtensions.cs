using System.Collections.Generic;
using System.Linq;

namespace Greved.Core
{
    public static class EnumerableExtensions
    {
        public static List<T> ToList<T>(this IEnumerable<T> enumerable, int capacity)
        {
            var list = new List<T>(capacity);
            list.AddRange(enumerable);
            return list;
        }

        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int size)
        {
            T[] bucket = null;
            var count = 0;

            foreach (var item in source)
            {
                if (bucket == null)
                    bucket = new T[size];

                bucket[count++] = item;

                if (count != size)
                    continue;

                yield return bucket;

                bucket = null;
                count = 0;
            }

            if (bucket != null && count > 0)
                yield return bucket.Take(count);
        }
    }
}
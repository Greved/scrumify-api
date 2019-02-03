using System;
using System.Collections.Generic;

namespace Greved.Core
{
    public static class DictionaryExtensions
    {
        public static T2 SafeGet<T1, T2>(this IDictionary<T1, T2> source, T1 key, T2 defaultValue)
        {
            return source.TryGetValue(key, out var result) ? result : defaultValue;
        }
        public static T2 SafeGet<T1, T2>(this IDictionary<T1, T2> source, T1 key)
        {
            return source.SafeGet(key, default(T2));
        }

        public static T2 GetOrAdd<T1, T2>(this IDictionary<T1, T2> source, T1 key, Func<T1, T2> getDefaultValue)
        {
            T2 result;
            if (source.TryGetValue(key, out result)) return result;
            source.Add(key, result = getDefaultValue(key));
            return result;
        }
    }
}
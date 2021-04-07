using System;
using System.Collections.Generic;

namespace Elpida.Backend.Services.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> DistinctBy<T, TR>(this IEnumerable<T> enumerable, Func<T,TR> keyGetter)
        {
            var hashset = new HashSet<TR>();
            using var enumerator = enumerable.GetEnumerator();
            
            while (enumerator.MoveNext())
            {
                var key = keyGetter(enumerator.Current);
                if (hashset.Contains(key)) continue;
                hashset.Add(key);
                yield return enumerator.Current;
            }
        }
    }
}
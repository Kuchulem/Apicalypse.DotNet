using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apicalypse.DotNet.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool IsContainedIn<T>(this IEnumerable<T> source, IEnumerable<T> value)
        {
            foreach (var sourceValue in source)
                if (!value.Contains(sourceValue))
                    return false;

            return true;
        }

        public static bool IsExactMatch<T>(this IEnumerable<T> source, IEnumerable<T> value)
        {
            return source.Equals(value);
        }
    }
}

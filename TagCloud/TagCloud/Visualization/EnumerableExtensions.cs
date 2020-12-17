using System.Collections.Generic;

namespace TagCloud.Visualization
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<T> Cycle<T>(this IEnumerable<T> collection)
        {
            while (true)
            {
                foreach (var item in collection)
                    yield return item;
            }
        }
    }
}
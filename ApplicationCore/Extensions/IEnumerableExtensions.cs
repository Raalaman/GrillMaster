using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore
{
    public static class IEnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return !enumerable?.Any() ?? true;
        }
    }
}

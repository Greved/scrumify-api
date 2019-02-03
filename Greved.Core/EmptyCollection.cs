using System.Collections.Generic;

namespace Greved.Core
{
    public static class EmptyCollection<T>
    {
        public static readonly List<T> List = new List<T>(0);
    }
}
using System;
using System.Collections.Generic;

namespace rm.Extensions
{
    /// <summary>
    /// Generic class that implements IComparer{T}.
    /// </summary>
    public class GenericComparer<T> : IComparer<T>
    {
        private Func<T, T, int> compare;

        public GenericComparer(Func<T, T, int> compare)
        {
            compare.ThrowIfArgumentNull("compare");
            this.compare = compare;
        }

        #region IComparer<T> methods
        public int Compare(T x, T y)
        {
            return compare(x, y);
        }
        #endregion
    }
}

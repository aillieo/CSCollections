using System;
using System.Collections.Generic;

namespace AillieoUtils.Collections
{
    public class CompareSelector<TSource, TTarget> : IComparer<TSource> where TTarget : IComparable<TTarget>
    {
        private readonly Func<TSource, TTarget> selector;

        public CompareSelector(Func<TSource, TTarget> selector)
        {
            this.selector = selector;
        }

        public int Compare(TSource x, TSource y)
        {
            TTarget tx = selector(x);
            TTarget ty = selector(y);
            if (tx != null)
            {
                return tx.CompareTo(ty);
            }
            else if(ty != null)
            {
                return -ty.CompareTo(tx);
            }

            return 0;
        }
    }
}

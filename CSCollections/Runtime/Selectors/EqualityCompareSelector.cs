using System;
using System.Collections.Generic;

namespace AillieoUtils.Collections
{
    public class EqualityCompareSelector<TSource, TTarget> : IEqualityComparer<TSource>
    {
        private readonly Func<TSource, TTarget> selector;

        public EqualityCompareSelector(Func<TSource, TTarget> selector)
        {
            this.selector = selector;
        }

        public bool Equals(TSource x, TSource y)
        {
            return selector(x).Equals( selector(y));
        }

        public int GetHashCode(TSource obj)
        {
            return selector(obj).GetHashCode();
        }
    }
}

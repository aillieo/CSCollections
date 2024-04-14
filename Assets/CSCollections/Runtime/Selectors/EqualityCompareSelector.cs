// -----------------------------------------------------------------------
// <copyright file="EqualityCompareSelector.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.Collections
{
    using System;
    using System.Collections.Generic;

    public class EqualityCompareSelector<TSource, TTarget> : IEqualityComparer<TSource>
    {
        private readonly Func<TSource, TTarget> selector;

        public EqualityCompareSelector(Func<TSource, TTarget> selector)
        {
            this.selector = selector;
        }

        /// <inheritdoc/>
        public bool Equals(TSource x, TSource y)
        {
            return this.selector(x).Equals(this.selector(y));
        }

        /// <inheritdoc/>
        public int GetHashCode(TSource obj)
        {
            return this.selector(obj).GetHashCode();
        }
    }
}

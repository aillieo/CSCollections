// -----------------------------------------------------------------------
// <copyright file="CompareSelector.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.Collections
{
    using System;
    using System.Collections.Generic;

    public class CompareSelector<TSource, TTarget> : IComparer<TSource>
        where TTarget : IComparable<TTarget>
    {
        private readonly Func<TSource, TTarget> selector;

        public CompareSelector(Func<TSource, TTarget> selector)
        {
            this.selector = selector;
        }

        /// <inheritdoc/>
        public int Compare(TSource x, TSource y)
        {
            TTarget tx = this.selector(x);
            TTarget ty = this.selector(y);
            if (tx != null)
            {
                return tx.CompareTo(ty);
            }
            else if (ty != null)
            {
                return -ty.CompareTo(tx);
            }

            return 0;
        }
    }
}

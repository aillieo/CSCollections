// -----------------------------------------------------------------------
// <copyright file="PriorityQueue`2.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class PriorityQueue<TItem, TPriority> : IReadOnlyCollection<TItem>, ICollection
    {
        private const int defaultCapacity = 16;
        private readonly IComparer<TPriority> comparer;
        private Pair[] data;

        public PriorityQueue()
            : this(null)
        {
        }

        public PriorityQueue(int capacity)
            : this(capacity, null)
        {
        }

        public PriorityQueue(IComparer<TPriority> comparer)
            : this(defaultCapacity, comparer)
        {
        }

        public PriorityQueue(int capacity, IComparer<TPriority> comparer)
        {
            this.comparer = (comparer == null) ? Comparer<TPriority>.Default : comparer;
            this.data = new Pair[capacity];
        }

        /// <inheritdoc/>
        public int Count { get; private set; }

        /// <inheritdoc/>
        bool ICollection.IsSynchronized => false;

        /// <inheritdoc/>
        object ICollection.SyncRoot => this;

        public void Enqueue(TItem item, TPriority priority)
        {
            if (this.Count >= this.data.Length)
            {
                Array.Resize(ref this.data, this.Count * 2);
            }

            this.data[this.Count] = new Pair(item, priority);
            this.SiftUp(this.Count++);
        }

        public TItem Dequeue()
        {
            var v = this.Peek();
            this.data[0] = this.data[--this.Count];
            if (this.Count > 0)
            {
                this.SiftDown(0);
            }

            return v;
        }

        public void Clear()
        {
            this.Count = 0;
        }

        public TItem Peek()
        {
            if (this.Count > 0)
            {
                return this.data[0].item;
            }

            throw new Exception($"attempt to get Top from a empty {nameof(PriorityQueue<TItem, TPriority>)}");
        }

        internal TPriority PeekPriority()
        {
            if (this.Count > 0)
            {
                return this.data[0].priority;
            }

            throw new Exception($"attempt to get Top from a empty {nameof(PriorityQueue<TItem, TPriority>)}");
        }

        /// <inheritdoc/>
        public void CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (index < 0 || index >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (array.Length - index < this.Count)
            {
                throw new ArgumentException("The number of elements in the source collection is greater than the available space from index to the end of the destination array.");
            }

            for (int i = 0; i < this.Count; i++)
            {
                array.SetValue(this.data[i].item, index + i);
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <inheritdoc/>
        public IEnumerator<TItem> GetEnumerator()
        {
            for (var i = 0; i < this.Count; ++i)
            {
                yield return this.data[i].item;
            }
        }

        private void SiftUp(int n)
        {
            var v = this.data[n];
            for (var n2 = n / 2;
                n > 0 && this.comparer.Compare(v.priority, this.data[n2].priority) > 0;
                n = n2, n2 /= 2)
            {
                this.data[n] = this.data[n2];
            }

            this.data[n] = v;
        }

        private void SiftDown(int n)
        {
            var v = this.data[n];
            for (var n2 = n * 2;
                n2 < this.Count;
                n = n2, n2 *= 2)
            {
                if (n2 + 1 < this.Count && this.comparer.Compare(this.data[n2 + 1].priority, this.data[n2].priority) > 0)
                {
                    n2++;
                }

                if (this.comparer.Compare(v.priority, this.data[n2].priority) >= 0)
                {
                    break;
                }

                this.data[n] = this.data[n2];
            }

            this.data[n] = v;
        }

        private struct Pair
        {
            public TItem item;
            public TPriority priority;

            public Pair(TItem item, TPriority priority)
            {
                this.item = item;
                this.priority = priority;
            }
        }
    }
}

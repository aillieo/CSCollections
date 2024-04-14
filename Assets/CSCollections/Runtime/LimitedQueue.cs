// -----------------------------------------------------------------------
// <copyright file="LimitedQueue.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class LimitedQueue<T> : IReadOnlyCollection<T>, ICollection
    {
        private static readonly int defaultCapacity = 16;

        private readonly Queue<T> queue;
        private int size;

        public LimitedQueue(int size)
            : this(size, defaultCapacity)
        {
        }

        public LimitedQueue(int size, int capacity)
        {
            this.size = size;
            this.queue = new Queue<T>(capacity);
        }

        public int Size
        {
            get
            {
                return this.size;
            }

            set
            {
                if (this.size != value)
                {
                    if (value < this.size)
                    {
                        this.size = value;
                        this.Trim();
                    }
                    else
                    {
                        this.size = value;
                    }
                }
            }
        }

        /// <inheritdoc/>
        public int Count => this.queue.Count;

        /// <inheritdoc/>
        bool ICollection.IsSynchronized => false;

        /// <inheritdoc/>
        object ICollection.SyncRoot => this;

        public void Add(T item)
        {
            if (this.queue.Count >= this.size)
            {
                T obj = this.queue.Dequeue();
                this.queue.Enqueue(item);
            }
            else
            {
                this.queue.Enqueue(item);
            }
        }

        public bool TryAdd(T item)
        {
            if (this.queue.Count >= this.size)
            {
                return false;
            }

            this.queue.Enqueue(item);
            return true;
        }

        public float Sum(Func<T, float> selector)
        {
            return this.queue.Sum(selector);
        }

        public float Average(Func<T, float> selector)
        {
            return this.queue.Average(selector);
        }

        public void Clear()
        {
            this.queue.Clear();
        }

        public bool Contains(T item)
        {
            return this.queue.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.queue.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public void CopyTo(Array array, int index)
        {
            ((ICollection)this.queue).CopyTo(array, index);
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            return this.queue.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private void Trim()
        {
            while (this.queue.Count > this.size)
            {
                this.queue.Dequeue();
            }
        }
    }
}

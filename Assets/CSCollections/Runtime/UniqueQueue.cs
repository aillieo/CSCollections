// -----------------------------------------------------------------------
// <copyright file="UniqueQueue.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class UniqueQueue<T> : IReadOnlyCollection<T>, ICollection
    {
        private readonly HashSet<T> set;
        private readonly Queue<T> queue;

        public UniqueQueue()
        {
            this.set = new HashSet<T>();
            this.queue = new Queue<T>();
        }

        public UniqueQueue(IEqualityComparer<T> comparer)
        {
            this.set = new HashSet<T>(comparer);
            this.queue = new Queue<T>();
        }

        /// <inheritdoc/>
        public int Count => this.queue.Count;

        /// <inheritdoc/>
        bool ICollection.IsSynchronized => false;

        /// <inheritdoc/>
        object ICollection.SyncRoot => this;

        public void Clear()
        {
            this.set.Clear();
            this.queue.Clear();
        }

        public bool Contains(T item)
        {
            return this.set.Contains(item);
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

            T[] elements = this.queue.ToArray();
            Array.Copy(elements, 0, array, index, elements.Length);
        }

        public T Dequeue()
        {
            T item = this.queue.Dequeue();
            this.set.Remove(item);
            return item;
        }

        public bool Enqueue(T item)
        {
            if (this.set.Add(item))
            {
                this.queue.Enqueue(item);
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            return this.queue.GetEnumerator();
        }

        public T Peek()
        {
            return this.queue.Peek();
        }
    }
}

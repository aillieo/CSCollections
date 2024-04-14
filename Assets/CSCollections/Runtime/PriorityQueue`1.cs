// -----------------------------------------------------------------------
// <copyright file="PriorityQueue`1.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class PriorityQueue<T> : IReadOnlyCollection<T>, ICollection
    {
        private const int defaultCapacity = 16;
        private readonly IComparer<T> comparer;
        private T[] data;

        public PriorityQueue()
            : this(null)
        {
        }

        public PriorityQueue(int capacity)
            : this(capacity, null)
        {
        }

        public PriorityQueue(IComparer<T> comparer)
            : this(defaultCapacity, comparer)
        {
        }

        public PriorityQueue(int capacity, IComparer<T> comparer)
        {
            this.comparer = (comparer == null) ? Comparer<T>.Default : comparer;
            this.data = new T[capacity];
        }

        /// <inheritdoc/>
        public int Count { get; private set; }

        /// <inheritdoc/>
        bool ICollection.IsSynchronized => false;

        /// <inheritdoc/>
        object ICollection.SyncRoot => this;

        public void Enqueue(T item)
        {
            if (this.Count >= this.data.Length)
            {
                Array.Resize(ref this.data, this.Count * 2);
            }

            this.data[this.Count] = item;
            this.SiftUp(this.Count++);
        }

        public T Dequeue()
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

        public T Peek()
        {
            if (this.Count > 0)
            {
                return this.data[0];
            }

            throw new Exception($"attempt to get Top from a empty {nameof(PriorityQueue<T>)}");
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

            if (array.Rank != 1)
            {
                throw new ArgumentException("The destination array must be a one-dimensional array.");
            }

            if (array is T[] tArray)
            {
                Array.Copy(this.data, 0, tArray, index, this.Count);
            }
            else
            {
                var targetType = array.GetType().GetElementType();
                if (!typeof(T).IsAssignableFrom(targetType))
                {
                    throw new ArgumentException("The type of the source collection cannot be cast automatically to the type of the destination array.");
                }

                var destinationArray = Array.CreateInstance(targetType, this.Count);
                Array.Copy(this.data, 0, destinationArray, 0, this.Count);
                destinationArray.CopyTo(array, index);
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < this.Count; ++i)
            {
                yield return this.data[i];
            }
        }

        private void SiftUp(int n)
        {
            var v = this.data[n];
            for (var n2 = n / 2;
                n > 0 && this.comparer.Compare(v, this.data[n2]) > 0;
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
                if (n2 + 1 < this.Count && this.comparer.Compare(this.data[n2 + 1], this.data[n2]) > 0)
                {
                    n2++;
                }

                if (this.comparer.Compare(v, this.data[n2]) >= 0)
                {
                    break;
                }

                this.data[n] = this.data[n2];
            }

            this.data[n] = v;
        }
    }
}

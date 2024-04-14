// -----------------------------------------------------------------------
// <copyright file="UniquePriorityQueue.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class UniquePriorityQueue<T> : IReadOnlyCollection<T>, ICollection
    {
        private const int defaultCapacity = 16;
        private readonly IComparer<T> comparer;
        private List<T> data;
        private Dictionary<T, int> set;

        public UniquePriorityQueue()
            : this(defaultCapacity, null)
        {
        }

        public UniquePriorityQueue(int capacity)
            : this(capacity, null)
        {
        }

        public UniquePriorityQueue(IComparer<T> comparer)
            : this(defaultCapacity, comparer)
        {
        }

        public UniquePriorityQueue(int capacity, IComparer<T> comparer)
        {
            this.comparer = (comparer == null) ? Comparer<T>.Default : comparer;
            this.data = new List<T>(capacity);

            var equalityComparer = new EqualityComparer(this.comparer);
            this.set = new Dictionary<T, int>(capacity, equalityComparer);
        }

        /// <inheritdoc/>
        public int Count { get; private set; }

        /// <inheritdoc/>
        bool ICollection.IsSynchronized => false;

        /// <inheritdoc/>
        object ICollection.SyncRoot => this;

        public bool Enqueue(T item)
        {
            if (this.set.ContainsKey(item))
            {
                return false;
            }

            while (this.Count >= this.data.Count)
            {
                this.data.Add(default);
            }

            this.data[this.Count] = item;
            this.set[item] = this.Count;
            this.SiftUp(this.Count++);

            return true;
        }

        public T Dequeue()
        {
            T v = this.Peek();
            this.set.Remove(v);

            this.Count--;

            T last = this.data[this.Count];
            this.data[this.Count] = default;
            if (this.Count > 0)
            {
                this.data[0] = last;
                this.set[last] = 0;
            }

            if (this.Count > 0)
            {
                this.SiftDown(0);
            }

            return v;
        }

        public void Clear()
        {
            this.Count = 0;
            this.set.Clear();
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

            if (array is T[] destinationArray)
            {
                Array.Copy(this.data.ToArray(), 0, destinationArray, index, this.Count);
            }
            else
            {
                for (int i = 0; i < this.Count; i++)
                {
                    array.SetValue(this.data[i], index++);
                }
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

        public bool Remove(T item)
        {
            if (this.set.TryGetValue(item, out var index))
            {
                this.data[index] = default;
                this.set.Remove(item);

                for (var i = index; i < this.Count - 1; ++i)
                {
                    T v0 = this.data[i + 1];
                    this.data[i] = v0;
                    this.set[v0] = i;
                }

                this.Count--;

                if (index > 0)
                {
                    this.SiftDown(index - 1);
                }

                return true;
            }

            return false;
        }

        private void SiftUp(int n)
        {
            T v = this.data[n];
            for (var n2 = n / 2;
                n > 0 && this.comparer.Compare(v, this.data[n2]) > 0;
                n = n2, n2 /= 2)
            {
                T v0 = this.data[n2];
                this.data[n] = v0;
                this.set[v0] = n;
            }

            this.data[n] = v;
            this.set[v] = n;
        }

        private void SiftDown(int n)
        {
            T v = this.data[n];
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

                T v0 = this.data[n2];
                this.data[n] = v0;
                this.set[v0] = n;
            }

            this.data[n] = v;
            this.set[v] = n;
        }

        private class EqualityComparer : IEqualityComparer<T>
        {
            private readonly IComparer<T> comparer;

            public EqualityComparer(IComparer<T> comparer)
            {
                this.comparer = comparer;
            }

            public bool Equals(T x, T y)
            {
                return this.comparer.Compare(x, y) == 0;
            }

            public int GetHashCode(T obj)
            {
                if (obj == null)
                {
                    return 0;
                }

                return obj.GetHashCode();
            }
        }
    }
}

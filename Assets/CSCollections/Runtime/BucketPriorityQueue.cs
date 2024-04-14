// -----------------------------------------------------------------------
// <copyright file="BucketPriorityQueue.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class BucketPriorityQueue<TItem, TPriority> : IReadOnlyCollection<TItem>, ICollection
    {
        private const int defaultPartition = 8;

        private readonly PriorityQueue<TItem, TPriority>[] queues;
        private readonly TPriority priorityMin;
        private readonly TPriority priorityMax;
        private readonly int partitions;
        private readonly IComparer<TPriority> comparer;

        private int firstNonEmpty = -1;

        public BucketPriorityQueue(int partitions, TPriority expectedPriorityMin, TPriority expectedPriorityMax)
        {
            if (partitions <= 0)
            {
                throw new ArgumentException("The number of partitions must be greater than zero.");
            }

            this.partitions = partitions;
            this.priorityMin = expectedPriorityMin;
            this.priorityMax = expectedPriorityMax;

            this.queues = new PriorityQueue<TItem, TPriority>[partitions];
            for (int i = 0; i < partitions; i++)
            {
                this.queues[i] = new PriorityQueue<TItem, TPriority>();
            }
        }

        public BucketPriorityQueue(int partitions, Func<TPriority, int> partitionFunc)
            : this(partitions, default(TPriority), default(TPriority))
        {
        }

        public BucketPriorityQueue(int partitions, TPriority expectedPriorityMin, TPriority expectedPriorityMax, IComparer<TPriority> comparer)
            : this(partitions, expectedPriorityMin, expectedPriorityMax)
        {
            this.comparer = (comparer == null) ? Comparer<TPriority>.Default : comparer;
        }

        public BucketPriorityQueue(int partitions, Func<TPriority, int> partitionFunc, IComparer<TPriority> comparer)
            : this(partitions, default(TPriority), default(TPriority), comparer)
        {
        }

        /// <inheritdoc/>
        public int Count { get; private set; }

        /// <inheritdoc/>
        bool ICollection.IsSynchronized => false;

        /// <inheritdoc/>
        object ICollection.SyncRoot => this;

        public void Enqueue(TItem item, TPriority priority)
        {
            int bucket = this.GetBucket(priority);
            this.queues[bucket].Enqueue(item, priority);
            this.Count++;

            if (this.firstNonEmpty == -1 || Comparer<TPriority>.Default.Compare(priority, this.queues[this.firstNonEmpty].PeekPriority()) < 0)
            {
                this.firstNonEmpty = bucket;
            }
        }

        public TItem Dequeue()
        {
            if (this.Count == 0)
            {
                throw new InvalidOperationException("The queue is empty.");
            }

            TItem item = this.queues[this.firstNonEmpty].Dequeue();
            this.Count--;

            if (this.queues[this.firstNonEmpty].Count == 0)
            {
                this.FindNextNonEmptyBucket();
            }

            return item;
        }

        public void Clear()
        {
            for (int i = 0; i < this.partitions; i++)
            {
                this.queues[i].Clear();
            }

            this.Count = 0;
            this.firstNonEmpty = -1;
        }

        public TItem Peek()
        {
            if (this.Count == 0)
            {
                throw new InvalidOperationException("The queue is empty.");
            }

            return this.queues[this.firstNonEmpty].Peek();
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
                throw new ArgumentOutOfRangeException(nameof(index), "The index is out of range.");
            }

            if (array.Length - index < this.Count)
            {
                throw new ArgumentException("The destination array is not large enough to hold the elements.", nameof(array));
            }

            int arrayIndex = index;
            foreach (var queue in this.queues)
            {
                foreach (var item in queue)
                {
                    array.SetValue(item, arrayIndex);
                    arrayIndex++;
                }
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
            foreach (var queue in this.queues)
            {
                foreach (var item in queue)
                {
                    yield return item;
                }
            }
        }

        private int GetBucket(TPriority priority)
        {
            for (int i = 0; i < this.partitions; i++)
            {
                if (this.comparer.Compare(priority, this.queues[i].PeekPriority()) <= 0)
                {
                    return i;
                }
            }

            throw new InvalidOperationException("No bucket found for the given priority.");
        }

        private void FindNextNonEmptyBucket()
        {
            for (int i = 1; i < this.partitions; i++)
            {
                int index = (this.firstNonEmpty + i) % this.partitions;
                if (this.queues[index].Count > 0)
                {
                    this.firstNonEmpty = index;
                    return;
                }
            }

            this.firstNonEmpty = -1;
        }
    }
}

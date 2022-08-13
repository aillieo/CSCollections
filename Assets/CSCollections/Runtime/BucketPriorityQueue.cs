namespace AillieoUtils.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class BucketPriorityQueue<TItem, TPriority> : IReadOnlyCollection<TItem>, ICollection
    {
        private readonly PriorityQueue<TItem, TPriority>[] queues;
        private readonly TPriority priorityMin;
        private readonly TPriority priorityMax;
        private readonly int partitions;
        private readonly IComparer<TPriority> comparer;

        private const int defaultPartition = 8;

        private int firstNonEmpty = -1;

        public int Count { get; private set; }

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => this;

        public BucketPriorityQueue(int partitions, TPriority expectedPriorityMin, TPriority expectedPriorityMax)
        {
            throw new NotImplementedException();
        }

        public BucketPriorityQueue(int partitions, Func<TPriority, int> partitionFunc)
        {
            throw new NotImplementedException();
        }

        public BucketPriorityQueue(int partitions, TPriority expectedPriorityMin, TPriority expectedPriorityMax, IComparer<TPriority> comparer)
        {
            throw new NotImplementedException();
        }

        public BucketPriorityQueue(int partitions, Func<TPriority, int> partitionFunc, IComparer<TPriority> comparer)
        {
            throw new NotImplementedException();
        }

        public void Enqueue(TItem item, TPriority priority)
        {
            throw new NotImplementedException();
        }

        public TItem Dequeue()
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public TItem Peek()
        {
            if (Count > 0)
            {
                throw new NotImplementedException();
            }

            throw new Exception($"attempt to get Top from a empty {nameof(PriorityQueue<TItem, TPriority>)}");
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        private int GetBucket(TPriority priority)
        {
            throw new NotImplementedException();
        }
    }
}

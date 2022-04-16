using System;
using System.Collections;
using System.Collections.Generic;

namespace AillieoUtils.Collections
{
    public class BucketPriorityQueue<TItem, TPriority> : IEnumerable<TItem>, IEnumerable, IReadOnlyCollection<TItem>, ICollection
    {
        private readonly PriorityQueue<TItem, TPriority>[] queues;
        private readonly TPriority priorityMin;
        private readonly TPriority priorityMax;
        private readonly int partitions;
        private readonly IComparer<TPriority> comparer;

        private const int defaultPartition = 8;

        private int firstNonEmpty = -1;

        public int Count { get; private set; }

        public bool IsSynchronized => throw new NotImplementedException();

        public object SyncRoot => throw new NotImplementedException();

        public BucketPriorityQueue(int partitions, TPriority expectedPriorityMin, TPriority expectedPriorityMax)
        {
        }

        public BucketPriorityQueue(int partitions, Func<TPriority, int> partitionFunc)
        {
        }

        public BucketPriorityQueue(int partitions, TPriority expectedPriorityMin, TPriority expectedPriorityMax, IComparer<TPriority> comparer)
        {

        }

        public BucketPriorityQueue(int partitions, Func<TPriority, int> partitionFunc, IComparer<TPriority> comparer)
        {

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

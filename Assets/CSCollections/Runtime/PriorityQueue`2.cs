using System;
using System.Collections;
using System.Collections.Generic;

namespace AillieoUtils.Collections
{
    public class PriorityQueue<TItem, TPriority> : IReadOnlyCollection<TItem>, ICollection
    {
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

        private readonly IComparer<TPriority> comparer;
        private Pair[] data;
        private const int defaultCapacity = 16;

        public int Count { get; private set; }

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => this;

        public PriorityQueue()
            : this(null) { }

        public PriorityQueue(int capacity)
            : this(capacity, null) { }

        public PriorityQueue(IComparer<TPriority> comparer)
            : this(defaultCapacity, comparer) { }

        public PriorityQueue(int capacity, IComparer<TPriority> comparer)
        {
            this.comparer = (comparer == null) ? Comparer<TPriority>.Default : comparer;
            this.data = new Pair[capacity];
        }

        public void Enqueue(TItem item, TPriority priority)
        {
            if (Count >= data.Length)
            {
                Array.Resize(ref data, Count * 2);
            }

            data[Count] = new Pair(item, priority);
            SiftUp(Count++);
        }

        public TItem Dequeue()
        {
            var v = Peek();
            data[0] = data[--Count];
            if (Count > 0)
            {
                SiftDown(0);
            }
            return v;
        }

        public void Clear()
        {
            Count = 0;
        }

        public TItem Peek()
        {
            if (Count > 0)
            {
                return data[0].item;
            }

            throw new Exception($"attempt to get Top from a empty {nameof(PriorityQueue<TItem, TPriority>)}");
        }

        private void SiftUp(int n)
        {
            var v = data[n];
            for (var n2 = n / 2;
                n > 0 && comparer.Compare(v.priority, data[n2].priority) > 0;
                n = n2, n2 /= 2)
            {
                data[n] = data[n2];
            }

            data[n] = v;
        }

        private void SiftDown(int n)
        {
            var v = data[n];
            for (var n2 = n * 2;
                n2 < Count;
                n = n2, n2 *= 2)
            {
                if (n2 + 1 < Count && comparer.Compare(data[n2 + 1].priority, data[n2].priority) > 0)
                {
                    n2++;
                }

                if (comparer.Compare(v.priority, data[n2].priority) >= 0)
                {
                    break;
                }

                data[n] = data[n2];
            }
            data[n] = v;
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
            for (int i = 0; i < Count; ++i)
            {
                yield return data[i].item;
            }
        }
    }
}

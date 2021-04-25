using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AillieoUtils.Collections
{
    public class SlidingWindow<T> : IEnumerable<T>, IEnumerable, ICollection<T>, ICollection
    {
        private readonly Queue<T> queue;
        private int size;

        private static readonly int defaultCapacity = 16;

        public SlidingWindow(int size):
            this(size, defaultCapacity)
        {
        }

        public SlidingWindow(int size, int capacity)
        {
            queue = new Queue<T>(capacity);
        }

        public int Size
        {
            get { return size; }
            set
            {
                if (size != value)
                {
                    if (value < size)
                    {
                        size = value;
                        Trim();
                    }
                    else
                    {
                        size = value;
                    }
                }
            }
        }

        private void Trim()
        {
            while (queue.Count > size)
            {
                queue.Dequeue();
            }
        }

        public int Count => queue.Count;

        public void Add(T item)
        {
            if (queue.Count >= size)
            {
                queue.Dequeue();
            }

            queue.Enqueue(item);
        }

        public float Sum(Func<T, float> selector)
        {
            return queue.Sum(selector);
        }

        public float Average(Func<T, float> selector)
        {
            return queue.Average(selector);
        }

        public bool IsReadOnly => false;

        public bool IsSynchronized => throw new NotImplementedException();

        public object SyncRoot => throw new NotImplementedException();

        public void Clear()
        {
            queue.Clear();
        }

        public bool Contains(T item)
        {
            return queue.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return queue.GetEnumerator();
        }

        public bool Remove(T item)
        {
            return ((ICollection<T>)queue).Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}

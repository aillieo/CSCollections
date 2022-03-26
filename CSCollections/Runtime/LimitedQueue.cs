using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AillieoUtils.Collections
{
    public class LimitedQueue<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection
    {
        public event Action<T> onPop;

        private readonly Queue<T> queue;
        private int size;

        private static readonly int defaultCapacity = 16;

        public LimitedQueue(int size):
            this(size, defaultCapacity)
        {
        }

        public LimitedQueue(int size, int capacity)
        {
            this.size = size;
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
            List<Exception> exceptions = null;
            while (queue.Count > size)
            {
                T obj = queue.Dequeue();
                try
                {
                    onPop?.Invoke(obj);
                }
                catch(Exception e)
                {
                    if(exceptions == null)
                    {
                        exceptions = new List<Exception>();
                    }
                    exceptions.Add(e);
                }
            }

            if(exceptions != null)
            {
                throw new AggregateException(exceptions);
            }
        }

        public int Count => queue.Count;

        public void Add(T item)
        {
            if (queue.Count >= size)
            {
                T obj = queue.Dequeue();
                queue.Enqueue(item);

                onPop?.Invoke(obj);
            }
            else
            {
                queue.Enqueue(item);
            }
        }

        public bool TryAdd(T item)
        {
            if (queue.Count >= size)
            {
                return false;
            }

            queue.Enqueue(item);
            return true;
        }

        public float Sum(Func<T, float> selector)
        {
            return queue.Sum(selector);
        }

        public float Average(Func<T, float> selector)
        {
            return queue.Average(selector);
        }

        public bool IsSynchronized => ((ICollection)queue).IsSynchronized;

        public object SyncRoot => ((ICollection)queue).SyncRoot;

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
            queue.CopyTo(array, arrayIndex);
        }

        public void CopyTo(Array array, int index)
        {
            ((ICollection)queue).CopyTo(array, index);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return queue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

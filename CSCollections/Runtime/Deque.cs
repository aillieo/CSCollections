using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AillieoUtils.Collections
{
    public class Deque<T> : ICollection<T>, ICollection
    {
        private static readonly int defaultCapacity = 8;

        private int version; // todo

        private T[] buffer;

        private int offset;

        public Deque() : this(defaultCapacity)
        {
        }

        public Deque(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentException(nameof(capacity), "should >= 0");
            }

            buffer = capacity == 0 ? Array.Empty<T>() : new T[capacity];
        }

        public void PushRight(T item)
        {
            EnsureCapacity(Count + 1);

            Count++;
            Right = item;

            version++;
        }

        public void PushLeft(T item)
        {
            EnsureCapacity(Count + 1);

            offset = SafeGetIndex(offset - 1);
            Count++;
            Left = item;

            version++;
        }

        public T PopRight()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Attempt to get item from an empty deque");
            }

            var right = Right;
            Right = default;
            Count--;

            version++;
            return right;
        }

        public T PopLeft()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Attempt to get item from an empty deque");
            }

            var left = Left;
            Left = default;
            offset = SafeGetIndex(offset + 1);
            Count--;

            version++;
            return left;
        }

        public T PeekRight()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Attempt to get item from an empty deque");
            }

            return Right;
        }

        public T PeekLeft()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Attempt to get item from an empty deque");
            }

            return Left;
        }

        public void Clear()
        {
            if (LoopsAround)
            {
                Array.Clear(buffer, offset, Capacity - offset);
                Array.Clear(buffer, 0, offset + (Count - Capacity));
            }
            else
            {
                Array.Clear(buffer, offset, Count);
            }

            Count = 0;
            offset = 0;
            version++;
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void ICollection<T>.Add(T item)
        {
            PushRight(item);
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            return this.Contains(item, EqualityComparer<T>.Default);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (arrayIndex < 0 || arrayIndex >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            }

            if (Count == 0)
            {
                return;
            }

            if (array.Length - arrayIndex < Count)
            {
                throw new ArgumentException(nameof(array));
            }

            if (LoopsAround)
            {
                Array.Copy(buffer, offset, array, arrayIndex, Capacity - offset);
                Array.Copy(buffer, 0, array, arrayIndex + Capacity - offset, offset + (Count - Capacity));
            }
            else
            {
                Array.Copy(buffer, offset, array, arrayIndex, Count);
            }
        }

        public void TrimExcess()
        {
            Capacity = Count;
        }

        private void EnsureCapacity(int min)
        {
            if (Capacity < min)
            {
                var newCapacity = Capacity == 0 ? defaultCapacity : Capacity * 2;
                newCapacity = Math.Max(newCapacity, min);
                Capacity = newCapacity;
            }
        }

        private int SafeGetIndex(int position)
        {
            if (Capacity != 0)
            {
                return (position + Capacity) % Capacity;
            }

            return 0;
        }

        private int GetIndexInBuffer(int index)
        {
            if (index < 0 || index >= Capacity)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            return SafeGetIndex(offset + index);
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public int Count { get; private set; }

        public bool IsEmpty
        {
            get { return Count == 0; }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        public int Capacity
        {
            get
            {
                return buffer.Length;
            }

            private set
            {
                if (value == buffer.Length)
                {
                    return;
                }

                if (value < Count)
                {
                    throw new Exception($"capacity < count : {value},{Count}");
                }

                T[] newBuffer = new T[value];
                CopyTo(newBuffer, 0);
                offset = 0;
                buffer = newBuffer;
            }
        }

        private bool LoopsAround => Count > Capacity - offset;

        private T Left
        {
            get { return this[0]; }
            set { this[0] = value; }
        }

        private T Right
        {
            get { return this[Count - 1]; }
            set { this[Count - 1] = value; }
        }

        public bool IsSynchronized => throw new NotImplementedException();

        public object SyncRoot => throw new NotImplementedException();

        public T this[int index]
        {
            get { return buffer[GetIndexInBuffer(index)]; }
            set { buffer[GetIndexInBuffer(index)] = value; }
        }
    }
}

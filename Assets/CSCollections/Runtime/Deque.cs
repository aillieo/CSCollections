// -----------------------------------------------------------------------
// <copyright file="Deque.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a double-ended queue (deque) data structure.
    /// </summary>
    /// <typeparam name="T">The type of elements in the deque.</typeparam>
    public class Deque<T> : IReadOnlyCollection<T>, ICollection
    {
        private static readonly int defaultCapacity = 8;

        private int version; // todo

        private T[] buffer;

        private int offset;

        /// <summary>
        /// Initializes a new instance of the <see cref="Deque{T}"/> class with the default capacity.
        /// </summary>
        public Deque()
            : this(defaultCapacity)
        {
        }

        public Deque(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentException("should >= 0", nameof(capacity));
            }

            this.buffer = capacity == 0 ? Array.Empty<T>() : new T[capacity];
        }

        /// <inheritdoc/>
        public int Count { get; private set; }

        public int Capacity
        {
            get
            {
                return this.buffer.Length;
            }

            private set
            {
                if (value == this.buffer.Length)
                {
                    return;
                }

                if (value < this.Count)
                {
                    throw new Exception($"capacity < count : {value},{this.Count}");
                }

                var newBuffer = new T[value];
                this.CopyTo(newBuffer, 0);
                this.offset = 0;
                this.buffer = newBuffer;
            }
        }

        /// <inheritdoc/>
        bool ICollection.IsSynchronized => false;

        /// <inheritdoc/>
        object ICollection.SyncRoot => this;

        private bool LoopsAround => this.Count > this.Capacity - this.offset;

        private T Left
        {
            get { return this[0]; }
            set { this[0] = value; }
        }

        private T Right
        {
            get { return this[this.Count - 1]; }
            set { this[this.Count - 1] = value; }
        }

        public T this[int index]
        {
            get { return this.buffer[this.GetIndexInBuffer(index)]; }
            set { this.buffer[this.GetIndexInBuffer(index)] = value; }
        }

        public void PushRight(T item)
        {
            this.EnsureCapacity(this.Count + 1);

            this.Count++;
            this.Right = item;

            this.version++;
        }

        public void PushLeft(T item)
        {
            this.EnsureCapacity(this.Count + 1);

            this.offset = this.SafeGetIndex(this.offset - 1);
            this.Count++;
            this.Left = item;

            this.version++;
        }

        public T PopRight()
        {
            if (this.Count == 0)
            {
                throw new InvalidOperationException("Attempt to get item from an empty deque");
            }

            var right = this.Right;
            this.Right = default;
            this.Count--;

            this.version++;
            return right;
        }

        public T PopLeft()
        {
            if (this.Count == 0)
            {
                throw new InvalidOperationException("Attempt to get item from an empty deque");
            }

            var left = this.Left;
            this.Left = default;
            this.offset = this.SafeGetIndex(this.offset + 1);
            this.Count--;

            this.version++;
            return left;
        }

        public T PeekRight()
        {
            if (this.Count == 0)
            {
                throw new InvalidOperationException("Attempt to get item from an empty deque");
            }

            return this.Right;
        }

        public T PeekLeft()
        {
            if (this.Count == 0)
            {
                throw new InvalidOperationException("Attempt to get item from an empty deque");
            }

            return this.Left;
        }

        public void Clear()
        {
            if (this.LoopsAround)
            {
                Array.Clear(this.buffer, this.offset, this.Capacity - this.offset);
                Array.Clear(this.buffer, 0, this.offset + (this.Count - this.Capacity));
            }
            else
            {
                Array.Clear(this.buffer, this.offset, this.Count);
            }

            this.Count = 0;
            this.offset = 0;
            this.version++;
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < this.Count; i++)
            {
                yield return this[i];
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
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

            if (this.Count == 0)
            {
                return;
            }

            if (array.Length - arrayIndex < this.Count)
            {
                throw new ArgumentException("Length not enough", nameof(array));
            }

            if (this.LoopsAround)
            {
                Array.Copy(this.buffer, this.offset, array, arrayIndex, this.Capacity - this.offset);
                Array.Copy(this.buffer, 0, array, arrayIndex + this.Capacity - this.offset, this.offset + (this.Count - this.Capacity));
            }
            else
            {
                Array.Copy(this.buffer, this.offset, array, arrayIndex, this.Count);
            }
        }

        public void TrimExcess()
        {
            this.Capacity = this.Count;
        }

        /// <inheritdoc/>
        public void CopyTo(Array array, int index)
        {
            this.CopyTo(array as T[], index);
        }

        private void EnsureCapacity(int min)
        {
            if (this.Capacity < min)
            {
                var newCapacity = this.Capacity == 0 ? defaultCapacity : this.Capacity * 2;
                newCapacity = Math.Max(newCapacity, min);
                this.Capacity = newCapacity;
            }
        }

        private int SafeGetIndex(int position)
        {
            if (this.Capacity != 0)
            {
                return (position + this.Capacity) % this.Capacity;
            }

            return 0;
        }

        private int GetIndexInBuffer(int index)
        {
            if (index < 0 || index >= this.Capacity)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return this.SafeGetIndex(this.offset + index);
        }
    }
}

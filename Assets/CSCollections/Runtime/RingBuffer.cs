// -----------------------------------------------------------------------
// <copyright file="RingBuffer.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class RingBuffer<T> : IEnumerable<T>
    {
        private readonly T[] buffer;
        private int cursor;

        public RingBuffer(int size)
        {
            this.buffer = new T[size];
        }

        public T Current => this.buffer[this.cursor];

        /// <inheritdoc/>
        public void Add(T item)
        {
            this.buffer[this.cursor] = item;
            this.cursor = (this.cursor + 1) % this.buffer.Length;
        }

        /// <inheritdoc/>
        public void Clear()
        {
            Array.Clear(this.buffer, 0, this.buffer.Length);
            this.cursor = 0;
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < this.buffer.Length; i++)
            {
                yield return this.buffer[i];
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}

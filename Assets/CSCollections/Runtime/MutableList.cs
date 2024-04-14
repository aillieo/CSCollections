// -----------------------------------------------------------------------
// <copyright file="MutableList.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class MutableList<T> : IList<T>
    {
        private readonly List<T> list;
        private readonly HashSet<int> removedIndexes;
        private readonly Dictionary<int, T> modifiedValues;
        private int iterationLock;

        public MutableList()
        {
            this.list = new List<T>();
            this.removedIndexes = new HashSet<int>();
            this.modifiedValues = new Dictionary<int, T>();
        }

        /// <inheritdoc/>
        public int Count { get; }

        /// <inheritdoc/>
        bool ICollection<T>.IsReadOnly => false;

        /// <inheritdoc/>
        public T this[int index]
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <inheritdoc/>
        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Clear()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        private void Rebuild()
        {
            if (this.removedIndexes.Count == 0 && this.modifiedValues.Count == 0)
            {
                return;
            }

            throw new NotImplementedException();
        }

        [Serializable]
        public struct Enumerator : IEnumerator<T>
        {
            private MutableList<T> mutable;
            private List<T>.Enumerator enumerator;
            private T current;

            internal Enumerator(MutableList<T> mutable)
            {
                this.mutable = mutable;
                this.enumerator = mutable.list.GetEnumerator();
                this.current = default;
                mutable.iterationLock++;
            }

            /// <inheritdoc/>
            public T Current => this.current;

            /// <inheritdoc/>
            object IEnumerator.Current => this.Current;

            /// <inheritdoc/>
            public bool MoveNext()
            {
                throw new NotImplementedException();
            }

            /// <inheritdoc/>
            public void Reset()
            {
                throw new NotImplementedException();
            }

            /// <inheritdoc/>
            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }
    }
}

// -----------------------------------------------------------------------
// <copyright file="UniqueList.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class UniqueList<T> : IList<T>, IList, IReadOnlyCollection<T>
    {
        private readonly HashSet<T> set;
        private readonly List<T> list;

        public UniqueList()
        {
            this.set = new HashSet<T>();
            this.list = new List<T>();
        }

        public UniqueList(IEqualityComparer<T> comparer)
        {
            this.set = new HashSet<T>(comparer);
            this.list = new List<T>();
        }

        /// <inheritdoc/>
        public int Count => this.list.Count;

        /// <inheritdoc/>
        bool ICollection.IsSynchronized => false;

        /// <inheritdoc/>
        object ICollection.SyncRoot => this;

        /// <inheritdoc/>
        bool ICollection<T>.IsReadOnly => false;

        /// <inheritdoc/>
        bool IList.IsReadOnly => false;

        /// <inheritdoc/>
        bool IList.IsFixedSize => false;

        /// <inheritdoc/>
        public T this[int index]
        {
            get => this.list[index];
            set
            {
                if (this.set.Contains(value))
                {
                    throw new ArgumentException("Item already exists in the list.");
                }
                else
                {
                    this.set.Remove(this.list[index]);
                    this.set.Add(value);
                    this.list[index] = value;
                }
            }
        }

        /// <inheritdoc/>
        object IList.this[int index] { get => this[index];
            set
            {
              if (value is T item)
              {
                  this[index] = item;
              }
              else
              {
                  throw new ArgumentException("Invalid value type.");
              }
            }
        }

        /// <inheritdoc/>
        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Clear()
        {
            this.set.Clear();
            this.list.Clear();
        }

        /// <inheritdoc/>
        public bool Contains(T item)
        {
            return this.set.Contains(item);
        }

        /// <inheritdoc/>
        public bool Contains(object value)
        {
            if(value is T item)
            {
                return this.Contains(item);
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public void CopyTo(Array array, int index)
        {
            this.list.CopyTo((T[])array, index);
        }

        /// <inheritdoc/>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.list.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <inheritdoc/>
        public int IndexOf(T item)
        {
            return this.list.IndexOf(item);
        }

        /// <inheritdoc/>
        public int IndexOf(object value)
        {
            if (value is T item)
            {
                return this.IndexOf(item);
            }
            else
            {
                return -1;
            }
        }

        /// <inheritdoc/>
        public void Insert(int index, T item)
        {
            this.list.Insert(index, item);
            this.set.Add(item);
        }

        /// <inheritdoc/>
        public void Insert(int index, object value)
        {
            if (value is T item)
            {
                this.Insert(index, item);
            }
            else
            {
                throw new ArgumentException("Invalid value type.");
            }
        }

        /// <inheritdoc/>
        public bool Remove(T item)
        {
            var removedFromSet = this.set.Remove(item);
            var removedFromList = this.list.Remove(item);
            return removedFromSet && removedFromList;
        }

        /// <inheritdoc/>
        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }
    }
}

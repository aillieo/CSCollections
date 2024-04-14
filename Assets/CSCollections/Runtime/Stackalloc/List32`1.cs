// -----------------------------------------------------------------------
// <copyright file="List32`1.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.Collections
{
    using System.Collections;
    using System.Collections.Generic;

    public struct List32<T> : IList<T>
    {
        private List8<T> list0;
        private List8<T> list1;
        private List8<T> list2;
        private List8<T> list3;

        /// <inheritdoc/>
        public int Count => this.list0.Count + this.list1.Count + this.list2.Count + this.list3.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <inheritdoc/>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= this.Count)
                {
                    throw new System.ArgumentOutOfRangeException(nameof(index));
                }

                if (index < 8)
                {
                    return this.list0[index];
                }
                else if (index < 16)
                {
                    return this.list1[index - 8];
                }
                else if (index < 24)
                {
                    return this.list2[index - 16];
                }
                else
                {
                    return this.list3[index - 24];
                }
            }

            set
            {
                if (index < 0 || index >= this.Count)
                {
                    throw new System.ArgumentOutOfRangeException(nameof(index));
                }

                if (index < 8)
                {
                    this.list0[index] = value;
                }
                else if (index < 16)
                {
                    this.list1[index - 8] = value;
                }
                else if (index < 24)
                {
                    this.list2[index - 16] = value;
                }
                else
                {
                    this.list3[index - 24] = value;
                }
            }
        }

        /// <inheritdoc/>
        public void Add(T item)
        {
            if (this.list0.Count < 8)
            {
                this.list0.Add(item);
            }
            else if (this.list1.Count < 8)
            {
                this.list1.Add(item);
            }
            else if (this.list2.Count < 8)
            {
                this.list2.Add(item);
            }
            else if (this.list3.Count < 8)
            {
                this.list3.Add(item);
            }
            else
            {
                throw new System.InvalidOperationException("List32<T> is full.");
            }
        }

        /// <inheritdoc/>
        public void Clear()
        {
            if (this.list0.Count > 0)
            {
                this.list0.Clear();
            }

            if (this.list1.Count > 0)
            {
                this.list1.Clear();
            }

            if (this.list2.Count > 0)
            {
                this.list2.Clear();
            }

            if (this.list3.Count > 0)
            {
                this.list3.Clear();
            }
        }

        /// <inheritdoc/>
        public bool Contains(T item)
        {
            return this.IndexOf(item) != -1;
        }

        /// <inheritdoc/>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new System.ArgumentNullException(nameof(array));
            }

            if (arrayIndex < 0 || arrayIndex + this.Count > array.Length)
            {
                throw new System.ArgumentOutOfRangeException(nameof(arrayIndex));
            }

            var index = arrayIndex;
            for (var i = 0; i < this.list0.Count; i++)
            {
                array[index++] = this.list0[i];
            }

            for (var i = 0; i < this.list1.Count; i++)
            {
                array[index++] = this.list1[i];
            }

            for (var i = 0; i < this.list2.Count; i++)
            {
                array[index++] = this.list2[i];
            }

            for (var i = 0; i < this.list3.Count; i++)
            {
                array[index++] = this.list3[i];
            }
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in this.list0)
            {
                yield return item;
            }

            foreach (var item in this.list1)
            {
                yield return item;
            }

            foreach (var item in this.list2)
            {
                yield return item;
            }

            foreach (var item in this.list3)
            {
                yield return item;
            }
        }

        /// <inheritdoc/>
        public int IndexOf(T item)
        {
            var index = this.list0.IndexOf(item);
            if (index != -1)
            {
                return index;
            }

            index = this.list1.IndexOf(item);
            if (index != -1)
            {
                return index + 8;
            }

            index = this.list2.IndexOf(item);
            if (index != -1)
            {
                return index + 16;
            }

            index = this.list3.IndexOf(item);
            if (index != -1)
            {
                return index + 24;
            }

            return -1;
        }

        /// <inheritdoc/>
        public void Insert(int index, T item)
        {
            if (index < 0 || index > this.Count)
            {
                throw new System.ArgumentOutOfRangeException(nameof(index));
            }

            if (index < 8)
            {
                this.list0.Insert(index, item);
            }
            else if (index < 16)
            {
                this.list1.Insert(index - 8, item);
            }
            else if (index < 24)
            {
                this.list2.Insert(index - 16, item);
            }
            else if (index < 32)
            {
                this.list3.Insert(index - 24, item);
            }
            else
            {
                throw new System.ArgumentOutOfRangeException(nameof(index));
            }
        }

        /// <inheritdoc/>
        public bool Remove(T item)
        {
            var removed = this.list0.Remove(item);
            if (!removed)
            {
                removed = this.list1.Remove(item);
                if (!removed)
                {
                    removed = this.list2.Remove(item);
                    if (!removed)
                    {
                        removed = this.list3.Remove(item);
                    }
                }
            }

            return removed;
        }

        /// <inheritdoc/>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= this.Count)
            {
                throw new System.ArgumentOutOfRangeException(nameof(index));
            }

            if (index < 8)
            {
                this.list0.RemoveAt(index);
            }
            else if (index < 16)
            {
                this.list1.RemoveAt(index - 8);
            }
            else if (index < 24)
            {
                this.list2.RemoveAt(index - 16);
            }
            else
            {
                this.list3.RemoveAt(index - 24);
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}

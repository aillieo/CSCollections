// -----------------------------------------------------------------------
// <copyright file="List8`1.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.Collections
{
    using System.Collections;
    using System.Collections.Generic;

    public struct List8<T> : IList<T>
    {
        private T element0;
        private T element1;
        private T element2;
        private T element3;
        private T element4;
        private T element5;
        private T element6;
        private T element7;

        private int count;

        /// <inheritdoc/>
        public int Count => this.count;

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

                switch (index)
                {
                    case 0:
                        return this.element0;
                    case 1:
                        return this.element1;
                    case 2:
                        return this.element2;
                    case 3:
                        return this.element3;
                    case 4:
                        return this.element4;
                    case 5:
                        return this.element5;
                    case 6:
                        return this.element6;
                    case 7:
                        return this.element7;
                    default:
                        throw new System.ArgumentOutOfRangeException(nameof(index));
                }
            }

            set
            {
                if (index < 0 || index >= this.Count)
                {
                    throw new System.ArgumentOutOfRangeException(nameof(index));
                }

                switch (index)
                {
                    case 0:
                        this.element0 = value;
                        break;
                    case 1:
                        this.element1 = value;
                        break;
                    case 2:
                        this.element2 = value;
                        break;
                    case 3:
                        this.element3 = value;
                        break;
                    case 4:
                        this.element4 = value;
                        break;
                    case 5:
                        this.element5 = value;
                        break;
                    case 6:
                        this.element6 = value;
                        break;
                    case 7:
                        this.element7 = value;
                        break;
                    default:
                        throw new System.ArgumentOutOfRangeException(nameof(index));
                }
            }
        }

        /// <inheritdoc/>
        public void Add(T item)
        {
            if (this.count >= 8)
            {
                throw new System.InvalidOperationException("List8<T> is full.");
            }

            switch (this.count)
            {
                case 0:
                    this.element0 = item;
                    break;
                case 1:
                    this.element1 = item;
                    break;
                case 2:
                    this.element2 = item;
                    break;
                case 3:
                    this.element3 = item;
                    break;
                case 4:
                    this.element4 = item;
                    break;
                case 5:
                    this.element5 = item;
                    break;
                case 6:
                    this.element6 = item;
                    break;
                case 7:
                    this.element7 = item;
                    break;
            }

            this.count++;
        }

        /// <inheritdoc/>
        public void Clear()
        {
            for (var i = 0; i < this.count; ++i)
            {
                this[i] = default;
            }

            this.count = 0;
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

            array[arrayIndex] = this.element0;
            array[arrayIndex + 1] = this.element1;
            array[arrayIndex + 2] = this.element2;
            array[arrayIndex + 3] = this.element3;
            array[arrayIndex + 4] = this.element4;
            array[arrayIndex + 5] = this.element5;
            array[arrayIndex + 6] = this.element6;
            array[arrayIndex + 7] = this.element7;
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            yield return this.element0;
            yield return this.element1;
            yield return this.element2;
            yield return this.element3;
            yield return this.element4;
            yield return this.element5;
            yield return this.element6;
            yield return this.element7;
        }

        /// <inheritdoc/>
        public int IndexOf(T item)
        {
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;

            if (comparer.Equals(this.element0, item))
            {
                return 0;
            }

            if (comparer.Equals(this.element1, item))
            {
                return 1;
            }

            if (comparer.Equals(this.element2, item))
            {
                return 2;
            }

            if (comparer.Equals(this.element3, item))
            {
                return 3;
            }

            if (comparer.Equals(this.element4, item))
            {
                return 4;
            }

            if (comparer.Equals(this.element5, item))
            {
                return 5;
            }

            if (comparer.Equals(this.element6, item))
            {
                return 6;
            }

            if (comparer.Equals(this.element7, item))
            {
                return 7;
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

            if (this.Count == 8)
            {
                throw new System.InvalidOperationException("List8<T> is full.");
            }

            // Shift elements to the right to make space for the new item
            for (var i = this.Count - 1; i >= index; i--)
            {
                this[i + 1] = this[i];
            }

            this[index] = item;
            this.count++;
        }

        /// <inheritdoc/>
        public bool Remove(T item)
        {
            var index = this.IndexOf(item);
            if (index != -1)
            {
                this.RemoveAt(index);
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= this.Count)
            {
                throw new System.ArgumentOutOfRangeException(nameof(index));
            }

            for (var i = index; i < this.Count - 1; i++)
            {
                this[i] = this[i + 1];
            }

            this.count--;
            this[this.count] = default(T);
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}

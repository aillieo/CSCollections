using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AillieoUtils.Collections
{
    public class MutableList<T> : IList<T>
    {
        private readonly List<T> list;
        private readonly HashSet<int> removedIndexes;
        private readonly Dictionary<int, T> modifiedValues;
        private int iterationLock = 0;

        public MutableList()
        {
            this.list = new List<T>();
            this.removedIndexes = new HashSet<int>();
            this.modifiedValues = new Dictionary<int, T>();
        }

        private void Rebuild()
        {
            if (removedIndexes.Count == 0 && modifiedValues.Count == 0)
            {
                return;
            }

            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public int Count { get; }

        bool ICollection<T>.IsReadOnly => false;

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public T this[int index]
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
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

            public bool MoveNext()
            {
                throw new NotImplementedException();
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }

            public T Current => current;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }
    }
}

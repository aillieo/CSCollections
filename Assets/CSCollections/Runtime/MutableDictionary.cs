// -----------------------------------------------------------------------
// <copyright file="MutableDictionary.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class MutableDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> dictionary;
        private readonly HashSet<TKey> removedKeys;
        private readonly Dictionary<TKey, TValue> modifiedValues;
        private int iterationLock;

        public MutableDictionary()
        {
            this.dictionary = new Dictionary<TKey, TValue>();
            this.removedKeys = new HashSet<TKey>();
            this.modifiedValues = new Dictionary<TKey, TValue>();
        }

        public MutableDictionary(int capacity)
        {
            throw new NotImplementedException();
        }

        public MutableDictionary(IEqualityComparer<TKey> comparer)
        {
            throw new NotImplementedException();
        }

        public MutableDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        /// <inheritdoc/>
        public int Count
        {
            get
            {
                if (this.iterationLock > 0)
                {
                    return this.dictionary.Count - this.removedKeys.Count;
                }
                else
                {
                    return this.dictionary.Count;
                }
            }
        }

        /// <inheritdoc/>
        public ICollection<TKey> Keys
        {
            get
            {
                if (this.iterationLock > 0)
                {
                    return this.dictionary.Keys.Where(k => !this.removedKeys.Contains(k)).ToArray();
                }
                else
                {
                    return this.dictionary.Keys;
                }
            }
        }

        /// <inheritdoc/>
        public ICollection<TValue> Values
        {
            get
            {
                if (this.iterationLock > 0)
                {
                    return this.dictionary
                        .Where(p => !this.removedKeys.Contains(p.Key))
                        .Select(p => this.modifiedValues.TryGetValue(p.Key, out TValue v) ? v : p.Value)
                        .ToArray();
                }
                else
                {
                    return this.dictionary.Values;
                }
            }
        }

        /// <inheritdoc/>
        public TValue this[TKey key]
        {
            get
            {
                if (this.iterationLock > 0)
                {
                    if (this.modifiedValues.TryGetValue(key, out TValue value))
                    {
                        return value;
                    }

                    if (this.removedKeys.Contains(key))
                    {
                        throw new KeyNotFoundException();
                    }

                    return this.dictionary[key];
                }
                else
                {
                    return this.dictionary[key];
                }
            }

            set
            {
                if (this.iterationLock > 0)
                {
                    if (this.modifiedValues.ContainsKey(key))
                    {
                        this.modifiedValues[key] = value;
                        return;
                    }

                    if (this.removedKeys.Contains(key))
                    {
                        this.removedKeys.Remove(key);
                        this.modifiedValues[key] = value;
                        return;
                    }

                    if (this.dictionary.ContainsKey(key))
                    {
                        this.modifiedValues[key] = value;
                    }
                    else
                    {
                        throw new Exception("Add key while iterating");
                    }
                }
                else
                {
                    this.dictionary[key] = value;
                }
            }
        }

        /// <inheritdoc/>
        public void Add(TKey key, TValue value)
        {
            if (this.iterationLock > 0)
            {
                if (this.removedKeys.Contains(key))
                {
                    throw new Exception("Add key while iterating");
                }

                this.removedKeys.Remove(key);
                this.modifiedValues[key] = value;
            }
            else
            {
                this.dictionary.Add(key, value);
            }
        }

        /// <inheritdoc/>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this.Add(item.Key, item.Value);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            if (this.iterationLock > 0)
            {
                this.dictionary.Select(p => this.removedKeys.Add(p.Key));
                this.modifiedValues.Clear();
            }
            else
            {
                this.dictionary.Clear();
                this.removedKeys.Clear();
                this.modifiedValues.Clear();
            }
        }

        /// <inheritdoc/>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            if (this.iterationLock > 0)
            {
                if (this.removedKeys.Contains(item.Key))
                {
                    return false;
                }

                if (this.modifiedValues.Contains(item))
                {
                    return true;
                }

                return this.dictionary.Contains(item);
            }
            else
            {
                return this.dictionary.Contains(item);
            }
        }

        /// <inheritdoc/>
        public bool ContainsKey(TKey key)
        {
            if (this.iterationLock > 0)
            {
                return this.dictionary.ContainsKey(key) && !this.removedKeys.Contains(key);
            }
            else
            {
                return this.dictionary.ContainsKey(key);
            }
        }

        /// <inheritdoc/>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (this.iterationLock > 0)
            {
                if (this.removedKeys.Count > 0 || this.modifiedValues.Count > 0)
                {
                    throw new Exception("Copy while iterating");
                }
            }

            ((ICollection<KeyValuePair<TKey, TValue>>)this.dictionary).CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <inheritdoc/>
        public bool Remove(TKey key)
        {
            if (this.iterationLock > 0)
            {
                if (this.dictionary.ContainsKey(key))
                {
                    this.removedKeys.Add(key);
                    this.modifiedValues.Remove(key);
                    return true;
                }

                return false;
            }
            else
            {
                return this.dictionary.Remove(key);
            }
        }

        /// <inheritdoc/>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (this.iterationLock > 0)
            {
                if (this.dictionary.ContainsKey(item.Key))
                {
                    if (((ICollection<KeyValuePair<TKey, TValue>>)this.modifiedValues).Remove(item))
                    {
                        this.removedKeys.Add(item.Key);
                        return true;
                    }

                    if (this.dictionary.Contains(item) && !this.removedKeys.Contains(item.Key))
                    {
                        this.removedKeys.Add(item.Key);
                        return true;
                    }

                    return false;
                }

                return false;
            }
            else
            {
                return ((ICollection<KeyValuePair<TKey, TValue>>)this.dictionary).Remove(item);
            }
        }

        /// <inheritdoc/>
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (this.iterationLock > 0)
            {
                if (this.modifiedValues.TryGetValue(key, out value))
                {
                    return true;
                }

                if (this.removedKeys.Contains(key))
                {
                    value = default;
                    return false;
                }

                return this.dictionary.TryGetValue(key, out value);
            }
            else
            {
                return this.dictionary.TryGetValue(key, out value);
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private void Rebuild()
        {
            if (this.removedKeys.Count == 0 && this.modifiedValues.Count == 0)
            {
                return;
            }

            foreach (var pair in this.modifiedValues)
            {
                this.dictionary[pair.Key] = pair.Value;
            }

            this.modifiedValues.Clear();

            foreach (var key in this.removedKeys)
            {
                this.dictionary.Remove(key);
            }

            this.removedKeys.Clear();
        }

        [Serializable]
        public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private MutableDictionary<TKey, TValue> mutable;
            private Dictionary<TKey, TValue>.Enumerator enumerator;
            private KeyValuePair<TKey, TValue> current;

            internal Enumerator(MutableDictionary<TKey, TValue> mutable)
            {
                this.mutable = mutable;
                this.enumerator = mutable.dictionary.GetEnumerator();
                mutable.iterationLock++;
                this.current = default;
            }

            /// <inheritdoc/>
            public KeyValuePair<TKey, TValue> Current
            {
                get => this.current;
            }

            /// <inheritdoc/>
            object IEnumerator.Current
            {
                get => this.Current;
            }

            /// <inheritdoc/>
            public bool MoveNext()
            {
                if (this.enumerator.MoveNext())
                {
                    this.current = this.enumerator.Current;
                    while (true)
                    {
                        if (this.mutable.removedKeys.Contains(this.current.Key))
                        {
                            this.enumerator.MoveNext();
                            this.current = this.enumerator.Current;
                            continue;
                        }

                        if (this.mutable.modifiedValues.ContainsKey(this.current.Key))
                        {
                            this.current = new KeyValuePair<TKey, TValue>(
                                this.current.Key,
                                this.mutable.modifiedValues[this.current.Key]);
                        }

                        break;
                    }

                    return true;
                }
                else
                {
                    this.current = default;
                    this.mutable.iterationLock--;
                    this.mutable.Rebuild();
                    return false;
                }
            }

            /// <inheritdoc/>
            public void Dispose()
            {
                this.enumerator.Dispose();
            }

            /// <inheritdoc/>
            void IEnumerator.Reset()
            {
                ((IEnumerator)this.enumerator).Reset();
            }
        }
    }
}

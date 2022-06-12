using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AillieoUtils.Collections
{
    public class MutableDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> dictionary;
        private readonly HashSet<TKey> removedKeys;
        private readonly Dictionary<TKey, TValue> modifiedValues;
        private int iterationLock = 0;

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

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        public void Add(TKey key, TValue value)
        {
            if (iterationLock > 0)
            {
                if (removedKeys.Contains(key))
                {
                    throw new Exception("Add key while iterating");
                }

                removedKeys.Remove(key);
                modifiedValues[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            if (iterationLock > 0)
            {
                dictionary.Select(p => removedKeys.Add(p.Key));
                modifiedValues.Clear();
            }
            else
            {
                dictionary.Clear();
                removedKeys.Clear();
                modifiedValues.Clear();
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            if (iterationLock > 0)
            {
                if (removedKeys.Contains(item.Key))
                {
                    return false;
                }

                if (modifiedValues.Contains(item))
                {
                    return true;
                }

                return dictionary.Contains(item);
            }
            else
            {
                return dictionary.Contains(item);
            }
        }

        public bool ContainsKey(TKey key)
        {
            if (iterationLock > 0)
            {
                return dictionary.ContainsKey(key) && !removedKeys.Contains(key);
            }
            else
            {
                return dictionary.ContainsKey(key);
            }
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (iterationLock > 0)
            {
                if (removedKeys.Count > 0 || modifiedValues.Count > 0)
                {
                    throw new Exception("Copy while iterating");
                }
            }

            ((ICollection<KeyValuePair<TKey, TValue>>) dictionary).CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get
            {
                if (iterationLock > 0)
                {
                    return dictionary.Count - removedKeys.Count;
                }
                else
                {
                    return dictionary.Count;
                }
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new Enumerator(this);
        }

        public bool Remove(TKey key)
        {
            if (iterationLock > 0)
            {
                if (dictionary.ContainsKey(key))
                {
                    removedKeys.Add(key);
                    modifiedValues.Remove(key);
                    return true;
                }

                return false;
            }
            else
            {
                return dictionary.Remove(key);
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (iterationLock > 0)
            {
                if (dictionary.ContainsKey(item.Key))
                {
                    if (((ICollection<KeyValuePair<TKey, TValue>>) modifiedValues).Remove(item))
                    {
                        removedKeys.Add(item.Key);
                        return true;
                    }

                    if (dictionary.Contains(item) && !removedKeys.Contains(item.Key))
                    {
                        removedKeys.Add(item.Key);
                        return true;
                    }

                    return false;
                }

                return false;
            }
            else
            {
                return ((ICollection<KeyValuePair<TKey, TValue>>) dictionary).Remove(item);
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (iterationLock > 0)
            {
                if (modifiedValues.TryGetValue(key, out value))
                {
                    return true;
                }

                if (removedKeys.Contains(key))
                {
                    value = default;
                    return false;
                }

                return dictionary.TryGetValue(key, out value);
            }
            else
            {
                return dictionary.TryGetValue(key, out value);
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                if (iterationLock > 0)
                {
                    if (modifiedValues.TryGetValue(key, out TValue value))
                    {
                        return value;
                    }

                    if (removedKeys.Contains(key))
                    {
                        throw new KeyNotFoundException();
                    }

                    return dictionary[key];
                }
                else
                {
                    return dictionary[key];
                }
            }

            set
            {
                if (iterationLock > 0)
                {
                    if (modifiedValues.ContainsKey(key))
                    {
                        modifiedValues[key] = value;
                        return;
                    }

                    if (removedKeys.Contains(key))
                    {
                        removedKeys.Remove(key);
                        modifiedValues[key] = value;
                        return;
                    }

                    if (dictionary.ContainsKey(key))
                    {
                        modifiedValues[key] = value;
                    }
                    else
                    {
                        throw new Exception("Add key while iterating");
                    }
                }
                else
                {
                    dictionary[key] = value;
                }

            }
        }

        public ICollection<TKey> Keys {
            get
            {
                if (iterationLock > 0)
                {
                    return dictionary.Keys.Where(k => !removedKeys.Contains(k)).ToArray();
                }
                else
                {
                    return dictionary.Keys;
                }
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                if (iterationLock > 0)
                {
                    return dictionary
                        .Where(p => !removedKeys.Contains(p.Key))
                        .Select(p => modifiedValues.TryGetValue(p.Key, out TValue v) ? v : p.Value)
                        .ToArray();
                }
                else
                {
                    return dictionary.Values;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void Rebuild()
        {
            if (removedKeys.Count == 0 && modifiedValues.Count == 0)
            {
                return;
            }

            foreach (var pair in modifiedValues)
            {
                dictionary[pair.Key] = pair.Value;
            }
            modifiedValues.Clear();

            foreach (var key in removedKeys)
            {
                dictionary.Remove(key);
            }
            removedKeys.Clear();
        }

        [Serializable]
        public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>//, IDictionaryEnumerator
        {
            private MutableDictionary<TKey, TValue> mutable;
            private Dictionary<TKey, TValue>.Enumerator enumerator;
            private KeyValuePair<TKey, TValue> current;

            internal Enumerator(MutableDictionary<TKey, TValue> mutable)
            {
                this.mutable = mutable;
                this.enumerator = mutable.dictionary.GetEnumerator();
                mutable.iterationLock++;
            }

            public bool MoveNext()
            {
                if (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    while (true)
                    {
                        if (mutable.removedKeys.Contains(current.Key))
                        {
                            enumerator.MoveNext();
                            current = enumerator.Current;
                            continue;
                        }

                        if (mutable.modifiedValues.ContainsKey(current.Key))
                        {
                            current = new KeyValuePair<TKey, TValue>(
                                current.Key,
                                mutable.modifiedValues[current.Key]);
                        }

                        break;;
                    }

                    return true;
                }
                else
                {
                    current = default;
                    mutable.iterationLock--;
                    mutable.Rebuild();
                    return false;
                }
            }

            public KeyValuePair<TKey, TValue> Current
            {
                get => current;
            }

            public void Dispose()
            {
                this.enumerator.Dispose();
            }

            object IEnumerator.Current
            {
                get => Current;
            }

            void IEnumerator.Reset()
            {
                ((IEnumerator)enumerator).Reset();
            }
        }
    }
}

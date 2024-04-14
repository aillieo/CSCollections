// -----------------------------------------------------------------------
// <copyright file="WeightedSet.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class WeightedSet<T> : ICollection<T>
    {
        public bool logWhileTaking = false;

        private readonly Dictionary<T, float> managedItems = new Dictionary<T, float>();

        private Random rand;

        private float cachedWeightSum = -1f;

        public WeightedSet()
            : this(new Random())
        {
        }

        public WeightedSet(Random rand)
        {
            this.rand = rand;
        }

        /// <inheritdoc/>
        public int Count => this.managedItems.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        public void Add(IDictionary<T, float> items)
        {
            foreach (var pair in items)
            {
                if (this.managedItems.TryGetValue(pair.Key, out var oldWeight))
                {
                    var weight = Math.Max(pair.Value + oldWeight, 0);
                    this.managedItems[pair.Key] = weight;
                }
                else
                {
                    var weight = Math.Max(pair.Value, 0);
                    this.managedItems[pair.Key] = weight;
                }
            }

            this.cachedWeightSum = -1f;
        }

        public void Update(IDictionary<T, float> items)
        {
            foreach (var pair in items)
            {
                var weight = Math.Max(pair.Value, 0);
                this.managedItems[pair.Key] = weight;
            }

            this.cachedWeightSum = -1f;
        }

        public void Add(T item, float weight)
        {
            if (this.managedItems.TryGetValue(item, out var oldWeight))
            {
                weight = Math.Max(weight + oldWeight, 0);
            }
            else
            {
                weight = Math.Max(weight, 0);
            }

            this.managedItems[item] = weight;
            this.cachedWeightSum = -1f;
        }

        public void Update(T item, float weight)
        {
            weight = Math.Max(weight, 0);
            this.managedItems[item] = weight;

            this.cachedWeightSum = -1f;
        }

        /// <inheritdoc/>
        public void Clear()
        {
            this.managedItems.Clear();
            this.cachedWeightSum = -1f;
        }

        /// <inheritdoc/>
        public bool Remove(T item)
        {
            if (this.managedItems.Remove(item))
            {
                this.cachedWeightSum = -1f;
                return true;
            }

            return false;
        }

        public IEnumerable<T> RandomTake(int count)
        {
            if (count > this.managedItems.Count)
            {
                throw new Exception("not enough items");
            }

            if (count == this.managedItems.Count)
            {
                return this.managedItems.Select(e => e.Key);
            }

            return this.InternalRandomTake(count);
        }

        public float GetWeight(T item)
        {
            if (!this.managedItems.TryGetValue(item, out var weight))
            {
                return 0;
            }

            if (weight <= 0)
            {
                return 0;
            }

            if (this.cachedWeightSum < 0)
            {
                this.cachedWeightSum = this.managedItems.Sum(item => item.Value);
            }

            return weight / this.cachedWeightSum;
        }

        public T RandomTake()
        {
            if (this.managedItems.Count == 0)
            {
                throw new Exception("not enough items");
            }

            if (this.cachedWeightSum < 0)
            {
                this.cachedWeightSum = this.managedItems.Sum(item => item.Value);
            }

            var weightSum = this.cachedWeightSum;
            var ran = (float)this.rand.NextDouble() * weightSum;
            var sum = 0f;
            foreach (var pair in this.managedItems)
            {
                sum += pair.Value;

                if (this.logWhileTaking)
                {
                    UnityEngine.Debug.Log($"index={0} sum={sum} ran={ran}");
                }

                if (sum > ran)
                {
                    return pair.Key;
                }
            }

            throw new IndexOutOfRangeException();
        }

        /// <inheritdoc/>
        public void Add(T item)
        {
            this.Add(item, 1f);
        }

        /// <inheritdoc/>
        public bool Contains(T item)
        {
            return this.managedItems.ContainsKey(item);
        }

        /// <inheritdoc/>
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

            if (array.Length - arrayIndex < this.managedItems.Count)
            {
                throw new ArgumentException("The destination array is not large enough to copy the elements.");
            }

            int i = arrayIndex;
            foreach (var item in this.managedItems)
            {
                array[i] = item.Key;
                i++;
            }
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            return this.managedItems.Select(e => e.Key).GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private IEnumerable<T> InternalRandomTake(int count)
        {
            if (count == 1)
            {
                yield return this.RandomTake();
                yield break;
            }

            if (this.cachedWeightSum < 0)
            {
                this.cachedWeightSum = this.managedItems.Sum(item => item.Value);
            }

            var weightSum = this.cachedWeightSum;

            // deepcopy
            var deepcopy = new List<KeyValuePair<T, float>>(this.managedItems);

            for (var i = 0; i < count; i++)
            {
                var ran = (float)this.rand.NextDouble() * weightSum;

                var sum = 0f;
                for (var j = 0; j < deepcopy.Count; j++)
                {
                    sum += deepcopy[j].Value;

                    if (this.logWhileTaking)
                    {
                        UnityEngine.Debug.Log($"index={j} sum={sum} ran={ran}");
                    }

                    if (sum > ran)
                    {
                        yield return deepcopy[j].Key;
                        weightSum -= deepcopy[j].Value;

                        // 移到最后一个
                        var last = deepcopy[deepcopy.Count - 1];
                        deepcopy[j] = last;
                        deepcopy.RemoveAt(deepcopy.Count - 1);

                        break;
                    }
                }
            }
        }
    }
}

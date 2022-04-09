using System;
using System.Linq;
using NUnit.Framework;

namespace AillieoUtils.Collections.Tests
{
    [Category(nameof(WeightedSetTest))]
    public class WeightedSetTest
    {
        [Test]
        public static void TestHalf()
        {
            WeightedSet<int> set = new WeightedSet<int>();
            set.Add(0, 0.5f);
            set.Add(1, 0.5f);

            float result = (float)Enumerable.Range(1, 100000).Select(i => set.RandomTake()).Average();
            UnityEngine.Debug.Log($"result = {result}");
            Assert.LessOrEqual(Math.Abs(result - 0.5f), 0.01f);
        }
    }
}

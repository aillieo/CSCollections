using UnityEngine;
using System;
using UnityEngine.Assertions;
using AillieoUtils.Collections;
using System.Linq;

namespace AillieoUtils.Collections.Tests
{
    public class LFUCacheTest
    {
        public static void TestLFU()
        {
            TestShrinkFactor();
            TestGetValue();
            TestReplace();
        }

        private static void TestShrinkFactor()
        {
            foreach (int i in Enumerable.Range(160, 200))
            {
                float f = 0.2f;
                while (f < 0.9f)
                {
                    TestShrinkFactorOnce(i, f);
                    f += 0.06f;
                }
            }
        }

        private static void TestShrinkFactorOnce(int capacity, float factor)
        {
            var cache1 = new LFUCache<int, int>(capacity, factor);
            foreach (var i in Enumerable.Range(0, capacity))
            {
                cache1.Add(i, 0);
            }

            Assert.AreEqual(cache1.Count, capacity);
            cache1.Add(capacity, 0);
            Assert.AreEqual(cache1.Count, (int)(capacity * factor) + 1);
        }

        private static void TestGetValue()
        {
            var cache = new LFUCache<string, int>(4, 0.5f);

            cache.Add("a", 1);
            cache.Add("b", 2);
            cache.Add("c", 3);
            cache.Add("d", 4);

            Assert.AreEqual(cache["a"], 1, "should get 1");
            Assert.AreEqual(cache["b"], 2, "should get 2");
            Assert.AreEqual(cache["c"], 3, "should get 3");
            Assert.AreEqual(cache["d"], 4, "should get 4");
        }

        private static void TestReplace()
        {
            var cache = new LFUCache<string, int>(4, 0.5f);

            cache.Add("a", 1);
            cache.Add("b", 2);
            cache.Add("c", 3);

            int x = default;
            x = cache["a"];
            x = cache["a"];
            x = cache["b"];

            cache.Add("d", 4);

            cache.Add("e", 5);

            Assert.AreEqual(cache["a"], 1, "should get 1");
            Assert.AreEqual(cache["b"], 2, "should get 0");
            Assert.IsFalse(cache.ContainsKey("c"));
            Assert.IsFalse(cache.ContainsKey("d"));
            Assert.AreEqual(cache["e"], 5, "should get 5");
        }
    }
}

using UnityEngine;
using System;
using UnityEngine.Assertions;

namespace AillieoUtils.Collections.Tests
{
    public class LRUCacheTest
    {
        public static void TestLRU()
        {
            TestGetValue();
            TestReplace();
        }

        private static void TestGetValue()
        {
            var cache = new LRUCache<string, int>(3);

            cache["a"] = 1;
            cache["b"] = 2;
            cache["c"] = 3;
            cache["d"] = 4;

            Assert.IsFalse(cache.ContainsKey("a"));
            Assert.AreEqual(cache["b"], 2, "should get 2");
            Assert.AreEqual(cache["c"], 3, "should get 3");
            Assert.AreEqual(cache["d"], 4, "should get 4");
        }

        private static void TestReplace()
        {
            var cache = new LRUCache<string, int>(3);

            cache["a"] = 1;
            cache["b"] = 2;
            cache["c"] = 3;

            int x = default;
            x = cache["a"];
            x = cache["a"];
            x = cache["b"];

            cache["d"] = 4;
            cache["e"] = 5;

            Assert.IsFalse(cache.ContainsKey("a"));
            Assert.AreEqual(cache["b"], 2, "should get 2");
            Assert.IsFalse(cache.ContainsKey("c"));
            Assert.AreEqual(cache["d"], 4, "should get 4");
            Assert.AreEqual(cache["e"], 5, "should get 5");
        }
    }
}

using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace AillieoUtils.Collections.Tests
{
    [Category(nameof(LinkedDictionaryTest))]
    public static class LinkedDictionaryTest
    {
        [Test]
        public static void TestAdd()
        {
            LinkedDictionary<int, int> linked = new LinkedDictionary<int, int>();
            linked.Add(1, 1);
            Assert.AreEqual(linked.Count, 1);
            Assert.AreEqual(linked[1], 1);

            linked.Add(2, 2);
            Assert.AreEqual(linked.Count, 2);
            Assert.AreEqual(linked[1], 1);
            Assert.AreEqual(linked[2], 2);
        }

        [Test]
        public static void TestUpdate()
        {
            LinkedDictionary<string, string> linked = new LinkedDictionary<string, string>();
            linked.Add("a", "a");
            Assert.AreEqual(linked.Count, 1);
            Assert.AreEqual(linked["a"], "a");

            linked["a"] = "b";
            Assert.AreEqual(linked.Count, 1);
            Assert.AreEqual(linked["a"], "b");
        }

        [Test]
        public static void TestContains()
        {
            LinkedDictionary<int, string> linked = new LinkedDictionary<int, string>();
            linked.Add(1, "a");
            Assert.AreEqual(linked.Count, 1);
            Assert.AreEqual(linked[1], "a");
            Assert.IsTrue(linked.ContainsKey(1));
            Assert.IsFalse(linked.ContainsKey(0));
            Assert.IsFalse(linked.ContainsKey(2));
        }

        [Test]
        public static void TestRemove()
        {
            LinkedDictionary<string, int> linked = new LinkedDictionary<string, int>();
            linked.Add("a", 1);
            linked.Add("b", 2);
            Assert.AreEqual(linked.Count, 2);
            Assert.AreEqual(linked["a"], 1);
            Assert.AreEqual(linked["b"], 2);
            Assert.IsTrue(linked.ContainsKey("a"));
            Assert.IsTrue(linked.ContainsKey("b"));

            Assert.IsTrue(linked.Remove("a"));
            Assert.IsFalse(linked.Remove("c"));

            Assert.AreEqual(linked.Count, 1);
            Assert.IsFalse(linked.ContainsKey("a"));
            Assert.IsTrue(linked.ContainsKey("b"));
            Assert.AreEqual(linked["b"], 2);

            Assert.IsFalse(linked.Remove("a"));
            Assert.IsTrue(linked.Remove("b"));
            Assert.AreEqual(linked.Count, 0);
            Assert.IsFalse(linked.ContainsKey("b"));
        }

        [Test]
        public static void TestFirstKey()
        {
            LinkedDictionary<int, char> linked = new LinkedDictionary<int, char>();
            linked.Add(1, 'a');
            Assert.AreEqual(linked.FirstKey, 1);
            linked.Add(2, 'b');
            Assert.AreEqual(linked.FirstKey, 1);
            linked.Remove(1);
            Assert.AreEqual(linked.FirstKey, 2);
            linked.AddFirst(3, 'c');
            Assert.AreEqual(linked.FirstKey, 3);
            linked.Add(4, 'd');
            Assert.AreEqual(linked.FirstKey, 3);
            linked.Remove(3);
            Assert.AreEqual(linked.FirstKey, 2);
            linked.Remove(2);
            Assert.AreEqual(linked.FirstKey, 4);
            linked.Remove(4);
        }

        [Test]
        public static void TestLastKey()
        {
            LinkedDictionary<int, bool> linked = new LinkedDictionary<int, bool>();
            linked.Add(1, false);
            Assert.AreEqual(linked.LastKey, 1);
            linked.Add(2, true);
            Assert.AreEqual(linked.LastKey, 2);
            linked.Remove(1);
            Assert.AreEqual(linked.LastKey, 2);
            linked.AddFirst(3, false);
            Assert.AreEqual(linked.LastKey, 2);
            linked.Add(4, true);
            Assert.AreEqual(linked.LastKey, 4);
            linked.Remove(3);
            Assert.AreEqual(linked.LastKey, 4);
            linked.Remove(4);
            Assert.AreEqual(linked.LastKey, 2);
            linked.Remove(2);
        }
    }
}

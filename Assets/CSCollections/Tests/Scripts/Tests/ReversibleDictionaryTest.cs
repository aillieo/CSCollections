using System;
using System.Linq;
using NUnit.Framework;

namespace AillieoUtils.Collections.Tests
{
    [Category(nameof(ReversibleDictionaryTest))]
    public class ReversibleDictionaryTest
    {
        [Test]
        public static void TestAdd()
        {
            ReversibleDictionary<int, int> dictionary = new ReversibleDictionary<int, int>();
            dictionary.Add(1, 1);
            Assert.AreEqual(dictionary.Count, 1);
            Assert.AreEqual(dictionary.FirstKeyForValue(1), 1);
            Assert.AreEqual(dictionary.FirstKeyForValueOrDefault(4, 0), 0);
        }
    }
}

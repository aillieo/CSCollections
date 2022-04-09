using System;
using System.Linq;
using NUnit.Framework;

namespace AillieoUtils.Collections.Tests
{
    [Category(nameof(MutableDictionaryTest))]
    public class MutableDictionaryTest
    {
        [Test]
        public static void TestModifySimple()
        {
            MutableDictionary<int, int> dictionary = new MutableDictionary<int, int>();
            dictionary[0] = 0;
            dictionary[1] = 1;
            dictionary[2] = 2;
            dictionary[3] = 3;
            dictionary[4] = 4;

            foreach (var p in dictionary)
            {
                int value = p.Value;
                dictionary[p.Key] = value + 1;
                Assert.AreEqual(value + 1, dictionary[p.Key]);
            }
        }
    }
}

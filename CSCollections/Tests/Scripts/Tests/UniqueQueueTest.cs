using System;
using System.Linq;
using NUnit.Framework;

namespace AillieoUtils.Collections.Tests
{
    [Category(nameof(UniqueQueueTest))]
    public class UniqueQueueTest
    {
        [Test]
        public static void TestSimple()
        {
            UniqueQueue<int> queue = new UniqueQueue<int>();
            Assert.IsTrue(queue.Enqueue(0));
            Assert.IsFalse(queue.Enqueue(0));
            Assert.IsTrue(queue.Contains(0));
            Assert.AreEqual(queue.Peek(), 0);
            Assert.AreEqual(queue.Dequeue(), 0);
        }
    }
}

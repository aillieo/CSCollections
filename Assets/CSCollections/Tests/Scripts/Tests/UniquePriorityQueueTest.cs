using System;
using System.Linq;
using NUnit.Framework;

namespace AillieoUtils.Collections.Tests
{
    [Category(nameof(UniquePriorityQueueTest))]
    public class UniquePriorityQueueTest
    {
        [Test]
        public static void Test()
        {
            UniquePriorityQueue<int> queue = new UniquePriorityQueue<int>();
            Assert.IsTrue(queue.Enqueue(2));
            Assert.IsTrue(queue.Enqueue(3));
            Assert.IsTrue(queue.Enqueue(0));
            Assert.IsTrue(queue.Enqueue(1));

            Assert.AreEqual(queue.Count, 4);
            Assert.AreEqual(queue.Dequeue(), 3);
            Assert.AreEqual(queue.Dequeue(), 2);
            Assert.AreEqual(queue.Dequeue(), 1);
            Assert.AreEqual(queue.Dequeue(), 0);
            Assert.AreEqual(queue.Count, 0);
        }
    }
}

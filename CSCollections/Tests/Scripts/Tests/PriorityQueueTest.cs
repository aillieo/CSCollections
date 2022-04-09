using System;
using System.Linq;
using NUnit.Framework;

namespace AillieoUtils.Collections.Tests
{
    [Category(nameof(PriorityQueueTest))]
    public class PriorityQueueTest
    {
        [Test]
        public static void Test()
        {
            PriorityQueue<int> queue = new PriorityQueue<int>();
            queue.Enqueue(2);
            queue.Enqueue(3);
            queue.Enqueue(0);
            queue.Enqueue(1);
            Assert.AreEqual(queue.Count, 4);
            Assert.AreEqual(queue.Dequeue(), 3);
            Assert.AreEqual(queue.Dequeue(), 2);
            Assert.AreEqual(queue.Dequeue(), 1);
            Assert.AreEqual(queue.Dequeue(), 0);
            Assert.AreEqual(queue.Count, 0);
        }
    }
}

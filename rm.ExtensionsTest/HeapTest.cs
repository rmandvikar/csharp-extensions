using System;
using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest
{
    [TestFixture]
    public class HeapTest
    {
        class Person
        {
            public int Age { get; set; }
        }
        // tests Siftdown() internally.
        [Test]
        public void Append01()
        {
            var person1 = new Person { Age = 1 };
            var person2 = new Person { Age = 1 };
            var minheap = new MinHeap<Person, int>(2, x => x.Age);
            minheap.Append(person1);
            minheap.Append(person2);
            foreach (var x in minheap) { break; }
            Assert.AreEqual(person1, minheap.Peek());
            Assert.AreNotEqual(person2, minheap.Peek());
        }
        // tests SiftUp() internally.
        [Test]
        public void Insert01()
        {
            var person1 = new Person { Age = 1 };
            var person2 = new Person { Age = 1 };
            var maxheap = new MaxHeap<Person, int>(2, x => x.Age);
            maxheap.Insert(person1);
            maxheap.Insert(person2);
            Assert.AreEqual(person1, maxheap.Peek());
            Assert.AreNotEqual(person2, maxheap.Peek());
        }
        [Test]
        public void Insert02()
        {
            var minheap = new MinHeap<int>(3);
            Assert.AreEqual(0, minheap.Count());
            minheap.Insert(0);
            Assert.AreEqual(1, minheap.Count());
            minheap.Insert(0);
            Assert.AreEqual(2, minheap.Count());
            minheap.Insert(2);
            Assert.AreEqual(3, minheap.Count());

            var maxheap = new MaxHeap<int>(3);
            Assert.AreEqual(0, maxheap.Count());
            maxheap.Insert(0);
            Assert.AreEqual(1, maxheap.Count());
            maxheap.Insert(0);
            Assert.AreEqual(2, maxheap.Count());
            maxheap.Insert(1);
            Assert.AreEqual(3, maxheap.Count());
        }
        [Test]
        public void Delete01()
        {
            var minheap = new MinHeap<int>(2);
            Assert.AreEqual(0, minheap.Count());
            minheap.Insert(1);
            minheap.Insert(0);
            Assert.Throws<InvalidOperationException>(() => { minheap.Insert(-1); });
            Assert.AreEqual(0, minheap.Delete());
            Assert.AreEqual(1, minheap.Count());

            var maxheap = new MaxHeap<int>(2);
            Assert.AreEqual(0, maxheap.Count());
            maxheap.Insert(0);
            maxheap.Insert(1);
            Assert.Throws<InvalidOperationException>(() => { maxheap.Insert(-1); });
            Assert.AreEqual(1, maxheap.Delete());
            Assert.AreEqual(1, maxheap.Count());
        }
        [Test]
        public void Displace01()
        {
            var minheap = new MinHeap<int>(2);
            Assert.AreEqual(0, minheap.Count());
            minheap.Insert(5);
            minheap.Insert(4);
            Assert.AreEqual(4, minheap.Displace(6));
            Assert.AreEqual(2, minheap.Count());
            Assert.AreEqual(5, minheap.Peek());

            var maxheap = new MaxHeap<int>(2);
            Assert.AreEqual(0, maxheap.Count());
            maxheap.Insert(3);
            maxheap.Insert(4);
            Assert.AreEqual(4, maxheap.Displace(2));
            Assert.AreEqual(2, maxheap.Count());
            Assert.AreEqual(3, maxheap.Peek());
        }
        [Test]
        public void Peek01()
        {
            var minheap = new MinHeap<int>(2);
            Assert.Throws<InvalidOperationException>(() => { minheap.Peek(); });
            minheap.Insert(0);
            Assert.AreEqual(0, minheap.Peek());

            var maxheap = new MaxHeap<int>(2);
            Assert.Throws<InvalidOperationException>(() => { maxheap.Peek(); });
            maxheap.Insert(0);
            Assert.AreEqual(0, maxheap.Peek());
        }
    }
}

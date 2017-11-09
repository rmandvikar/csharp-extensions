using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using rm.Extensions;
using rm.Extensions.Deque;

namespace rm.ExtensionsTest
{
	[TestFixture]
	public class DequeTest
	{
		[Test]
		public void Enqueue01()
		{
			var dq = new Deque<int>();
			dq.Enqueue(1);
			dq.Enqueue(2);
			Assert.AreEqual(2, dq.Count());
		}

		[Test]
		public void Dequeue01()
		{
			var dq = new Deque<int>();
			dq.Enqueue(1);
			dq.Enqueue(2);
			Assert.AreEqual(2, dq.Count());
			Assert.AreEqual(1, dq.Dequeue());
			Assert.AreEqual(1, dq.Count());
			Assert.AreEqual(2, dq.Dequeue());
			Assert.AreEqual(0, dq.Count());
		}

		[Test]
		public void PeekPeekTail01()
		{
			var dq = new Deque<int>();
			dq.Enqueue(1);
			dq.Enqueue(2);
			dq.Enqueue(3);
			Assert.AreEqual(1, dq.Peek());
			Assert.AreEqual(3, dq.PeekTail());
		}

		[Test]
		public void IsEmpty01()
		{
			var dq = new Deque<int>();
			Assert.IsTrue(dq.IsEmpty());
			dq.Enqueue(1);
			Assert.IsFalse(dq.IsEmpty());
			dq.Dequeue();
			Assert.IsTrue(dq.IsEmpty());
		}

		[Test]
		public void Clear01()
		{
			var dq = new Deque<int>();
			dq.Enqueue(1);
			dq.Enqueue(2);
			Assert.IsFalse(dq.IsEmpty());
			dq.Clear();
			Assert.IsTrue(dq.IsEmpty());
			dq.Enqueue(1);
			dq.Enqueue(2);
			Assert.IsFalse(dq.IsEmpty());
		}

		[Test]
		public void Delete_Head_01()
		{
			var dq = new Deque<int>();
			Assert.AreEqual(0, dq.Count());
			var n1 = dq.Enqueue(1);
			var n2 = dq.Enqueue(2);
			var n3 = dq.Enqueue(3);
			Assert.AreEqual(1, dq.Peek());
			dq.Delete(n1);
			Assert.AreEqual(2, dq.Peek());
		}

		[Test]
		public void Delete_Tail_02()
		{
			var dq = new Deque<int>();
			Assert.AreEqual(0, dq.Count());
			var n1 = dq.Enqueue(1);
			var n2 = dq.Enqueue(2);
			var n3 = dq.Enqueue(3);
			Assert.AreEqual(3, dq.PeekTail());
			dq.Delete(n3);
			Assert.AreEqual(2, dq.PeekTail());
		}

		[Test]
		public void Delete_Between_03()
		{
			var dq = new Deque<int>();
			Assert.AreEqual(0, dq.Count());
			var n1 = dq.Enqueue(1);
			var n2 = dq.Enqueue(2);
			var n3 = dq.Enqueue(3);
			Assert.AreEqual(1, dq.Peek());
			Assert.AreEqual(3, dq.PeekTail());
			dq.Delete(n2);
			Assert.AreEqual(1, dq.Peek());
			Assert.AreEqual(3, dq.PeekTail());
		}

		[Test]
		public void Delete_HeadTail_04()
		{
			var dq = new Deque<int>();
			Assert.AreEqual(0, dq.Count());
			var n1 = dq.Enqueue(1);
			Assert.AreEqual(1, dq.Peek());
			Assert.AreEqual(1, dq.PeekTail());
			dq.Delete(n1);
			Assert.IsTrue(dq.IsEmpty());
		}

		[Test]
		public void Delete_ReEnqueue_05()
		{
			var dq = new Deque<int>();
			Assert.AreEqual(0, dq.Count());
			var n = dq.Enqueue(1);
			Assert.AreEqual(1, dq.Count());
			dq.Delete(n);
			Assert.AreEqual(0, dq.Count());
			n = dq.Enqueue(1);
			Assert.AreEqual(1, dq.Count());
		}

		[Test]
		public void Delete_Owner_06()
		{
			var dqA = new Deque<int>();
			var anode = dqA.Enqueue(1);
			var dqB = new Deque<int>();
			var bnode = dqB.Enqueue(1);
			Assert.Throws<InvalidOperationException>(() => dqB.Delete(anode));
			Assert.Throws<InvalidOperationException>(() => dqA.Delete(bnode));
		}

		[Test(Description = "Deque<T> v/s Queue<T> find speed test.")]
		[Category("slow")]
		public void Find01()
		{
			var count = 1000000;
			var sw = new Stopwatch();

			var dq = new Deque<int>();
			Node<int> node = null;
			for (int i = 0; i < count; i++)
			{ node = dq.Enqueue(i); }
			sw.Start();
			dq.Delete(node);
			sw.Stop();
			var dqTime = sw.ElapsedMilliseconds;
			Console.WriteLine(dqTime);

			sw.Reset();
			var q = new Queue<int>();
			for (int i = 0; i < count; i++)
			{ q.Enqueue(i); }
			sw.Start();
			// Queue<T>.Remove(x) and Where(x) are O(n).
			q.Where(x => x == count - 1).ToList();
			sw.Stop();
			var qTime = sw.ElapsedMilliseconds;
			Console.WriteLine(qTime);

			Assert.Less(dqTime, qTime);
			Assert.Less(dqTime, 3);
		}

		[Test(Description = "Deque<T> v/s Queue<T> enumerate speed test.")]
		[Category("slow")]
		public void Enumerate01()
		{
			var count = 1000000;
			var sw = new Stopwatch();

			var dq = new Deque<int>();
			for (int i = 0; i < count; i++)
			{ dq.Enqueue(i); }
			sw.Start();
			foreach (var item in dq) { }
			sw.Stop();
			var dqTime = sw.ElapsedMilliseconds;
			Console.WriteLine(dqTime);

			sw.Reset();
			var q = new Queue<int>();
			for (int i = 0; i < count; i++)
			{ q.Enqueue(i); }
			sw.Start();
			foreach (var item in q) { }
			sw.Stop();
			var qTime = sw.ElapsedMilliseconds;
			Console.WriteLine(qTime);

			Assert.Less(dqTime, qTime * 4);
			Assert.Less(dqTime, 50);
		}
	}
}

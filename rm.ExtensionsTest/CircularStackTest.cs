using System;
using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest
{
	[TestFixture]
	public class CircularStackTest
	{
		[Test]
		public void Push01()
		{
			var capacity = 4;
			var cq = new CircularStack<int>(capacity);
			for (int i = 0; i < 6; i++)
			{ cq.Push(i); }
			Assert.AreEqual(capacity, cq.Count());
		}

		[Test]
		public void Pop01()
		{
			var capacity = 4;
			var cq = new CircularStack<int>(capacity);
			for (int i = 0; i < capacity; i++)
			{ cq.Push(i); }
			for (int i = 0; i < capacity; i++)
			{
				Assert.AreEqual(capacity - (i + 1), cq.Pop());
				Assert.AreEqual(capacity - (i + 1), cq.Count());
			}
			Assert.Throws<InvalidOperationException>(() => cq.Pop());
		}

		[Test]
		public void PushPop01()
		{
			var capacity = 4;
			var cq = new CircularStack<int>(capacity);
			cq.Push(0);
			cq.Push(1);
			cq.Push(2);
			cq.Push(3);
			cq.Pop();
			cq.Pop();
			Assert.AreEqual(1, cq.Peek());
			Assert.AreEqual(0, cq.PeekBottom());
			cq.Push(4);
			cq.Push(5);
			Assert.AreEqual(5, cq.Peek());
			Assert.AreEqual(0, cq.PeekBottom());
			cq.Push(6);
			cq.Push(7);
			Assert.AreEqual(7, cq.Peek());
			Assert.AreEqual(4, cq.PeekBottom());
		}

		[Test]
		public void PeekPeekBottom01()
		{
			var capacity = 4;
			var cq = new CircularStack<int>(capacity);
			for (int i = 0; i < 6; i++)
			{ cq.Push(i); }
			Assert.AreEqual(capacity, cq.Count());
			Assert.AreEqual(5, cq.Peek());
			Assert.AreEqual(2, cq.PeekBottom());
		}

		[Test]
		public void IsEmpty01()
		{
			var cq = new CircularStack<int>(1);
			Assert.IsTrue(cq.IsEmpty());
			cq.Push(1);
			Assert.IsFalse(cq.IsEmpty());
			cq.Pop();
			Assert.IsTrue(cq.IsEmpty());
		}

		[Test]
		public void Count01()
		{
			var cq = new CircularStack<int>(4);
			Assert.AreEqual(0, cq.Count());
			cq.Push(1);
			cq.Push(1);
			Assert.AreEqual(2, cq.Count());
			cq.Pop();
			Assert.AreEqual(1, cq.Count());
			cq.Pop();
			Assert.AreEqual(0, cq.Count());
		}

		[Test]
		public void Capacity01()
		{
			var capacity = 4;
			var cq = new CircularStack<int>(capacity);
			Assert.AreEqual(capacity, cq.Capacity());
			for (int i = 0; i < 6; i++)
			{ cq.Push(i); }
			Assert.AreEqual(capacity, cq.Capacity());
		}

		[Test]
		public void Clear01()
		{
			var capacity = 4;
			var cq = new CircularStack<int>(capacity);
			Assert.AreEqual(capacity, cq.Capacity());
			for (int i = 0; i < 6; i++)
			{ cq.Push(i); }
			Assert.AreEqual(capacity, cq.Count());
			cq.Clear();
			Assert.AreEqual(0, cq.Count());
			for (int i = 0; i < 6; i++)
			{ cq.Push(i); }
			Assert.AreEqual(capacity, cq.Count());
		}

		[Test]
		public void ArgumentOutOfRange01()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => new CircularStack<int>(0));
			Assert.Throws<ArgumentOutOfRangeException>(() => new CircularStack<int>(-1));
		}
	}
}

using System;
using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest;

[TestFixture]
public class CircularQueueTest
{
	[Test]
	public void Enqueue01()
	{
		var capacity = 4;
		var cq = new CircularQueue<int>(capacity);
		for (int i = 0; i < 6; i++)
		{ cq.Enqueue(i); }
		Assert.AreEqual(capacity, cq.Count());
	}

	[Test]
	public void Dequeue01()
	{
		var capacity = 4;
		var cq = new CircularQueue<int>(capacity);
		for (int i = 0; i < capacity; i++)
		{ cq.Enqueue(i); }
		for (int i = 0; i < capacity; i++)
		{
			Assert.AreEqual(i, cq.Dequeue());
			Assert.AreEqual(capacity - (i + 1), cq.Count());
		}
		Assert.Throws<InvalidOperationException>(() => cq.Dequeue());
	}

	[Test]
	public void EnqueueDequeue01()
	{
		var capacity = 4;
		var cq = new CircularQueue<int>(capacity);
		cq.Enqueue(0);
		cq.Enqueue(1);
		cq.Enqueue(2);
		cq.Enqueue(3);
		cq.Dequeue();
		cq.Dequeue();
		Assert.AreEqual(2, cq.Peek());
		Assert.AreEqual(3, cq.PeekTail());
		cq.Enqueue(4);
		cq.Enqueue(5);
		Assert.AreEqual(2, cq.Peek());
		Assert.AreEqual(5, cq.PeekTail());
		cq.Enqueue(6);
		cq.Enqueue(7);
		Assert.AreEqual(4, cq.Peek());
		Assert.AreEqual(7, cq.PeekTail());
	}

	[Test]
	public void PeekPeekTail01()
	{
		var capacity = 4;
		var cq = new CircularQueue<int>(capacity);
		for (int i = 0; i < 6; i++)
		{ cq.Enqueue(i); }
		Assert.AreEqual(capacity, cq.Count());
		Assert.AreEqual(2, cq.Peek());
		Assert.AreEqual(5, cq.PeekTail());
	}

	[Test]
	public void IsEmpty01()
	{
		var cq = new CircularQueue<int>(1);
		Assert.IsTrue(cq.IsEmpty());
		cq.Enqueue(1);
		Assert.IsFalse(cq.IsEmpty());
		cq.Dequeue();
		Assert.IsTrue(cq.IsEmpty());
	}

	[Test]
	public void Count01()
	{
		var cq = new CircularQueue<int>(4);
		Assert.AreEqual(0, cq.Count());
		cq.Enqueue(1);
		cq.Enqueue(1);
		Assert.AreEqual(2, cq.Count());
		cq.Dequeue();
		Assert.AreEqual(1, cq.Count());
		cq.Dequeue();
		Assert.AreEqual(0, cq.Count());
	}

	[Test]
	public void Capacity01()
	{
		var capacity = 4;
		var cq = new CircularQueue<int>(capacity);
		Assert.AreEqual(capacity, cq.Capacity());
		for (int i = 0; i < 6; i++)
		{ cq.Enqueue(i); }
		Assert.AreEqual(capacity, cq.Capacity());
	}

	[Test]
	public void Clear01()
	{
		var capacity = 4;
		var cq = new CircularQueue<int>(capacity);
		Assert.AreEqual(capacity, cq.Capacity());
		for (int i = 0; i < 6; i++)
		{ cq.Enqueue(i); }
		Assert.AreEqual(capacity, cq.Count());
		cq.Clear();
		Assert.AreEqual(0, cq.Count());
		for (int i = 0; i < 6; i++)
		{ cq.Enqueue(i); }
		Assert.AreEqual(capacity, cq.Count());
	}

	[Test]
	public void ArgumentOutOfRange01()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() => new CircularQueue<int>(0));
		Assert.Throws<ArgumentOutOfRangeException>(() => new CircularQueue<int>(-1));
	}
}

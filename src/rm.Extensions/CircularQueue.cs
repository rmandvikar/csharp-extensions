using System;
using System.Collections;
using System.Collections.Generic;

namespace rm.Extensions;

/// <summary>
/// Defines circular queue methods.
/// </summary>
public interface ICircularQueue<T>
{
	/// <summary>
	/// Enqueues <paramref name="x"/> into queue.
	/// </summary>
	void Enqueue(T x);

	/// <summary>
	/// Dequeues head from queue.
	/// </summary>
	T Dequeue();

	/// <summary>
	/// Peeks head of queue.
	/// </summary>
	T Peek();

	/// <summary>
	/// Peeks tail of queue.
	/// </summary>
	T PeekTail();

	/// <summary>
	/// Returns true if queue is empty.
	/// </summary>
	bool IsEmpty();

	/// <summary>
	/// Returns count of queue.
	/// </summary>
	int Count();

	/// <summary>
	/// Returns capacity of queue.
	/// </summary>
	int Capacity();

	/// <summary>
	/// Clears queue.
	/// </summary>
	void Clear();
}

/// <summary>
/// Circular queue.
/// </summary>
/// <remarks>Uses array as backing store.</remarks>
public class CircularQueue<T> : ICircularQueue<T>, IEnumerable<T>
{
	#region members

	private readonly T[] store;
	private readonly int capacity;
	private int count;
	private int head;
	private int tail;

	#endregion

	#region ctors

	public CircularQueue(int capacity = 8)
	{
		capacity.ThrowIfArgumentOutOfRange(minRange: 1, exMessage: nameof(capacity));
		store = new T[capacity];
		this.capacity = capacity;
		head = tail = count = 0;
	}

	#endregion

	#region ICircularQueue<T> methods

	/// <summary>
	/// Enqueues <paramref name="x"/> into queue.
	/// </summary>
	public void Enqueue(T x)
	{
		store[tail] = x;
		tail = WrapIndex(tail + 1);
		if (count == capacity)
		{
			head = tail;
		}
		if (count < capacity)
		{
			count++;
		}
	}

	/// <summary>
	/// Dequeues head from queue.
	/// </summary>
	public T Dequeue()
	{
		if (IsEmpty())
		{
			throw new InvalidOperationException("Queue is empty.");
		}
		var item = store[head];
		head = WrapIndex(head + 1);
		count--;
		return item;
	}

	/// <summary>
	/// Peeks head of queue.
	/// </summary>
	public T Peek()
	{
		if (IsEmpty())
		{
			throw new InvalidOperationException("Queue is empty.");
		}
		return store[head];
	}

	/// <summary>
	/// Peeks tail of queue.
	/// </summary>
	public T PeekTail()
	{
		if (IsEmpty())
		{
			throw new InvalidOperationException("Queue is empty.");
		}
		return store[WrapIndex(tail - 1)];
	}

	/// <summary>
	/// Returns true if queue is empty.
	/// </summary>
	public bool IsEmpty()
	{
		return count == 0;
	}

	/// <summary>
	/// Returns count of queue.
	/// </summary>
	public int Count()
	{
		return count;
	}

	/// <summary>
	/// Returns capacity of queue.
	/// </summary>
	public int Capacity()
	{
		return capacity;
	}

	/// <summary>
	/// Clears queue.
	/// </summary>
	public void Clear()
	{
		head = tail = count = 0;
	}

	#endregion

	private int WrapIndex(int index)
	{
		var wIndex = index % capacity;
		if (wIndex < 0)
		{
			wIndex += capacity;
		}
		return wIndex;
	}

	#region IEnumerable<T> methods

	public IEnumerator<T> GetEnumerator()
	{
		var index = head;
		for (int c = 0; c < count; c++)
		{
			yield return store[index];
			index = WrapIndex(index + 1);
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	#endregion
}

using System;
using System.Collections;
using System.Collections.Generic;
using rm.Extensions.Deque;
using Ex = rm.Extensions.ExceptionHelper;

namespace rm.Extensions;

/// <summary>
/// Defines hashqueue methods.
/// </summary>
public interface IHashQueue<T> : IEnumerable<T>
{
	/// <summary>
	/// Enqueues <paramref name="x"/> into hashqueue.
	/// </summary>
	void Enqueue(T x);

	/// <summary>
	/// Dequeues head from hashqueue.
	/// </summary>
	T Dequeue();

	/// <summary>
	/// Deletes given <paramref name="x"/> from hashqueue.
	/// Returns true if successful.
	/// </summary>
	bool Delete(T x);

	/// <summary>
	/// Peeks head of hashqueue.
	/// </summary>
	T Peek();

	/// <summary>
	/// Peeks tail of hashqueue.
	/// </summary>
	T PeekTail();

	/// <summary>
	/// Returns true if <paramref name="x"/> is in hashqueue.
	/// </summary>
	bool Has(T x);

	/// <summary>
	/// Returns true if hashqueue is empty.
	/// </summary>
	bool IsEmpty();

	/// <summary>
	/// Returns count of hashqueue.
	/// </summary>
	int Count();

	/// <summary>
	/// Returns long count of hashqueue.
	/// </summary>
	long LongCount();

	/// <summary>
	/// Clears hashqueue.
	/// </summary>
	void Clear();
}

/// <summary>
/// Hashqueue.
/// </summary>
/// <remarks>
/// Uses deque and map as backing store.
///
/// All methods are O(1) time.
/// </remarks>
public class HashQueue<T> : IHashQueue<T>
{
	#region members

	IDeque<T> dq;
	IDictionary<T, Deque<Node<T>>> map;

	#endregion

	#region ctors

	public HashQueue()
	{
		dq = new Deque<T>();
		map = new Dictionary<T, Deque<Node<T>>>();
	}

	#endregion

	#region IHashQueue<T> methods

	/// <summary>
	/// Enqueues <paramref name="x"/> into hashqueue.
	/// </summary>
	public void Enqueue(T x)
	{
		x.ThrowIfArgumentNull(nameof(x));
		if (!map.TryGetValue(x, out Deque<Node<T>> nodeq))
		{
			nodeq = new Deque<Node<T>>();
			map[x] = nodeq;
		}
		nodeq.Enqueue(dq.Enqueue(x));
	}

	/// <summary>
	/// Dequeues head from hashqueue.
	/// </summary>
	public T Dequeue()
	{
		Ex.ThrowIfEmpty(IsEmpty(), "HashQueue is empty.");
		var x = dq.Dequeue();
		DeleteFromMap(x);
		return x;
	}

	/// <summary>
	/// Deletes given <paramref name="x"/> from hashqueue.
	/// Returns true if successful.
	/// </summary>
	public bool Delete(T x)
	{
		Ex.ThrowIfEmpty(IsEmpty(), "HashQueue is empty.");
		x.ThrowIfArgumentNull(nameof(x));
		if (!Has(x))
		{
			return false;
		}
		dq.Delete(DeleteFromMap(x));
		return true;
	}

	/// <summary>
	/// Deletes <paramref name="x"/> from <see cref="map"/> and
	/// returns the node.
	/// </summary>
	private Node<T> DeleteFromMap(T x)
	{
		var nodeq = map[x];
		foreach (var node in nodeq.Nodes())
		{
			var xnode = node.Value;
			if (EqualityComparer<T>.Default.Equals(xnode.Value, x))
			{
				nodeq.Delete(node);
				if (nodeq.IsEmpty())
				{
					map.Remove(x);
				}
				return node.Value;
			}
		}
		throw new InvalidOperationException(
			$"Node not found for item {x} in HashQueue.");
	}

	/// <summary>
	/// Peeks head of hashqueue.
	/// </summary>
	public T Peek()
	{
		Ex.ThrowIfEmpty(IsEmpty(), "HashQueue is empty.");
		return dq.Peek();
	}

	/// <summary>
	/// Peeks tail of hashqueue.
	/// </summary>
	public T PeekTail()
	{
		Ex.ThrowIfEmpty(IsEmpty(), "HashQueue is empty.");
		return dq.PeekTail();
	}

	/// <summary>
	/// Returns true if <paramref name="x"/> is in hashqueue.
	/// </summary>
	public bool Has(T x)
	{
		x.ThrowIfArgumentNull(nameof(x));
		return map.ContainsKey(x);
	}

	/// <summary>
	/// Returns true if hashqueue is empty.
	/// </summary>
	public bool IsEmpty()
	{
		return dq.IsEmpty();
	}

	/// <summary>
	/// Returns count of hashqueue.
	/// </summary>
	public int Count()
	{
		return dq.Count();
	}

	/// <summary>
	/// Returns long count of hashqueue.
	/// </summary>
	public long LongCount()
	{
		return dq.LongCount();
	}

	/// <summary>
	/// Clears hashqueue.
	/// </summary>
	public void Clear()
	{
		dq.Clear();
		map.Clear();
	}

	#endregion

	#region IEnumerable<T> methods

	public IEnumerator<T> GetEnumerator()
	{
		return dq.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	#endregion
}

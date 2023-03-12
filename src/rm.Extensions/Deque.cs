﻿using System;
using System.Collections;
using System.Collections.Generic;
using rm.Extensions.Deque;
using Ex = rm.Extensions.ExceptionHelper;

namespace rm.Extensions;

/// <summary>
/// Defines deque methods.
/// </summary>
interface IDeque<T> : IEnumerable<T>
{
	/// <summary>
	/// Enqueues <paramref name="x"/> into deque.
	/// </summary>
	Node<T> Enqueue(T x);

	/// <summary>
	/// Dequeues head from deque.
	/// </summary>
	T Dequeue();

	/// <summary>
	/// Peeks head of deque.
	/// </summary>
	T Peek();

	/// <summary>
	/// Peeks tail of deque.
	/// </summary>
	T PeekTail();

	/// <summary>
	/// Deletes given <paramref name="node"/> from deque.
	/// </summary>
	void Delete(Node<T> node);

	/// <summary>
	/// Returns true if deque is empty.
	/// </summary>
	bool IsEmpty();

	/// <summary>
	/// Returns count of deque.
	/// </summary>
	int Count();

	/// <summary>
	/// Returns long count of deque.
	/// </summary>
	long LongCount();

	/// <summary>
	/// Clears deque.
	/// </summary>
	void Clear();

	/// <summary>
	/// Inserts <paramref name="x"/> before given <paramref name="node"/>.
	/// </summary>
	Node<T> InsertBefore(Node<T> node, T x);

	/// <summary>
	/// Inserts <paramref name="x"/> after given <paramref name="node"/>.
	/// </summary>
	Node<T> InsertAfter(Node<T> node, T x);

	/// <summary>
	/// Inserts <paramref name="node"/> as head.
	/// </summary>
	void InsertHead(Node<T> node);

	/// <summary>
	/// Inserts <paramref name="node"/> as tail.
	/// </summary>
	void InsertTail(Node<T> node);

	/// <summary>
	/// Make <paramref name="node"/> as head.
	/// </summary>
	void MakeHead(Node<T> node);

	/// <summary>
	/// Make <paramref name="node"/> as tail.
	/// </summary>
	void MakeTail(Node<T> node);

	// HACK: Either encapsulate or refactor with Remove(T x).
	/// <summary>
	/// Iterates over all nodes.
	/// </summary>
	IEnumerable<Node<T>> Nodes();
}

/// <summary>
/// Deque.
/// </summary>
/// <remarks>
/// Uses linked list as backing store.
///
/// All methods are O(1) time unless noted.
/// </remarks>
public class Deque<T> : IDeque<T>
{
	#region members

	private Node<T> head;
	private Node<T> tail;
	private long count;

	#endregion

	#region ctors

	public Deque()
	{ }

	#endregion

	#region IDeque<T> methods

	/// <summary>
	/// Enqueues <paramref name="x"/> into deque.
	/// </summary>
	/// <returns>The node for <paramref name="x"/>.</returns>
	public Node<T> Enqueue(T x)
	{
		var node = new Node<T>(x, this);
		if (IsEmpty())
		{
			head = tail = node;
		}
		else
		{
			tail.next = node;
			node.prev = tail;
			tail = node;
		}
		count++;
		return node;
	}

	/// <summary>
	/// Dequeues head from deque.
	/// </summary>
	public T Dequeue()
	{
		Ex.ThrowIfEmpty(IsEmpty(), "Deque is empty.");
		count--;
		var node = head;
		node.owner = null;
		if (head == tail)
		{
			head = tail = null;
		}
		else
		{
			head = head.next;
			node.next = head.prev = null;
		}
		return node.Value;
	}

	/// <summary>
	/// Peeks head of deque.
	/// </summary>
	public T Peek()
	{
		Ex.ThrowIfEmpty(IsEmpty(), "Deque is empty.");
		return head.Value;
	}

	/// <summary>
	/// Peeks tail of deque.
	/// </summary>
	public T PeekTail()
	{
		Ex.ThrowIfEmpty(IsEmpty(), "Deque is empty.");
		return tail.Value;
	}

	/// <summary>
	/// Deletes given <paramref name="node"/> from deque.
	/// </summary>
	public void Delete(Node<T> node)
	{
		node.ThrowIfArgumentNull(nameof(node));
		if (node.owner != this)
		{
			throw new InvalidOperationException("Node does not belong to the deque.");
		}
		Ex.ThrowIfEmpty(IsEmpty(), "Deque is empty.");
		count--;
		node.owner = null;
		if (node == head && node == tail)
		{
			head = tail = null;
		}
		else if (node == head)
		{
			head = head.next;
			node.next = head.prev = null;
		}
		else if (node == tail)
		{
			tail = tail.prev;
			tail.next = node.prev = null;
		}
		else
		{
			node.prev.next = node.next;
			node.next.prev = node.prev;
			node.next = node.prev = null;
		}
	}

	/// <summary>
	/// Returns true if deque is empty.
	/// </summary>
	public bool IsEmpty()
	{
		return count == 0;
	}

	/// <summary>
	/// Returns count of deque.
	/// </summary>
	public int Count()
	{
		return checked((int)count);
	}

	/// <summary>
	/// Returns long count of deque.
	/// </summary>
	public long LongCount()
	{
		return count;
	}

	/// <summary>
	/// Clears deque.
	/// </summary>
	public void Clear()
	{
		head = tail = null;
		count = 0;
	}

	/// <summary>
	/// Inserts <paramref name="x"/> before given <paramref name="node"/>.
	/// </summary>
	/// <returns>The node for <paramref name="x"/>.</returns>
	public Node<T> InsertBefore(Node<T> node, T x)
	{
		node.ThrowIfArgumentNull(nameof(node));
		if (node.owner != this)
		{
			throw new InvalidOperationException("Node does not belong to the deque.");
		}
		count++;
		var prev = node.prev;
		var xnode = new Node<T>(x, this);
		var next = node;
		xnode.next = next;
		next.prev = xnode;
		if (next == head)
		{
			head = xnode;
		}
		else
		{
			prev.next = xnode;
			xnode.prev = prev;
		}
		return xnode;
	}

	/// <summary>
	/// Inserts <paramref name="x"/> after given <paramref name="node"/>.
	/// </summary>
	/// <returns>The node for <paramref name="x"/>.</returns>
	public Node<T> InsertAfter(Node<T> node, T x)
	{
		node.ThrowIfArgumentNull(nameof(node));
		if (node.owner != this)
		{
			throw new InvalidOperationException("Node does not belong to the deque.");
		}
		count++;
		var prev = node;
		var xnode = new Node<T>(x, this);
		var next = node.next;
		xnode.prev = prev;
		prev.next = xnode;
		if (prev == tail)
		{
			tail = xnode;
		}
		else
		{
			xnode.next = next;
			next.prev = xnode;
		}
		return xnode;
	}

	/// <summary>
	/// Inserts <paramref name="node"/> as head.
	/// </summary>
	public void InsertHead(Node<T> node)
	{
		node.ThrowIfArgumentNull(nameof(node));
		if (node.owner != null)
		{
			throw new InvalidOperationException("Node is already in deque.");
		}
		node.owner = this;
		if (IsEmpty())
		{
			head = tail = node;
		}
		else
		{
			node.next = head;
			head.prev = node;
			head = node;
		}
		count++;
	}

	/// <summary>
	/// Inserts <paramref name="node"/> as tail.
	/// </summary>
	public void InsertTail(Node<T> node)
	{
		node.ThrowIfArgumentNull(nameof(node));
		if (node.owner != null)
		{
			throw new InvalidOperationException("Node is already in deque.");
		}
		node.owner = this;
		if (IsEmpty())
		{
			head = tail = node;
		}
		else
		{
			tail.next = node;
			node.prev = tail;
			tail = node;
		}
		count++;
	}

	/// <summary>
	/// Make <paramref name="node"/> as head.
	/// </summary>
	public void MakeHead(Node<T> node)
	{
		node.ThrowIfArgumentNull(nameof(node));
		if (node.owner != this)
		{
			throw new InvalidOperationException("Node does not belong to the deque.");
		}
		if (node == head)
		{
			return;
		}
		if (node == tail)
		{
			tail = node.prev;
			tail.next = node.prev = null;
		}
		else
		{
			node.prev.next = node.next;
			node.next.prev = node.prev;
			node.next = node.prev = null;
		}
		head.prev = node;
		node.next = head;
		head = node;
	}

	/// <summary>
	/// Make <paramref name="node"/> as tail.
	/// </summary>
	public void MakeTail(Node<T> node)
	{
		node.ThrowIfArgumentNull(nameof(node));
		if (node.owner != this)
		{
			throw new InvalidOperationException("Node does not belong to the deque.");
		}
		if (node == tail)
		{
			return;
		}
		if (node == head)
		{
			head = node.next;
			node.next = head.prev = null;
		}
		else
		{
			node.prev.next = node.next;
			node.next.prev = node.prev;
			node.next = node.prev = null;
		}
		tail.next = node;
		node.prev = tail;
		tail = node;
	}

	/// <summary>
	/// Iterates over all nodes.
	/// </summary>
	public IEnumerable<Node<T>> Nodes()
	{
		var node = head;
		while (node != null)
		{
			yield return node;
			node = node.next;
		}
	}

	#endregion

	#region IEnumerable<T> methods

	public IEnumerator<T> GetEnumerator()
	{
		var node = head;
		while (node != null)
		{
			yield return node.Value;
			node = node.next;
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	#endregion
}

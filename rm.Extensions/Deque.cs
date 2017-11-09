using System;
using System.Collections;
using System.Collections.Generic;
using rm.Extensions.Deque;

namespace rm.Extensions
{
	/// <summary>
	/// Defines deque methods.
	/// </summary>
	interface IDeque<T>
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
		/// Clears deque.
		/// </summary>
		void Clear();
	}

	namespace Deque
	{
		/// <summary>
		/// Deque node.
		/// </summary>
		public class Node<T>
		{
			public readonly T Value;
			internal Node<T> prev;
			internal Node<T> next;
			internal Deque<T> owner;

			internal Node(T value, Deque<T> owner)
			{
				this.Value = value;
				this.owner = owner;
			}
		}
	}

	/// <summary>
	/// Deque.
	/// </summary>
	/// <remarks>Uses linked list as backing store.</remarks>
	public class Deque<T> : IDeque<T>, IEnumerable<T>
	{
		#region members

		private Node<T> head;
		private Node<T> tail;
		private int count;

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
			if (IsEmpty())
			{
				throw new InvalidOperationException("Deque is empty.");
			}
			count--;
			var node = head;
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
			if (IsEmpty())
			{
				throw new InvalidOperationException("Deque is empty.");
			}
			return head.Value;
		}

		/// <summary>
		/// Peeks tail of deque.
		/// </summary>
		public T PeekTail()
		{
			if (IsEmpty())
			{
				throw new InvalidOperationException("Deque is empty.");
			}
			return tail.Value;
		}

		/// <summary>
		/// Deletes given <paramref name="node"/> from deque.
		/// </summary>
		/// <remarks>O(1) time.</remarks>
		public void Delete(Node<T> node)
		{
			node.ThrowIfArgumentNull(nameof(node));
			if (node.owner != this)
			{
				throw new InvalidOperationException("Node does not belong to the deque.");
			}
			if (IsEmpty())
			{
				throw new InvalidOperationException("Deque is empty.");
			}
			count--;
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
}

﻿using System;
using System.Collections.Generic;

namespace rm.Extensions
{
	/// <summary>
	/// Defines heap methods.
	/// </summary>
	interface IHeap<T, TKey>
		where TKey : IComparable<TKey>
	{
		/// <summary>
		/// Inserts <paramref name="x"/> into heap.
		/// </summary>
		void Insert(T x);

		/// <summary>
		/// Appends <paramref name="x"/> to heap without maintaining the heap property.
		/// </summary>
		void Append(T x);

		/// <summary>
		/// Deletes top of heap.
		/// </summary>
		T Delete();

		/// <summary>
		/// Captures the current top to return it and replace it with <paramref name="x"/> and sift down.
		/// </summary>
		/// <remarks>Displace() avoids having to call Delete() and Insert() separately.</remarks>
		T Displace(T x);

		/// <summary>
		/// Peeks top of heap.
		/// </summary>
		T Peek();

		/// <summary>
		/// Returns count of heap.
		/// </summary>
		int Count();

		/// <summary>
		/// Returns true if heap is empty.
		/// </summary>
		bool IsEmpty();

		/// <summary>
		/// Returns true if heap is full.
		/// </summary>
		bool IsFull();
	}

	/// <summary>
	/// Base abstract class for heap.
	/// </summary>
	/// <remarks>Uses array as backing store.</remarks>
	public abstract class HeapBase<T, TKey> : IHeap<T, TKey>, IEnumerable<T>
		where TKey : IComparable<TKey>
	{
		#region members

		protected int capacity;
		protected T[] heap;
		protected int count;
		protected Func<T, TKey> keySelector;
		protected IComparer<TKey> comparer;
		protected bool isHeapified = true;

		#endregion

		#region ctors

		/// <summary>
		/// HeapBase ctor.
		/// </summary>
		/// <param name="capacity">Capacity of heap.</param>
		/// <param name="keySelector">T's key to heapify against.</param>
		/// <param name="comparer">Comparer for T's key.</param>
		protected HeapBase(int capacity, Func<T, TKey> keySelector, IComparer<TKey> comparer)
		{
			capacity.ThrowIfArgumentOutOfRange(nameof(capacity));
			keySelector.ThrowIfArgumentNull(nameof(keySelector));
			comparer.ThrowIfArgumentNull(nameof(comparer));
			this.capacity = capacity;
			this.heap = new T[capacity];
			this.keySelector = keySelector;
			this.comparer = comparer;
		}

		#endregion

		#region methods

		#region IHeap<T> methods

		/// <summary>
		/// Inserts <paramref name="x"/> into heap.
		/// </summary>
		public void Insert(T x)
		{
			if (IsFull())
			{
				throw new InvalidOperationException("Heap is full.");
			}
			if (!isHeapified)
			{
				Heapify();
			}
			heap[count] = x;
			count++;
			SiftUp(count - 1);
		}

		/// <summary>
		/// Appends <paramref name="x"/> to heap without maintaining the heap property.
		/// </summary>
		public void Append(T x)
		{
			if (IsFull())
			{
				throw new InvalidOperationException("Heap is full.");
			}
			isHeapified = false;
			heap[count] = x;
			count++;
		}

		/// <summary>
		/// Deletes top of heap.
		/// </summary>
		public T Delete()
		{
			if (IsEmpty())
			{
				throw new InvalidOperationException("Heap is empty.");
			}
			if (!isHeapified)
			{
				Heapify();
			}
			T x = heap[0];
			Helper.Swap(heap, 0, count - 1);
			count--;
			SiftDown(0);
			return x;
		}

		/// <summary>
		/// Captures the current top to return it and replace it with <paramref name="x"/> and sift down.
		/// </summary>
		/// <remarks>Displace() avoids having to call Delete() and Insert() separately.</remarks>
		public T Displace(T x)
		{
			x.ThrowIfArgumentNull(nameof(x));
			if (IsEmpty())
			{
				throw new InvalidOperationException("Heap is empty.");
			}
			if (!isHeapified)
			{
				Heapify();
			}
			T top = heap[0];
			heap[0] = x;
			SiftDown(0);
			return top;
		}

		/// <summary>
		/// Peeks top of heap.
		/// </summary>
		public T Peek()
		{
			if (IsEmpty())
			{
				throw new InvalidOperationException("Heap is empty.");
			}
			if (!isHeapified)
			{
				Heapify();
			}
			return heap[0];
		}

		/// <summary>
		/// Returns count of heap.
		/// </summary>
		public int Count()
		{
			return count;
		}

		/// <summary>
		/// Returns true if heap is empty.
		/// </summary>
		public bool IsEmpty()
		{
			return count == 0;
		}

		/// <summary>
		/// Returns true if heap is full.
		/// </summary>
		public bool IsFull()
		{
			return count == capacity;
		}

		#endregion

		#region IEnumerable<T> methods

		public IEnumerator<T> GetEnumerator()
		{
			if (!isHeapified)
			{
				Heapify();
			}
			for (int i = 0; i < count; i++)
			{
				yield return heap[i];
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		#region private methods

		/// <summary>
		/// Sifts down from start to end of heap.
		/// </summary>
		private void SiftDown(int start)
		{
			var i = start;
			while (i < count)
			{
				var left = i * 2 + 1;
				var right = left + 1;
				var m = i;
				if (left < count && !IsInHeapProperty(left, m))
				{
					m = left;
				}
				if (right < count && !IsInHeapProperty(right, m))
				{
					m = right;
				}
				if (i == m)
				{
					break;
				}
				Helper.Swap(heap, i, m);
				i = m;
			}
		}

		/// <summary>
		/// Sifts up from end to start of heap.
		/// </summary>
		private void SiftUp(int end)
		{
			var i = end;
			while (i > 0)
			{
				var parent = (i - 1) / 2;
				if (IsInHeapProperty(i, parent))
				{
					break;
				}
				Helper.Swap(heap, i, parent);
				i = parent;
			}
		}

		/// <summary>
		/// Heapifies using sift up.
		/// </summary>
		/// <remarks>O(nlogn) time.</remarks>
		private void HeapifySiftUp()
		{
			var start = 0;
			while (start < count)
			{
				SiftUp(start);
				start++;
			}
		}

		/// <summary>
		/// Heapifies using sift down.
		/// </summary>
		/// <remarks>O(n) time.</remarks>
		private void HeapifySiftDown()
		{
			var start = count / 2 - 1; // last parent
			while (start >= 0)
			{
				SiftDown(start);
				start--;
			}
		}

		/// <summary>
		/// Heapifies.
		/// </summary>
		private void Heapify()
		{
			// sift down is faster than using sift up
			HeapifySiftDown();
			isHeapified = true;
		}

		#endregion

		#region abstract methods

		/// <summary>
		/// Returns true if <paramref name="i"/> and <paramref name="parent"/> are in heap property.
		/// </summary>
		protected abstract bool IsInHeapProperty(int i, int parent);

		#endregion

		#endregion
	}

	/// <summary>
	/// Min heap.
	/// </summary>
	public class MinHeap<T, TKey> : HeapBase<T, TKey>
		where TKey : IComparable<TKey>
	{
		#region ctors

		/// <summary>
		/// MinHeap ctor.
		/// </summary>
		/// <param name="capacity">Capacity of heap.</param>
		/// <param name="keySelector">T's key to heapify against.</param>
		public MinHeap(int capacity, Func<T, TKey> keySelector)
			: this(capacity, keySelector, Comparer<TKey>.Default)
		{ }

		/// <summary>
		/// MinHeap ctor.
		/// </summary>
		/// <param name="capacity">Capacity of heap.</param>
		/// <param name="keySelector">T's key to heapify against.</param>
		/// <param name="comparer">Comparer for T's key.</param>
		public MinHeap(int capacity, Func<T, TKey> keySelector, IComparer<TKey> comparer)
			: base(capacity, keySelector, comparer)
		{ }

		#endregion

		#region HeapBase<T, TKey> methods

		/// <summary>
		/// Returns true if <paramref name="i"/> and <paramref name="parent"/> are in heap property.
		/// </summary>
		protected override bool IsInHeapProperty(int i, int parent)
		{
			return comparer.Compare(keySelector(heap[parent]), keySelector(heap[i])) <= 0;
		}

		#endregion
	}

	/// <summary>
	/// Min heap.
	/// </summary>
	public class MinHeap<T> : MinHeap<T, T>
		where T : IComparable<T>
	{
		#region ctors

		/// <summary>
		/// MinHeap ctor.
		/// </summary>
		/// <param name="capacity">Capacity of heap.</param>
		public MinHeap(int capacity)
			: this(capacity, Comparer<T>.Default)
		{ }

		/// <summary>
		/// MinHeap ctor.
		/// </summary>
		/// <param name="capacity">Capacity of heap.</param>
		/// <param name="comparer">Comparer for T.</param>
		public MinHeap(int capacity, IComparer<T> comparer)
			: base(capacity, x => x, comparer)
		{ }

		#endregion
	}

	/// <summary>
	/// Max heap.
	/// </summary>
	public class MaxHeap<T, TKey> : HeapBase<T, TKey>, IEnumerable<T>
		where TKey : IComparable<TKey>
	{
		#region ctors

		/// <summary>
		/// MaxHeap ctor.
		/// </summary>
		/// <param name="capacity">Capacity of heap.</param>
		/// <param name="keySelector">T's key to heapify against.</param>
		public MaxHeap(int capacity, Func<T, TKey> keySelector)
			: this(capacity, keySelector, Comparer<TKey>.Default)
		{ }

		/// <summary>
		/// MaxHeap ctor.
		/// </summary>
		/// <param name="capacity">Capacity of heap.</param>
		/// <param name="keySelector">T's key to heapify against.</param>
		/// <param name="comparer">Comparer for T's key.</param>
		public MaxHeap(int capacity, Func<T, TKey> keySelector, IComparer<TKey> comparer)
			: base(capacity, keySelector, comparer)
		{ }

		#endregion

		#region HeapBase<T, TKey> methods

		/// <summary>
		/// Returns true if <paramref name="i"/> and <paramref name="parent"/> are in heap property.
		/// </summary>
		protected override bool IsInHeapProperty(int i, int parent)
		{
			return comparer.Compare(keySelector(heap[parent]), keySelector(heap[i])) >= 0;
		}

		#endregion
	}

	/// <summary>
	/// Max heap.
	/// </summary>
	public class MaxHeap<T> : MaxHeap<T, T>
		where T : IComparable<T>
	{
		#region ctors

		/// <summary>
		/// MaxHeap ctor.
		/// </summary>
		/// <param name="capacity">Capacity of heap.</param>
		public MaxHeap(int capacity)
			: this(capacity, Comparer<T>.Default)
		{ }

		/// <summary>
		/// MaxHeap ctor.
		/// </summary>
		/// <param name="capacity">Capacity of heap.</param>
		/// <param name="comparer">Comparer for T.</param>
		public MaxHeap(int capacity, IComparer<T> comparer)
			: base(capacity, x => x, comparer)
		{ }

		#endregion
	}
}

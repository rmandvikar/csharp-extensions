using System;
using System.Collections.Generic;

namespace rm.Extensions;

/// <summary>
/// Defines heap methods.
/// </summary>
public interface IHeap<T, TKey> : IEnumerable<T>
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
/// Defines heap methods.
/// </summary>
public interface IHeap<T> : IEnumerable<T>
	where T : IComparable<T>
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
/// Heap helper class.
/// </summary>
/// <remarks>Uses array as backing store.</remarks>
internal class HeapHelper<T> : IEnumerable<T>
{
	#region members

	protected int capacity;
	protected T[] heap;
	protected int count;
	private IHeapPropertyComparer<T> heapPropertyComparer;
	protected bool isHeapified = true;

	#endregion

	#region ctors

	/// <summary>
	/// HeapHelper ctor.
	/// </summary>
	/// <param name="capacity">Capacity of heap.</param>
	/// <param name="heapPropertyComparer">Heap property comparer for T.</param>
	internal protected HeapHelper(int capacity,
		IHeapPropertyComparer<T> heapPropertyComparer)
	{
		capacity.ThrowIfArgumentOutOfRange(nameof(capacity));
		heapPropertyComparer.ThrowIfArgumentNull(nameof(heapPropertyComparer));
		this.capacity = capacity;
		this.heapPropertyComparer = heapPropertyComparer;
		this.heap = new T[capacity];
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

	/// <summary>
	/// Returns true if child and parent are in heap property.
	/// </summary>
	private bool IsInHeapProperty(int item, int parent)
	{
		// pass through
		return heapPropertyComparer.IsInHeapProperty(heap[item], heap[parent]);
	}

	#endregion

	#endregion
}

#region HeapPropertyComparer classes

/// <summary>
/// Defines heap property methods.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IHeapPropertyComparer<T>
{
	/// <summary>
	/// Returns true if <paramref name="item"/> and <paramref name="parent"/>
	/// are in heap property.
	/// </summary>
	bool IsInHeapProperty(T item, T parent);
}

public class MinHeapPropertyComparer<T, TKey> : IHeapPropertyComparer<T>
	where TKey : IComparable<TKey>
{
	private Func<T, TKey> keySelector;
	private IComparer<TKey> comparer;

	public MinHeapPropertyComparer(Func<T, TKey> keySelector, IComparer<TKey> comparer)
	{
		keySelector.ThrowIfArgumentNull(nameof(keySelector));
		comparer.ThrowIfArgumentNull(nameof(comparer));
		this.keySelector = keySelector;
		this.comparer = comparer;
	}

	#region IHeapPropertyComparer<T> methods

	public bool IsInHeapProperty(T item, T parent)
	{
		// parent <= child
		return comparer.Compare(keySelector(parent), keySelector(item)) <= 0;
	}

	#endregion
}

public class MaxHeapPropertyComparer<T, TKey> : IHeapPropertyComparer<T>
	where TKey : IComparable<TKey>
{
	private Func<T, TKey> keySelector;
	private IComparer<TKey> comparer;

	public MaxHeapPropertyComparer(Func<T, TKey> keySelector, IComparer<TKey> comparer)
	{
		keySelector.ThrowIfArgumentNull(nameof(keySelector));
		comparer.ThrowIfArgumentNull(nameof(comparer));
		this.keySelector = keySelector;
		this.comparer = comparer;
	}

	#region IHeapPropertyComparer<T> methods

	public bool IsInHeapProperty(T item, T parent)
	{
		// parent >= child
		return comparer.Compare(keySelector(parent), keySelector(item)) >= 0;
	}

	#endregion
}

public class MinHeapPropertyComparer<T> : MinHeapPropertyComparer<T, T>
	where T : IComparable<T>
{
	public MinHeapPropertyComparer(IComparer<T> comparer)
		: base(x => x, comparer)
	{ }
}

public class MaxHeapPropertyComparer<T> : MaxHeapPropertyComparer<T, T>
	where T : IComparable<T>
{
	public MaxHeapPropertyComparer(IComparer<T> comparer)
		: base(x => x, comparer)
	{ }
}

#endregion

/// <summary>
/// HeapBase class.
/// </summary>
public class HeapBase<T, TKey> : IHeap<T, TKey>
	where TKey : IComparable<TKey>
{
	private HeapHelper<T> heapHelper;

	/// <summary>
	/// HeapBase.
	/// </summary>
	/// <param name="capacity">Capacity of heap.</param>
	/// <param name="heapPropertyComparer">Heap property comparer for T.</param>
	public HeapBase(int capacity, IHeapPropertyComparer<T> heapPropertyComparer)
	{
		heapHelper = new HeapHelper<T>(capacity, heapPropertyComparer);
	}

	#region IHeap<T, TKey> methods

	public void Append(T x)
	{
		heapHelper.Append(x);
	}

	public int Count()
	{
		return heapHelper.Count();
	}

	public T Delete()
	{
		return heapHelper.Delete();
	}

	public T Displace(T x)
	{
		return heapHelper.Displace(x);
	}

	public void Insert(T x)
	{
		heapHelper.Insert(x);
	}

	public bool IsEmpty()
	{
		return heapHelper.IsEmpty();
	}

	public bool IsFull()
	{
		return heapHelper.IsFull();
	}

	public T Peek()
	{
		return heapHelper.Peek();
	}

	#endregion

	#region IEnumerable<T> methods

	public IEnumerator<T> GetEnumerator()
	{
		return heapHelper.GetEnumerator();
	}

	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

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
		: base(capacity, new MinHeapPropertyComparer<T, TKey>(keySelector, comparer))
	{ }

	#endregion
}

/// <summary>
/// Max heap.
/// </summary>
public class MaxHeap<T, TKey> : HeapBase<T, TKey>
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
		: base(capacity, new MaxHeapPropertyComparer<T, TKey>(keySelector, comparer))
	{ }

	#endregion
}

/// <summary>
/// HeapBase class.
/// </summary>
public class HeapBase<T> : IHeap<T>
	where T : IComparable<T>
{
	private HeapHelper<T> heapHelper;

	/// <summary>
	/// HeapBase.
	/// </summary>
	/// <param name="capacity">Capacity of heap.</param>
	/// <param name="heapPropertyComparer">Heap property comparer for T.</param>
	public HeapBase(int capacity, IHeapPropertyComparer<T> heapPropertyComparer)
	{
		heapHelper = new HeapHelper<T>(capacity, heapPropertyComparer);
	}

	#region IHeap<T> methods

	public void Append(T x)
	{
		heapHelper.Append(x);
	}

	public int Count()
	{
		return heapHelper.Count();
	}

	public T Delete()
	{
		return heapHelper.Delete();
	}

	public T Displace(T x)
	{
		return heapHelper.Displace(x);
	}

	public void Insert(T x)
	{
		heapHelper.Insert(x);
	}

	public bool IsEmpty()
	{
		return heapHelper.IsEmpty();
	}

	public bool IsFull()
	{
		return heapHelper.IsFull();
	}

	public T Peek()
	{
		return heapHelper.Peek();
	}

	#endregion

	#region IEnumerable<T> methods

	public IEnumerator<T> GetEnumerator()
	{
		return heapHelper.GetEnumerator();
	}

	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	#endregion
}

/// <summary>
/// Min heap.
/// </summary>
public class MinHeap<T> : HeapBase<T>
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
		: base(capacity, new MinHeapPropertyComparer<T>(comparer))
	{ }

	#endregion
}

/// <summary>
/// Max heap.
/// </summary>
public class MaxHeap<T> : HeapBase<T>
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
		: base(capacity, new MaxHeapPropertyComparer<T>(comparer))
	{ }

	#endregion
}

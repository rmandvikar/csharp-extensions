using System.Collections.Generic;

namespace rm.Extensions
{
	/// <summary>
	/// Defines LRU cache methods.
	/// </summary>
	interface ILruCache<TKey, TValue>
	{
		/// <summary>
		/// Gets value for <paramref name="key"/> from cache.
		/// </summary>
		/// <remarks>
		/// Makes item MRU.
		/// </remarks>
		TValue Get(TKey key);

		/// <summary>
		/// Inserts <paramref name="value"/> for <paramref name="key"/> in cache.
		/// </summary>
		/// <remarks>
		/// Makes item MRU (also if <paramref name="key"/> already exists).
		/// </remarks>
		void Insert(TKey key, TValue value);

		/// <summary>
		/// Removes <paramref name="key"/> from cache.
		/// </summary>
		bool Remove(TKey key);

		/// <summary>
		/// Returns true if <paramref name="key"/> exists in cache.
		/// </summary>
		bool HasKey(TKey key);

		/// <summary>
		/// Returns true if cache is empty.
		/// </summary>
		bool IsEmpty();

		/// <summary>
		/// Returns true if cache is full.
		/// </summary>
		bool IsFull();

		/// <summary>
		/// Returns capacity of cache.
		/// </summary>
		int Capacity();

		/// <summary>
		/// Returns count of cache.
		/// </summary>
		int Count();

		/// <summary>
		/// Clears cache.
		/// </summary>
		void Clear();
	}

	/// <summary>
	/// LRU cache.
	/// </summary>
	/// <remarks>
	/// Uses deque and map as backing store.
	///
	/// All methods are O(1) time.
	/// </remarks>
	public class LruCache<TKey, TValue> : ILruCache<TKey, TValue>
	{
		#region members

		private readonly int n;
		private readonly IDeque<TKey> dq;
		private readonly IDictionary<TKey, (Deque.Node<TKey>, TValue)> map;

		#endregion

		#region ctors

		public LruCache(int n)
		{
			n.ThrowIfArgumentOutOfRange(nameof(n));
			this.n = n;
			dq = new Deque<TKey>();
			map = new Dictionary<TKey, (Deque.Node<TKey>, TValue)>(capacity: n);
		}

		#endregion

		#region ILruCache<TKey, TValue> methods

		/// <summary>
		/// Gets value for <paramref name="key"/> from cache.
		/// </summary>
		/// <remarks>
		/// Makes item MRU.
		/// </remarks>
		public TValue Get(TKey key)
		{
			if (!map.ContainsKey(key))
			{
				return default(TValue);
			}
			var (node, value) = map[key];
			dq.MakeTail(node);
			return value;
		}

		/// <summary>
		/// Inserts <paramref name="value"/> for <paramref name="key"/> in cache.
		/// </summary>
		/// <remarks>
		/// Makes item MRU (also if <paramref name="key"/> already exists).
		/// </remarks>
		public void Insert(TKey key, TValue value)
		{
			key.ThrowIfArgumentNull(nameof(key));
			if (Capacity() == 0)
			{
				return;
			}
			if (map.ContainsKey(key))
			{
				var (node, _) = map[key];
				dq.MakeTail(node);
				map[key] = (node, value);
				return;
			}
			if (IsFull())
			{
				// remove lru
				map.Remove(dq.Dequeue());
			}
			map[key] = (dq.Enqueue(key), value);
		}

		/// <summary>
		/// Removes <paramref name="key"/> from cache.
		/// </summary>
		public bool Remove(TKey key)
		{
			if (!map.ContainsKey(key))
			{
				return false;
			}
			var (node, _) = map[key];
			dq.Delete(node);
			map.Remove(key);
			return true;
		}

		/// <summary>
		/// Returns true if <paramref name="key"/> exists in cache.
		/// </summary>
		public bool HasKey(TKey key)
		{
			return map.ContainsKey(key);
		}

		/// <summary>
		/// Returns true if cache is empty.
		/// </summary>
		public bool IsEmpty()
		{
			return Count() == 0;
		}

		/// <summary>
		/// Returns true if cache is full.
		/// </summary>
		public bool IsFull()
		{
			return Count() == Capacity();
		}

		/// <summary>
		/// Returns capacity of cache.
		/// </summary>
		public int Capacity()
		{
			return n;
		}

		/// <summary>
		/// Returns count of cache.
		/// </summary>
		public int Count()
		{
			return map.Count;
		}

		/// <summary>
		/// Clears cache.
		/// </summary>
		public void Clear()
		{
			dq.Clear();
			map.Clear();
		}

		#endregion
	}
}

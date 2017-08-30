using System;
using System.Collections;
using System.Collections.Generic;
using Ex = rm.Extensions.ExceptionHelper;

namespace rm.Extensions
{
	/// <summary>
	/// BitSet is a set that uses bit arithmatic to save space.
	/// </summary>
	/// <remarks>
	/// BitSet is space efficient compared to bool[] if the set is dense and not sparse.
	/// <para>
	/// Note: Cannot implement IEnumerable{int}.GetEnumerator() without loss of information.
	/// </para>
	/// </remarks>
	public class BitSet : IEnumerable<uint>, ICollection<uint>, IReadOnlyCollection<uint>
	{
		#region data members

		/// <summary>
		/// int array used as bit array for flags (32 bits for each int).
		/// </summary>
		/// <remarks>32, bytes: 4, binary: 11111 (+1), mask: 0x1f, power: 2^5</remarks>
		internal readonly int[] flags;

		/// <summary>
		/// The max int allowed in BitSet.
		/// </summary>
		public uint Max { get; }

		/// <summary>
		/// The number of elements in BitSet.
		/// </summary>
		public int Count { get; private set; }

		#endregion

		#region ctors

		/// <summary>
		/// Creates a BitSet for 0 to max, both inclusive.
		/// </summary>
		/// <param name="max">Max (inclusive), including 0.</param>
		public BitSet(uint max)
		{
			max.ThrowIfArgumentOutOfRange(nameof(max));
			Max = max;
			int size = (int)((max >> 5) + 1);
			flags = new int[size];
		}

		/// <summary>
		/// Creates a BitSet for 0 to max, both inclusive.
		/// </summary>
		/// <param name="max">Max (inclusive), including 0.</param>
		public BitSet(int max)
			: this((uint)(max.ThrowIfArgumentOutOfRange(nameof(max))))
		{ }

		#endregion

		#region methods

		/// <summary>
		/// Returns true if BitSet contains <paramref name="n"/>.
		/// </summary>
		public bool Has(uint n)
		{
			n.ThrowIfArgumentOutOfRange(nameof(n), maxRange: Max);
			uint index = (n >> 5);
			int offset = (int)(n & 0x1f);
			return Has(index, offset);
		}

		/// <summary>
		/// Returns true if BitSet has the flags[<paramref name="index"/>]'s
		/// <paramref name="offset"/> bit set.
		/// </summary>
		private bool Has(uint index, int offset)
		{
			return ((flags[index] >> offset) & 1) == 1;
		}

		/// <summary>
		/// Returns true if BitSet contains <paramref name="n"/>.
		/// </summary>
		public bool Has(int n)
		{
			Ex.ThrowIfArgumentOutOfRange(!(0 <= n && n <= Max), nameof(n));
			return Has((uint)n);
		}

		/// <summary>
		/// Adds <paramref name="n"/>.
		/// </summary>
		public void Add(uint n)
		{
			n.ThrowIfArgumentOutOfRange(nameof(n), maxRange: Max);
			uint index = (n >> 5);
			int offset = (int)(n & 0x1f);
			if (Has(index, offset))
			{
				return;
			}
			flags[index] |= (1 << offset);
			Count++;
		}

		/// <summary>
		/// Adds <paramref name="n"/>.
		/// </summary>
		public void Add(int n)
		{
			Ex.ThrowIfArgumentOutOfRange(!(0 <= n && n <= Max), nameof(n));
			Add((uint)n);
		}

		/// <summary>
		/// Removes <paramref name="n"/>.
		/// </summary>
		/// <returns>Returns true if removed else false.</returns>
		public bool Remove(uint n)
		{
			n.ThrowIfArgumentOutOfRange(nameof(n), maxRange: Max);
			uint index = (n >> 5);
			int offset = (int)(n & 0x1f);
			if (!Has(index, offset))
			{
				return false;
			}
			flags[index] &= ~(1 << offset);
			Count--;
			return true;
		}

		/// <summary>
		/// Removes <paramref name="n"/>.
		/// </summary>
		/// <returns>Returns true if removed else false.</returns>
		public bool Remove(int n)
		{
			Ex.ThrowIfArgumentOutOfRange(!(0 <= n && n <= Max), nameof(n));
			return Remove((uint)n);
		}

		/// <summary>
		/// Toggles <paramref name="n"/>.
		/// </summary>
		public void Toggle(uint n)
		{
			n.ThrowIfArgumentOutOfRange(nameof(n), maxRange: Max);
			uint index = (n >> 5);
			int offset = (int)(n & 0x1f);
			flags[index] ^= (1 << offset);
			Count += Has(index, offset) ? +1 : -1;
		}

		/// <summary>
		/// Toggles <paramref name="n"/>.
		/// </summary>
		public void Toggle(int n)
		{
			Ex.ThrowIfArgumentOutOfRange(!(0 <= n && n <= Max), nameof(n));
			Toggle((uint)n);
		}

		/// <summary>
		/// Clears the BitSet.
		/// </summary>
		/// <remarks>This is expensive.</remarks>
		public void Clear()
		{
			for (int i = 0; i < flags.Length; i++)
			{
				flags[i] = 0;
			}
			Count = 0;
		}

		#region IEnumerable<uint> methods

		public IEnumerator<uint> GetEnumerator()
		{
			var ycount = 0;
			for (uint i = 0; i <= Max; i++)
			{
				if (Has(i))
				{
					yield return i;
					ycount++;
					if (ycount == Count)
					{
						yield break;
					}
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		// Note: Cannot implement IEnumerable<int>.GetEnumerator() without loss of information

		#endregion

		#region ICollection<uint> methods

		public bool Contains(uint n)
		{
			return Has(n);
		}

		public bool Contains(int n)
		{
			return Has((uint)n);
		}

		public bool IsReadOnly => false;

		[Obsolete("Not implemented", true)]
		public void CopyTo(uint[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		#endregion

		#endregion
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace rm.Extensions
{
	/// <summary>
	/// IEnumerable extensions.
	/// </summary>
	public static class EnumerableExtension
	{
		/// <summary>
		/// Splits the collection into collections of size chunkSize.
		/// </summary>
		/// <remarks>
		/// Uses yield return. But uses ElementAt(index) which is inefficient.
		/// </remarks>
		[Obsolete]
		internal static IEnumerable<IEnumerable<T>> Chunk_bad1<T>(this IEnumerable<T> source,
			int chunkSize)
		{
			source.ThrowIfArgumentNull(nameof(source));
			chunkSize.ThrowIfArgumentOutOfRange(nameof(chunkSize));
			// to avoid inefficiency due to Count()
			var totalCount = source.Count();
			for (int start = 0; start < totalCount; start += chunkSize)
			{
				// note: skip/take is slow. not O(n) but (n/chunkSize)^2.
				// yield return source.Skip(chunk).Take(chunkSize);
				yield return source.Chunk_bad1(chunkSize, start, totalCount);
			}
		}

		/// <summary>
		/// Yields the next chunkSize elements starting at start and break if no more elements left.
		/// </summary>
		[Obsolete]
		private static IEnumerable<T> Chunk_bad1<T>(this IEnumerable<T> source,
			int chunkSize, int start, int totalCount)
		{
			source.ThrowIfArgumentNull(nameof(source));
			chunkSize.ThrowIfArgumentOutOfRange(nameof(chunkSize));
			for (int i = 0; i < chunkSize && start + i < totalCount; i++)
			{
				// note: source.ElementAt(index) is inefficient
				yield return source.ElementAt(start + i);
			}
		}

		/// <summary>
		/// Splits the collection into collections of size chunkSize.
		/// </summary>
		/// <remarks>
		/// Uses yield return and enumerator. But does not work with other methods as Count(), ElementAt(index), etc.
		/// </remarks>
		[Obsolete]
		internal static IEnumerable<IEnumerable<T>> Chunk_bad2<T>(this IEnumerable<T> source,
			int chunkSize)
		{
			source.ThrowIfArgumentNull(nameof(source));
			chunkSize.ThrowIfArgumentOutOfRange(nameof(chunkSize));
			var enumerator = source.GetEnumerator();
			while (enumerator.MoveNext())
			{
				yield return Chunk_bad2(chunkSize, enumerator);
			}
		}

		/// <summary>
		/// Yields the next chunkSize elements till the enumerator has any.
		/// </summary>
		[Obsolete]
		private static IEnumerable<T> Chunk_bad2<T>(int chunkSize, IEnumerator<T> enumerator)
		{
			var count = 0;
			do
			{
				yield return enumerator.Current;
				count++;
			} while (count < chunkSize && enumerator.MoveNext());
		}

		/// <summary>
		/// Splits the collection into collections of size chunkSize.
		/// </summary>
		/// <remarks>
		/// Uses yield return but buffers the chunk before returning. Works with other methods 
		/// as Count(), ElementAt(), etc.
		/// </remarks>
		public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source,
			int chunkSize)
		{
			source.ThrowIfArgumentNull(nameof(source));
			chunkSize.ThrowIfArgumentOutOfRange(nameof(chunkSize));
			var count = 0;
			var chunk = new List<T>(chunkSize);
			foreach (var item in source)
			{
				chunk.Add(item);
				count++;
				if (count == chunkSize)
				{
					yield return chunk.AsEnumerable();
					chunk = new List<T>(chunkSize);
					count = 0;
				}
			}
			if (count > 0)
			{
				yield return chunk.AsEnumerable();
			}
		}

		/// <summary>
		/// Returns true if collection is null or empty.
		/// </summary>
		public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
		{
			return source == null || !source.Any();
		}

		/// <summary>
		/// Returns specified value if source is null/empty/else same.
		/// </summary>
		public static IEnumerable<T> Or<T>(this IEnumerable<T> source, IEnumerable<T> or)
		{
			if (source.IsNullOrEmpty())
			{
				return or;
			}
			return source;
		}

		/// <summary>
		/// Splits a collection into n parts.
		/// </summary>
		/// <remarks>http://stackoverflow.com/questions/438188/split-a-collection-into-n-parts-with-linq</remarks>
		public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source, int parts)
		{
			source.ThrowIfArgumentNull(nameof(source));
			parts.ThrowIfArgumentOutOfRange(nameof(parts));
			// requires more space for objects
			//var splits_index = source.Select((x, index) => new { x, index = index })
			//	.GroupBy(x => x.index % parts)
			//	.Select(g => g.Select(x => x));
			int i = 0;
			var splits = source.GroupBy(x => i++ % parts)
				.Select(g => g.Select(x => x));
			return splits;
		}

		/// <summary>
		/// Returns true if list is ascendingly or descendingly sorted.
		/// </summary>
		public static bool IsSorted<T>(this IEnumerable<T> source)
			where T : IComparable, IComparable<T>
		{
			source.ThrowIfArgumentNull(nameof(source));
			// make an array to avoid inefficiency due to ElementAt(index)
			var sourceArray = source.ToArray();
			if (sourceArray.Length <= 1)
			{
				return true;
			}
			var isSorted = false;
			// asc test
			int i;
			for (i = 1; i < sourceArray.Length; i++)
			{
				if (sourceArray[i - 1].CompareTo(sourceArray[i]) > 0)
				{
					break;
				}
			}
			isSorted = sourceArray.Length == i;
			if (isSorted)
			{
				return true;
			}
			// desc test
			for (i = 1; i < sourceArray.Length; i++)
			{
				if (sourceArray[i - 1].CompareTo(sourceArray[i]) < 0)
				{
					break;
				}
			}
			isSorted = sourceArray.Length == i;
			return isSorted;
		}

		/// <summary>
		/// Returns the only two elements of a sequence.
		/// </summary>
		public static IEnumerable<T> Double<T>(this IEnumerable<T> source)
		{
			source.ThrowIfArgumentNull(nameof(source));
			return XOrDefaultInternal(source, count: 2, emptyCheck: false);
		}

		/// <summary>
		///  Returns the only two elements of a sequence that satisfy a specified condition.
		/// </summary>
		public static IEnumerable<T> Double<T>(this IEnumerable<T> source,
			Func<T, bool> predicate)
		{
			source.ThrowIfArgumentNull(nameof(source));
			predicate.ThrowIfArgumentNull(nameof(predicate));
			return Double(source.Where(predicate));
		}

		/// <summary>
		/// Returns the only two elements of a sequence, or a default value if the sequence is empty.
		/// </summary>
		public static IEnumerable<T> DoubleOrDefault<T>(this IEnumerable<T> source)
		{
			source.ThrowIfArgumentNull(nameof(source));
			return XOrDefaultInternal(source, count: 2, emptyCheck: true);
		}

		/// <summary>
		/// Returns the only two elements of a sequence that satisfy a specified condition 
		/// or a default value if no such elements exists.
		/// </summary>
		public static IEnumerable<T> DoubleOrDefault<T>(this IEnumerable<T> source,
			Func<T, bool> predicate)
		{
			source.ThrowIfArgumentNull(nameof(source));
			predicate.ThrowIfArgumentNull(nameof(predicate));
			return DoubleOrDefault(source.Where(predicate));
		}

		/// <summary>
		/// Returns the only <paramref name="count"/> elements of a sequence 
		/// or a default value if no such elements exists depending on <paramref name="emptyCheck"/>.
		/// </summary>
		private static IEnumerable<T> XOrDefaultInternal<T>(IEnumerable<T> source,
			int count, bool emptyCheck)
		{
			source.ThrowIfArgumentNull(nameof(source));
			count.ThrowIfArgumentOutOfRange(nameof(count));
			if (emptyCheck && source.IsNullOrEmpty())
			{
				return null;
			}
			// source.Count() == count is inefficient for large enumerable
			if (source.HasCount(count))
			{
				return source;
			}
			throw new InvalidOperationException(
				$"The input sequence does not contain {count} elements."
				);
		}

		/// <summary>
		/// Returns true if source has exactly <paramref name="count"/> elements efficiently.
		/// </summary>
		/// <remarks>
		/// Based on <see cref="Enumerable.Count{TSource}(IEnumerable{TSource})"/> method.
		/// </remarks>
		public static bool HasCount<TSource>(this IEnumerable<TSource> source, int count)
		{
			source.ThrowIfArgumentNull(nameof(source));
			count.ThrowIfArgumentOutOfRange(nameof(count));
			var collection = source as ICollection<TSource>;
			if (collection != null)
			{
				return collection.Count == count;
			}
			var collection2 = source as ICollection;
			if (collection2 != null)
			{
				return collection2.Count == count;
			}
			int num = 0;
			checked
			{
				using (var enumerator = source.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						num++;
						if (num > count)
						{
							return false;
						}
					}
				}
			}
			if (num < count)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Returns true if source has exactly <paramref name="count"/> elements
		/// that satisfy a specified condition efficiently.
		/// </summary>
		/// <remarks>
		/// Based on <see cref="Enumerable.Count{TSource}(IEnumerable{TSource})"/> method.
		/// Intentionally not fused with <see cref="HasCount{TSource}(IEnumerable{TSource}, int)"/>
		/// method.
		/// </remarks>
		public static bool HasCount<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate,
			int count)
		{
			source.ThrowIfArgumentNull(nameof(source));
			predicate.ThrowIfArgumentNull(nameof(predicate));
			count.ThrowIfArgumentOutOfRange(nameof(count));
			int num = 0;
			checked
			{
				using (var enumerator = source.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						var item = enumerator.Current;
						if (!predicate(item))
						{
							continue;
						}
						num++;
						if (num > count)
						{
							return false;
						}
					}
				}
			}
			if (num < count)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Returns true if source has at least <paramref name="count"/> elements efficiently.
		/// </summary>
		/// <remarks>
		/// Based on <see cref="Enumerable.Count{TSource}(IEnumerable{TSource})"/> method.
		/// </remarks>
		public static bool HasCountOfAtLeast<TSource>(this IEnumerable<TSource> source, int count)
		{
			source.ThrowIfArgumentNull(nameof(source));
			count.ThrowIfArgumentOutOfRange(nameof(count));
			var collection = source as ICollection<TSource>;
			if (collection != null)
			{
				return collection.Count >= count;
			}
			var collection2 = source as ICollection;
			if (collection2 != null)
			{
				return collection2.Count >= count;
			}
			int num = 0;
			checked
			{
				using (var enumerator = source.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						num++;
						if (num >= count)
						{
							return true;
						}
					}
				}
			}
			// when source has 0 elements
			if (num == count)
			{
				return true;
			}
			return false; // < count
		}

		/// <summary>
		/// Returns a new collection with items shuffled in O(n) time.
		/// </summary>
		/// <remarks>
		/// Fisher-Yates shuffle, revised by Knuth
		/// http://stackoverflow.com/questions/1287567/is-using-random-and-orderby-a-good-shuffle-algorithm
		/// </remarks>
		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
		{
			source.ThrowIfArgumentNull(nameof(source));
			rng.ThrowIfArgumentNull(nameof(rng));
			var items = source.ToArray();
			for (int i = items.Length - 1; i >= 0; i--)
			{
				var swapIndex = rng.Next(i + 1);
				yield return items[swapIndex];
				// no need to swap fully as items[swapIndex] is not used later
				items[swapIndex] = items[i];
			}
		}

		/// <summary>
		/// Returns a new collection with items shuffled in O(n) time.
		/// </summary>
		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
		{
			return Shuffle(source, new Random());
		}

		/// <summary>
		/// Slices an array as Python.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <param name="start">index to include.</param>
		/// <param name="end">index to exclude.</param>
		/// <param name="step">step</param>
		/// <returns></returns>
		/// <remarks>
		/// http://docs.python.org/2/tutorial/introduction.html#strings
		///    +---+---+---+---+---+
		///    | H | e | l | p | A |
		///    +---+---+---+---+---+
		///      0   1   2   3   4   5
		/// -6  -5  -4  -3  -2  -1    
		/// 
		/// note:
		/// [1:3] and [-4:-2] give { e, l }
		/// +ve step means traverse forward, -ve step means traverse backward
		/// defaults for +ve step, start = 0, end = 5 (a.Length)
		/// defaults for -ve step, start = -1, end = -6 (-a.Length -1)
		/// </remarks>
		public static IEnumerable<T> Slice<T>(this T[] array,
			int? start = null, int? end = null, int step = 1)
		{
			array.ThrowIfArgumentNull(nameof(array));
			int _start, _end;
			// step
			if (step == 0)
			{
				// handle gracefully
				yield break;
			}
			else if (step > 0)
			{
				// defaults for step > 0
				_start = 0;
				_end = array.Length;
			}
			else // step < 0
			{
				// defaults for step < 0
				_start = -1;
				_end = -array.Length - 1;
			}
			// inputs
			_start = start ?? _start;
			_end = end ?? _end;
			// get positive index for given index
			Func<int, int, int> toPositiveIndex = (int index, int length) =>
			{
				return index >= 0 ? index : index + length;
			};
			// start
			if (_start < -array.Length || _start >= array.Length)
			{
				yield break;
			}
			_start = toPositiveIndex(_start, array.Length);
			// end - check gracefully
			if (_end < -array.Length - 1)
			{
				_end = -array.Length - 1;
			}
			if (_end > array.Length)
			{
				_end = array.Length;
			}
			_end = toPositiveIndex(_end, array.Length);
			// start, end
			if (step > 0 && _start > _end)
			{
				yield break;
			}
			if (step < 0 && _end > _start)
			{
				yield break;
			}
			// slice
			if (step > 0)
			{
				var i = _start;
				while (i < _end)
				{
					yield return array[i];
					i += step;
				}
			}
			if (step < 0)
			{
				var i = _start;
				while (i > _end)
				{
					yield return array[i];
					i += step;
				}
			}
		}

		/// <summary>
		/// Returns a new collection with <paramref name="words"/> scrabbled like the game.
		/// </summary>
		/// <param name="words">Words to scrabble.</param>
		public static IEnumerable<string> Scrabble(this IEnumerable<string> words)
		{
			words.ThrowIfArgumentNull(nameof(words));
			foreach (var items in words.Where(x => !x.IsNullOrEmpty()).Scrabble<string>())
			{
				yield return string.Concat(items);
			}
		}

		/// <summary>
		/// Returns a new collection with <paramref name="words"/> scrabbled like the game
		/// with <paramref name="limit"/>.
		/// </summary>
		/// <param name="words">Words to scrabble.</param>
		/// <param name="limit">Number of <paramref name="words"/> to scrabble from
		/// <paramref name="words"/>.</param>
		public static IEnumerable<string> Scrabble(this IEnumerable<string> words, int limit)
		{
			words.ThrowIfArgumentNull(nameof(words));
			foreach (var items in words.Where(x => !x.IsNullOrEmpty()).Scrabble<string>(limit))
			{
				yield return string.Concat(items);
			}
		}

		/// <summary>
		/// Returns a new collection with <paramref name="source"/> scrabbled like the game.
		/// </summary>
		public static IEnumerable<T[]> Scrabble<T>(this IEnumerable<T> source)
		{
			source.ThrowIfArgumentNull(nameof(source));
			var wordsArray = source.ToArray();
			foreach (var item in
				ScrabbleInner
				(
					wordsArray, wordsArray.Length,
					new bool[wordsArray.Length], new T[wordsArray.Length], 0
				))
			{
				yield return item;
			}
		}

		/// <summary>
		/// Returns a new collection with <paramref name="source"/> scrabbled like the game
		/// with <paramref name="limit"/>.
		/// </summary>
		/// <param name="source">Words to scrabble.</param>
		/// <param name="limit">Number of <paramref name="source"/> to scrabble from
		/// <paramref name="source"/>.</param>
		public static IEnumerable<T[]> Scrabble<T>(this IEnumerable<T> source, int limit)
		{
			source.ThrowIfArgumentNull(nameof(source));
			var wordsArray = source.ToArray();
			limit.ThrowIfArgumentOutOfRange(nameof(limit), maxRange: wordsArray.Length);
			foreach (var item in
				ScrabbleInner
				(
					wordsArray, limit,
					new bool[wordsArray.Length], new T[limit], 0
				))
			{
				yield return item;
			}
		}

		/// <summary>
		/// Scrabbles recursively.
		/// </summary>
		/// <param name="items">Words to scrabble.</param>
		/// <param name="limit">Number of <paramref name="items"/> to scrabble from 
		/// <paramref name="items"/>.</param>
		/// <param name="used">Bool array to determine already used word in
		/// <paramref name="items"/>.</param>
		/// <param name="buffer">Buffer to hold the words.</param>
		/// <param name="depth">Call depth to determine when to return.</param>
		/// <remarks>Similar to the permute method.</remarks>
		private static IEnumerable<T[]> ScrabbleInner<T>(T[] items, int limit,
			bool[] used, T[] buffer, int depth)
		{
			// yield here
			if (depth > 0)
			{
				yield return buffer.Slice(end: depth).ToArray();
			}
			if (depth == limit)
			{
				yield break;
			}
			for (int i = 0; i < items.Length; i++)
			{
				if (used[i])
				{
					continue;
				}
				used[i] = true;
				buffer[depth] = items[i];
				foreach (var item in
					ScrabbleInner(items, limit, used, buffer, depth + 1))
				{
					yield return item;
				}
				buffer[depth] = default(T);
				used[i] = false;
			}
		}

		/// <summary>
		/// Converts a collection to HashSet.
		/// </summary>
		public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
		{
			source.ThrowIfArgumentNull(nameof(source));
			return new HashSet<T>(source);
		}

		public static IEnumerable<T[]> Permutation<T>(this IEnumerable<T> source)
		{
			source.ThrowIfArgumentNull(nameof(source));
			return source.Permutation(source.Count());
		}

		public static IEnumerable<T[]> Permutation<T>(this IEnumerable<T> source, int r)
		{
			source.ThrowIfArgumentNull(nameof(source));
			r.ThrowIfArgumentOutOfRange(nameof(r));
			if (!source.HasCountOfAtLeast(r))
			{
				throw new ArgumentOutOfRangeException(nameof(r));
			}
			var input = source.ToArray();
			var buffer = new T[r];
			var used = new bool[input.Length];
			var depth = 0;
			// yield is required
			foreach (var item in Permute(input, r, buffer, used, depth))
			{
				yield return item;
			}
		}

		private static IEnumerable<T[]> Permute<T>(T[] input, int r, T[] buffer,
			bool[] used, int depth)
		{
			if (depth > r)
			{
				yield break;
			}
			if (depth == r)
			{
				yield return (T[])buffer.Clone();
				yield break;
			}
			for (int i = 0; i < input.Length; i++)
			{
				if (used[i])
				{
					continue;
				}
				used[i] = true;
				buffer[depth] = input[i];
				foreach (var item in Permute(input, r, buffer, used, depth + 1))
				{
					yield return item;
				}
				buffer[depth] = default(T);
				used[i] = false;
			}
		}

		public static IEnumerable<T[]> Combination<T>(this IEnumerable<T> source)
		{
			source.ThrowIfArgumentNull(nameof(source));
			return source.Combination(source.Count());
		}

		public static IEnumerable<T[]> Combination<T>(this IEnumerable<T> source, int r)
		{
			source.ThrowIfArgumentNull(nameof(source));
			if (!source.HasCountOfAtLeast(r))
			{
				throw new ArgumentOutOfRangeException(nameof(r));
			}
			var input = source.ToArray();
			var buffer = new T[r];
			var depth = 0;
			var start = 0;
			// yield is required
			foreach (var item in Combine(input, r, buffer, depth, start))
			{
				yield return item;
			}
		}

		private static IEnumerable<T[]> Combine<T>(T[] input, int r, T[] buffer,
			int depth, int start)
		{
			if (depth > r)
			{
				yield break;
			}
			if (depth == r)
			{
				yield return (T[])buffer.Clone();
				yield break;
			}
			for (int i = start; i < input.Length; i++)
			{
				buffer[depth] = input[i];
				foreach (var item in Combine(input, r, buffer, depth + 1, i + 1))
				{
					yield return item;
				}
				buffer[depth] = default(T);
			}
		}

		public static bool IsEmpty<T>(this IEnumerable<T> source)
		{
			source.ThrowIfArgumentNull(nameof(source));
			return !source.Any();
		}

		public static bool IsEmpty<T>(this IEnumerable<T> source, Func<T, bool> predicate)
		{
			source.ThrowIfArgumentNull(nameof(source));
			predicate.ThrowIfArgumentNull(nameof(predicate));
			return !source.Any(predicate);
		}

		/// <summary>
		/// Returns top n efficiently.
		/// </summary>
		public static IEnumerable<T> Top<T>(this IEnumerable<T> source, int n)
			where T : IComparable<T>
		{
			return Top(source, n, x => x, Comparer<T>.Default);
		}

		/// <summary>
		/// Returns top n efficiently.
		/// </summary>
		public static IEnumerable<T> Top<T>(this IEnumerable<T> source, int n,
			IComparer<T> comparer)
			where T : IComparable<T>
		{
			return Top(source, n, x => x, comparer);
		}

		/// <summary>
		/// Returns top n efficiently.
		/// </summary>
		public static IEnumerable<T> Top<T, TKey>(this IEnumerable<T> source, int n,
			Func<T, TKey> keySelector)
			where TKey : IComparable<TKey>
		{
			return Top(source, n, keySelector, Comparer<TKey>.Default);
		}

		/// <summary>
		/// Returns top n efficiently.
		/// </summary>
		/// <remarks>Uses min-heap, O(elements * logn) time, O(n) space.</remarks>
		public static IEnumerable<T> Top<T, TKey>(this IEnumerable<T> source, int n,
			Func<T, TKey> keySelector, IComparer<TKey> comparer)
			where TKey : IComparable<TKey>
		{
			source.ThrowIfArgumentNull(nameof(source));
			n.ThrowIfArgumentOutOfRange(nameof(n));
			keySelector.ThrowIfArgumentNull(nameof(keySelector));
			comparer.ThrowIfArgumentNull(nameof(comparer));
			var minheap = new MinHeap<T, TKey>(n, keySelector, comparer);
			if (n == 0)
			{
				return minheap;
			}
			foreach (var x in source)
			{
				if (x == null)
				{
					continue;
				}
				if (minheap.IsFull())
				{
					if (comparer.Compare(keySelector(x), keySelector(minheap.Peek())) > 0) //x > heap[0]
					{
						minheap.Displace(x);
					}
				}
				else
				{
					minheap.Append(x);
				}
			}
			return minheap;
		}

		/// <summary>
		/// Returns bottom n efficiently.
		/// </summary>
		public static IEnumerable<T> Bottom<T>(this IEnumerable<T> source, int n)
			where T : IComparable<T>
		{
			return Bottom(source, n, x => x, Comparer<T>.Default);
		}

		/// <summary>
		/// Returns bottom n efficiently.
		/// </summary>
		public static IEnumerable<T> Bottom<T>(this IEnumerable<T> source, int n,
			IComparer<T> comparer)
			where T : IComparable<T>
		{
			return Bottom(source, n, x => x, comparer);
		}

		/// <summary>
		/// Returns bottom n efficiently.
		/// </summary>
		public static IEnumerable<T> Bottom<T, TKey>(this IEnumerable<T> source, int n,
			Func<T, TKey> keySelector)
			where TKey : IComparable<TKey>
		{
			return Bottom(source, n, keySelector, Comparer<TKey>.Default);
		}

		/// <summary>
		/// Returns bottom n efficiently.
		/// </summary>
		/// <remarks>Uses max-heap, O(elements * logn) time, O(n) space.</remarks>
		public static IEnumerable<T> Bottom<T, TKey>(this IEnumerable<T> source, int n,
			Func<T, TKey> keySelector, IComparer<TKey> comparer)
			where TKey : IComparable<TKey>
		{
			source.ThrowIfArgumentNull(nameof(source));
			n.ThrowIfArgumentOutOfRange(nameof(n));
			keySelector.ThrowIfArgumentNull(nameof(keySelector));
			comparer.ThrowIfArgumentNull(nameof(comparer));
			var maxheap = new MaxHeap<T, TKey>(n, keySelector, comparer);
			if (n == 0)
			{
				return maxheap;
			}
			foreach (var x in source)
			{
				if (x == null)
				{
					continue;
				}
				if (maxheap.IsFull())
				{
					if (comparer.Compare(keySelector(x), keySelector(maxheap.Peek())) < 0) //x < heap[0]
					{
						maxheap.Displace(x);
					}
				}
				else
				{
					maxheap.Append(x);
				}
			}
			return maxheap;
		}

		/// <summary>
		/// Returns source.Except(second, comparer) in a linqified way.
		/// </summary>
		public static IEnumerable<T> ExceptBy<T, TKey>(this IEnumerable<T> source, IEnumerable<T> second,
			Func<T, TKey> keySelector)
		{
			source.ThrowIfArgumentNull(nameof(source));
			second.ThrowIfArgumentNull(nameof(second));
			keySelector.ThrowIfArgumentNull(nameof(keySelector));
			return source.Except(second,
				// calls new GenericEqualityComparer<T, TKey>(keySelector)
				GenericEqualityComparer<T>.By(keySelector)
				);
		}

		/// <summary>
		/// Returns source.Distinct(comparer) in a linqified way.
		/// </summary>
		public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source,
			Func<T, TKey> keySelector)
		{
			source.ThrowIfArgumentNull(nameof(source));
			keySelector.ThrowIfArgumentNull(nameof(keySelector));
			return source.Distinct(
				// calls new GenericEqualityComparer<T, TKey>(keySelector)
				GenericEqualityComparer<T>.By(keySelector)
				);
		}

		/// <summary>
		/// Returns empty if enumerable is null else same enumerable.
		/// </summary>
		public static IEnumerable<T> OrEmpty<T>(this IEnumerable<T> source)
		{
			return source ?? Enumerable.Empty<T>();
		}

		/// <summary>
		/// Returns source.OrderBy(keySelector, comparer) in a linqified way.
		/// </summary>
		public static IOrderedEnumerable<T> OrderBy<T, TKey>(this IEnumerable<T> source,
			Func<T, TKey> keySelector, Func<TKey, TKey, int> compare)
		{
			source.ThrowIfArgumentNull(nameof(source));
			keySelector.ThrowIfArgumentNull(nameof(keySelector));
			compare.ThrowIfArgumentNull(nameof(compare));
			return source.OrderBy(keySelector, new GenericComparer<TKey>(compare));
		}

		/// <summary>
		/// Returns source.OrderByDescending(keySelector, comparer) in a linqified way.
		/// </summary>
		public static IOrderedEnumerable<T> OrderByDescending<T, TKey>(this IEnumerable<T> source,
			Func<T, TKey> keySelector, Func<TKey, TKey, int> compare)
		{
			source.ThrowIfArgumentNull(nameof(source));
			keySelector.ThrowIfArgumentNull(nameof(keySelector));
			compare.ThrowIfArgumentNull(nameof(compare));
			return source.OrderByDescending(keySelector, new GenericComparer<TKey>(compare));
		}

		/// <summary>
		/// Implements <see cref="Enumerable.Single{TSource}(IEnumerable{TSource})"/> without exception.
		/// </summary>
		public static bool TrySingle<T>(this IEnumerable<T> source, out T singleT)
		{
			source.ThrowIfArgumentNull(nameof(source));
			if (source.HasCount(1))
			{
				singleT = source.Single();
				return true;
			}
			singleT = default(T);
			return false;
		}

		/// <summary>
		/// Implements <see cref="Enumerable.Single{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>
		/// without exception.
		/// </summary>
		public static bool TrySingle<T>(this IEnumerable<T> source, Func<T, bool> predicate, out T singleT)
		{
			source.ThrowIfArgumentNull(nameof(source));
			predicate.ThrowIfArgumentNull(nameof(predicate));
			if (source.HasCount(predicate, 1))
			{
				singleT = source.Single(predicate);
				return true;
			}
			singleT = default(T);
			return false;
		}

		/// <summary>
		/// Returns bigint count of sequence.
		/// </summary>
		public static BigInteger BigCount<T>(this IEnumerable<T> source)
		{
			source.ThrowIfArgumentNull(nameof(source));
			BigInteger count = 0;
			foreach (var item in source)
			{
				count++;
			}
			return count;
		}
	}
}

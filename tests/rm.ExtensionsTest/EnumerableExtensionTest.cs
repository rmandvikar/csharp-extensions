using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using rm.Extensions;
using rm.ExtensionsTest.Sample;
using rm.Random2;

namespace rm.ExtensionsTest
{
	[TestFixture]
	public class EnumerableExtensionTest
	{
		private readonly Random random = RandomFactory.GetThreadStaticRandom();

		private void Print(IEnumerable<IEnumerable<int>> itemslist)
		{
			Console.WriteLine("chunks={0}", itemslist.Count());
			foreach (var items in itemslist)
			{
				Console.Write("chunk: ");
				foreach (var item in items)
				{
					Console.Write("{0}, ", item);
				}
				Console.WriteLine();
			}
		}

		private void Print<T>(T[] a)
		{
			Console.WriteLine(string.Join(", ", a));
		}

		[Test]
		public void Chunk_bad2_01()
		{
			var source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
			var chunks = source.Chunk(3);
			var i = 0;
			foreach (var chunk in chunks)
			{
				foreach (var item in chunk)
				{
					Assert.AreEqual(source[i], item);
					i++;
				}
			}
		}

		[Test]
		public void Chunk01()
		{
			var chunks = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }.Chunk(3);
			//Print(chunks);
			Assert.AreEqual(4, chunks.Count());
			Assert.AreEqual(10, chunks.ElementAt(3).First());
			Assert.AreEqual(1, chunks.ElementAt(0).First());
			Assert.AreEqual(4, chunks.ElementAt(1).First());
			Assert.AreEqual(7, chunks.ElementAt(2).First());
		}

		[Test]
		public void Chunk02()
		{
			var chunks = new int[] { 3 }.Chunk(10);
			//Print(chunks);
			Assert.AreEqual(1, chunks.Count());
			Assert.AreEqual(3, chunks.ElementAt(0).First());
		}

		[Test]
		public void Chunk03()
		{
			var chunks = new int[] { 1, 2, 3, 4, 5 }.Chunk(1);
			var chunks_rev = chunks.Reverse().ToList();
			//Print(chunks);
			Assert.AreEqual(5, chunks_rev.Count());
			for (int i = 0; i < chunks_rev.Count(); i++)
			{
				Assert.AreEqual(chunks_rev.Count() - i, chunks_rev.ElementAt(i).ElementAt(0));
			}
		}

		[Test]
		public void IsNullOrEmpty01()
		{
			Assert.IsFalse(new[] { 3 }.IsNullOrEmpty());
			Assert.IsTrue(new int[0].IsNullOrEmpty());
			Assert.IsTrue(((IEnumerable<int>)null).IsNullOrEmpty());
		}

		[Test]
		public void Split01()
		{
			int n = 3;
			var splits = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }.Split(n);
			//Print(splits);
			Assert.AreEqual(n, splits.Count());
			Assert.IsTrue(splits.ElementAt(0).SequenceEqual(new[] { 1, 4, 7, 10 }));
			Assert.IsTrue(splits.ElementAt(2).SequenceEqual(new[] { 3, 6, 9 }));
		}

		[Test]
		public void IsSorted01()
		{
			Assert.Throws<ArgumentNullException>(() => { ((IEnumerable<int>)null).IsSorted(); });
			var sourceAsc = new[] { 1, 5, 10 };
			Assert.IsTrue(sourceAsc.IsSorted());
			var sourceDesc = new[] { 1, 5, 10 }.Reverse();
			Assert.IsTrue(sourceDesc.IsSorted());
			var sourceUnsorted = new[] { 1, 3, 2 };
			Assert.IsFalse(sourceUnsorted.IsSorted());
		}

		[Test]
		public void IsSorted02()
		{
			var source1 = new[] { 1 };
			Assert.IsTrue(source1.IsSorted());
			var source2Asc = new[] { 1, 5 };
			Assert.IsTrue(source2Asc.IsSorted());
			var source2Desc = new[] { 1, 5 }.Reverse();
			Assert.IsTrue(source2Desc.IsSorted());
			var sourceAllSame = new[] { 5, 5, 5 };
			Assert.IsTrue(sourceAllSame.IsSorted());
		}

		[Test]
		public void Double01()
		{
			Assert.AreEqual(2, new[] { 1, 2 }.Double().Count());
			Assert.Throws<InvalidOperationException>(() => { new int[0].Double(); });

			Assert.Throws<ArgumentNullException>(() => { ((IEnumerable<int>)null).Double(); });
			Assert.Throws<InvalidOperationException>(() => { new[] { 1 }.Double(); });
			Assert.Throws<InvalidOperationException>(() => { new[] { 1, 2, 3 }.Double(); });
		}

		[Test]
		public void Double02()
		{
			Assert.AreEqual(2, new[] { 0, 1, 2, 3 }.Double(x => x > 1).Count());
			Assert.Throws<InvalidOperationException>(() => { new int[0].Double(x => x > 0); });

			Assert.Throws<ArgumentNullException>(() => { ((IEnumerable<int>)null).Double(x => x > 0); });
			Assert.Throws<InvalidOperationException>(() => { new[] { 1 }.Double(x => x > 0); });
			Assert.Throws<InvalidOperationException>(() => { new[] { 1, 2, 3 }.Double(x => x > 0); });
		}

		[Test]
		public void DoubleOrDefault01()
		{
			Assert.AreEqual(2, new[] { 1, 2 }.DoubleOrDefault().Count());
			Assert.AreEqual(null, new int[0].DoubleOrDefault());

			Assert.Throws<ArgumentNullException>(() => { ((IEnumerable<int>)null).DoubleOrDefault(); });
			Assert.Throws<InvalidOperationException>(() => { new[] { 1 }.DoubleOrDefault(); });
			Assert.Throws<InvalidOperationException>(() => { new[] { 1, 2, 3 }.DoubleOrDefault(); });
		}

		[Test]
		public void DoubleOrDefault02()
		{
			Assert.AreEqual(2, new[] { 0, 1, 2, 3 }.DoubleOrDefault(x => x > 1).Count());
			Assert.AreEqual(null, new int[0].DoubleOrDefault(x => x > 1));

			Assert.Throws<ArgumentNullException>(() => { ((IEnumerable<int>)null).DoubleOrDefault(x => x > 0); });
			Assert.Throws<InvalidOperationException>(() => { new[] { 1 }.DoubleOrDefault(x => x > 0); });
			Assert.Throws<InvalidOperationException>(() => { new[] { 1, 2, 3 }.DoubleOrDefault(x => x > 0); });
		}

		[Test]
		public void Shuffle01()
		{
			var items = Enumerable.Range(0, 5).ToArray();
			var itemsStr = string.Join(",", items);
			var tries = 2;
			var count = 0;
			for (int i = 0; i < tries; i++)
			{
				var shuffle = items.Shuffle(random).ToArray();
				var shuffleStr = string.Join(",", shuffle);
				Console.WriteLine(itemsStr);
				Console.WriteLine(shuffleStr);
				if (items.SequenceEqual(shuffle))
				{
					count++;
				}
				Console.WriteLine("--");
			}
			Assert.Less(count, tries);
		}

		[Test]
		public void Slice00_help()
		{
			var a = "12345".ToCharArray();
			// 1st 2
			Assert.AreEqual(a.Slice(end: 2).ToArray(), new[] { '1', '2' });
			// except 1st 2
			Assert.AreEqual(a.Slice(2).ToArray(), new[] { '3', '4', '5' });
			// last 2
			Assert.AreEqual(a.Slice(-2).ToArray(), new[] { '4', '5' });
			// except last 2
			Assert.AreEqual(a.Slice(0, -2).ToArray(), new[] { '1', '2', '3' });
			// 2nd char
			Assert.AreEqual(a.Slice(1, 1 + 1).ToArray(), new[] { '2' });
			// 2nd last char
			Assert.AreEqual(a.Slice(-2, -2 + 1).ToArray(), new[] { '4' });
		}

		[Test]
		// normal cases
		[TestCase(3, 5, 1, 3, 4)]
		[TestCase(0, 5, 1, 0, 4)]
		[TestCase(3, null, 1, 3, 9)]
		[TestCase(0, null, 1, 0, 9)]
		[TestCase(null, null, 1, 0, 9)]
		[TestCase(0, 10, 1, 0, 9)]
		[TestCase(0, int.MaxValue, 1, 0, 9)]
		[TestCase(-1, null, 1, 9, 9)]
		[TestCase(-2, null, 1, 8, 9)]
		[TestCase(0, -2, 1, 0, 7)]
		// corner cases
		[TestCase(0, 0, 1, null, null)]
		[TestCase(3, 5, 2, 3, 3)]
		[TestCase(3, 6, 2, 3, 5)]
		[TestCase(100, int.MaxValue, 1, null, null)]
		[TestCase(int.MaxValue, 1, 1, null, null)]
		[TestCase(-11, int.MaxValue, 1, null, null)]
		[TestCase(-6, -5, 1, 4, 4)]
		[TestCase(-5, -6, 1, null, null)]
		[TestCase(-5, -5, 1, null, null)]
		[TestCase(0, -10, 1, null, null)]
		[TestCase(0, -11, 1, null, null)]
		[TestCase(null, null, 100, 0, 0)]
		// -ve step
		[TestCase(null, null, -1, 9, 0)]
		[TestCase(null, int.MinValue, -1, 9, 0)]
		[TestCase(-1, int.MinValue, -1, 9, 0)]
		[TestCase(-7, -5, -1, null, null)]
		[TestCase(-5, -7, -1, 5, 4)]
		[TestCase(-5, -7, -2, 5, 5)]
		[TestCase(-7, null, -1, 3, 0)]
		public void Slice01(int? s, int? e, int i, int? first, int? last)
		{
			var a = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			var slice = a.Slice(start: s, end: e, step: i).ToArray();
			Print(slice);
			if (first.HasValue)
			{
				Assert.AreEqual(first, slice.First());
			}
			if (last.HasValue)
			{
				Assert.AreEqual(last, slice.Last());
			}
		}

		[Test]
		public void Slice02()
		{
			var a = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			var slice1 = a.Slice().ToArray();
			Print(slice1);
			Assert.AreEqual(a.First(), slice1.First());
			Assert.AreEqual(a.Last(), slice1.Last());
			var slice2 = a.Slice(step: -1).ToArray();
			Print(slice2);
			Assert.AreEqual(a.First(), slice2.Last());
			Assert.AreEqual(a.Last(), slice2.First());
		}

		[Test]
		[TestCase(null, null, 1)]
		public void Slice03(int? s, int? e, int i)
		{
			var a = new int[] { };
			var slice = a.Slice(s, e, i).ToArray();
			Print(slice);
			Assert.AreEqual(0, slice.Count());
		}

		[Test]
		[TestCase(null, null, 1, 1)]
		public void Slice04(int? s, int? e, int i, int count)
		{
			var a = new int[] { 1 };
			var slice = a.Slice(s, e, i).ToArray();
			Print(slice);
			Assert.AreEqual(count, slice.Count());
		}

		[Test]
		[TestCase("this", "is", "a", "test")]
		[TestCase("this", "test", "")]
		[TestCase("")]
		public void Scrabble01(params string[] words)
		{
			var result = words.Scrabble();
			foreach (var item in result)
			{
				Console.WriteLine(item);
			}
			var scrabbleCount = (int)words.Where(x => !x.IsNullOrEmpty()).Count().ScrabbleCount();
			var actualCount = result.Count();
			Console.WriteLine("scrabble count:{0}", scrabbleCount);
			Console.WriteLine("result count:{0}", actualCount);
			Assert.AreEqual(actualCount, result.Distinct().Count());
			Assert.AreEqual(scrabbleCount, actualCount);
		}

		private IEnumerable<int> GetEnumerable(int start, int end)
		{
			for (int i = start; i <= end; i++)
			{
				yield return i;
			}
		}

		[Test]
		public void HasCount01()
		{
			Assert.True(new[] { 1, 2 }.HasCount(2));
			Assert.False(new[] { 1 }.HasCount(2));
			Assert.False(Enumerable.Range(1, 1000000000).HasCount(2));
			Assert.True(new int[0].HasCount(0));
		}

		[Test]
		public void HasCount02()
		{
			Assert.True(GetEnumerable(1, 2).HasCount(2));
			Assert.False(GetEnumerable(1, 1).HasCount(2));
			Assert.False(GetEnumerable(1, 1000000000).HasCount(2));
			Assert.True(GetEnumerable(1, 0).HasCount(0));
		}

		[Test]
		public void HasCountPredicate01()
		{
			Assert.True(new[] { -1, 0, 1, 2 }.HasCount(x => x > 0, 2));
			Assert.False(new[] { -1, 0, 1 }.HasCount(x => x > 0, 2));
			Assert.False(Enumerable.Range(1, 1000000000).HasCount(x => x > 5, 2));
			Assert.True(new int[0].HasCount(x => x > 0, 0));
		}

		[Test]
		public void HasCountPredicate02()
		{
			Assert.True(GetEnumerable(-1, 2).HasCount(x => x > 0, 2));
			Assert.False(GetEnumerable(-1, 1).HasCount(x => x > 0, 2));
			Assert.False(GetEnumerable(1, 1000000000).HasCount(x => x > 5, 2));
			Assert.True(GetEnumerable(1, 0).HasCount(x => x > 5, 0));
		}

		[Test]
		public void HasCountOfAtLeast01()
		{
			Assert.True(new[] { 1, 2 }.HasCountOfAtLeast(2));
			Assert.False(new[] { 1 }.HasCountOfAtLeast(2));
			Assert.True(Enumerable.Range(1, 1000000000).HasCountOfAtLeast(2));
			Assert.True(new int[0].HasCountOfAtLeast(0));
		}

		[Test]
		public void HasCountOfAtLeast02()
		{
			Assert.True(GetEnumerable(1, 2).HasCountOfAtLeast(2));
			Assert.False(GetEnumerable(1, 1).HasCountOfAtLeast(2));
			Assert.True(GetEnumerable(1, 1000000000).HasCountOfAtLeast(2));
			Assert.True(GetEnumerable(1, 0).HasCountOfAtLeast(0));
		}

		[Test]
		[Category("slow")]
		public void HasCountX01()
		{
			var ts = DateTime.UtcNow;
			Assert.False(Enumerable.Range(1, 1000000).HasCount(2));
			Console.WriteLine("HasCount done in {0}.", DateTime.UtcNow.Subtract(ts).Round());
			ts = DateTime.UtcNow;
			Assert.True(Enumerable.Range(1, 1000000).HasCountOfAtLeast(2));
			Console.WriteLine("HasCountOfAtLeast done in {0}.", DateTime.UtcNow.Subtract(ts).Round());
			ts = DateTime.UtcNow;
			Assert.True(Enumerable.Range(1, 1000000).Count() > 2);
			Console.WriteLine("Count done in {0}.", DateTime.UtcNow.Subtract(ts).Round());
		}

		[Test]
		[TestCase(4, 2, 12)]
		[TestCase(4, 1, 4)]
		[TestCase(4, 4, 24)]
		[TestCase(2, 2, 2)]
		public void Permutation01(int n, int r, int count)
		{
			var input = Enumerable.Range(1, n);
			var result = input.Permutation(r);
			Assert.AreEqual(count, result.Count());
		}

		[Test]
		[TestCase(3, 2, 3)]
		[TestCase(3, 1, 3)]
		[TestCase(3, 3, 1)]
		[TestCase(2, 2, 1)]
		public void Combination01(int n, int r, int count)
		{
			var input = Enumerable.Range(1, n);
			var result = input.Combination(r);
			Assert.AreEqual(count, result.Count());
		}

		[Test]
		[TestCase(new int[] { }, true)]
		[TestCase(new int[] { 1 }, false)]
		public void IsEmpty01(int[] source, bool result)
		{
			Assert.AreEqual(result, source.IsEmpty());
		}

		[Test]
		public void IsEmpty02()
		{
			Assert.IsFalse(GetEnumerable(1, 10000000).IsEmpty());
			Assert.IsFalse(GetEnumerable(1, 10000000).IsEmpty(x => x == 10000000));
			Assert.IsTrue(GetEnumerable(1, 10000000).IsEmpty(x => x > 10000000));
			Assert.Throws<ArgumentNullException>(() => ((int[])null).IsEmpty());
		}

		[Test]
		[TestCase(3, 1, 4, 6, 7, 9)]
		[TestCase(3, 9, 1, 4, 6, 7)]
		[TestCase(3, 1, 1, 1, 1, 1)]
		[TestCase(3, -9, -1, -4, -6, -7)]
		[TestCase(3, 1)]
		[TestCase(3, 1, 2, 3)]
		[TestCase(5, -9, -8, -7, -6, -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9)]
		[TestCase(0, 1, 2, 3)]
		public void Top01(int n, params int[] args)
		{
			var result = args.Top(n).ToArray();
			Assert.AreEqual(args.Length < n ? args.Length : n, result.Length);
			var resultCsv = string.Join(",", result);
			var argsCsv = string.Join(",", args);
			args = args.OrderByDescending(x => x).ToArray();
			result = result.OrderByDescending(x => x).ToArray();
			var sortresult = args.Take(n).ToArray();
			var sortresultCsv = string.Join(",", args.Take(n).ToArray());
			Console.WriteLine(argsCsv);
			Console.WriteLine(resultCsv);
			Console.WriteLine(sortresultCsv);
			Console.WriteLine(string.Join(",", args));
			Assert.True(sortresult.SequenceEqual(result));
		}

		[Test]
		[TestCase(3, 1, 4, 6, 7, 9)]
		[TestCase(3, 9, 1, 4, 6, 7)]
		[TestCase(3, 1, 1, 1, 1, 1)]
		[TestCase(3, -9, -1, -4, -6, -7)]
		[TestCase(3, 1)]
		[TestCase(3, 1, 2, 3)]
		[TestCase(5, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0, -1, -2, -3, -4, -5, -6, -7, -8, -9)]
		[TestCase(0, 1, 2, 3)]
		public void Bottom01(int n, params int[] args)
		{
			var result = args.Bottom(n).ToArray();
			Assert.AreEqual(args.Length < n ? args.Length : n, result.Length);
			var resultCsv = string.Join(",", result);
			var argsCsv = string.Join(",", args);
			args = args.OrderBy(x => x).ToArray();
			result = result.OrderBy(x => x).ToArray();
			var sortresult = args.Take(n).ToArray();
			var sortresultCsv = string.Join(",", sortresult);
			Console.WriteLine(argsCsv);
			Console.WriteLine(resultCsv);
			Console.WriteLine(sortresultCsv);
			Console.WriteLine(string.Join(",", args));
			Assert.True(sortresult.SequenceEqual(result));
		}

		[Test]
		public void Top02()
		{
			Assert.DoesNotThrow(() =>
			{
				var heap1 = new[] { (ComparableClass)null }.Top(3).ToArray();
				Assert.AreEqual(0, heap1.Length);
				var heap2 = new[] { 1 }.Top(0).ToArray();
				Assert.AreEqual(0, heap2.Length);
			});
		}

		[Test]
		public void Bottom02()
		{
			Assert.DoesNotThrow(() =>
			{
				var heap1 = new[] { (ComparableClass)null }.Bottom(3).ToArray();
				Assert.AreEqual(0, heap1.Length);
				var heap2 = new[] { 1 }.Bottom(0).ToArray();
				Assert.AreEqual(0, heap2.Length);
			});
		}

		[Test]
		public void Top03()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => { new[] { 1 }.Top(-1); });
		}

		[Test]
		public void Bottom03()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => { new[] { 1 }.Bottom(-1); });
		}

		[Test]
		public void Top_keyselector01()
		{
			Assert.True(new[] { 0, 5, 10, 15, 20, 25 }.Select(x => new ComparableClass2 { Value = x })
				.Top(3, x => x.Value)
				.OrderByDescending(x => x.Value)
				.SequenceEqual(new[] { 25, 20, 15 }.Select(x => new ComparableClass2 { Value = x }),
					GenericEqualityComparer<ComparableClass2>.By(x => x.Value)));
		}

		[Test]
		public void Bottom_keyselector01()
		{
			Assert.True(new[] { 25, 20, 15, 10, 5, 0 }.Select(x => new ComparableClass2 { Value = x })
				.Bottom(3, x => x.Value)
				.OrderBy(x => x.Value)
				.SequenceEqual(new[] { 0, 5, 10 }.Select(x => new ComparableClass2 { Value = x }),
					GenericEqualityComparer<ComparableClass2>.By(x => x.Value)));
		}

		[Test]
		public void Top_keyselector02()
		{
			Assert.True(new[] { 25, 20, 15, 10, 5, 0 }.Select(x => new SampleClass { ComparableClass2 = new ComparableClass2 { Value = x } })
				.Top(3, x => x.ComparableClass2)
				.OrderByDescending(x => x.ComparableClass2)
				.SequenceEqual(new[] { 25, 20, 15 }.Select(x => new SampleClass { ComparableClass2 = new ComparableClass2 { Value = x } }),
					GenericEqualityComparer<SampleClass>.By(x => x.ComparableClass2)));
		}

		[Test]
		public void Bottom_keyselector02()
		{
			Assert.True(new[] { 25, 20, 15, 10, 5, 0 }.Select(x => new SampleClass { ComparableClass2 = new ComparableClass2 { Value = x } })
				.Bottom(3, x => x.ComparableClass2)
				.OrderBy(x => x.ComparableClass2)
				.SequenceEqual(new[] { 0, 5, 10 }.Select(x => new SampleClass { ComparableClass2 = new ComparableClass2 { Value = x } }),
					GenericEqualityComparer<SampleClass>.By(x => x.ComparableClass2)));
		}

		[Test]
		public void Top_comparer01()
		{
			var comparer = new ComparableClass2Comparer();
			Assert.True(new[] { 25, 20, 15, 10, 5, 0 }.Select(x => new ComparableClass2 { Value = x })
				.Top(3, comparer)
				.OrderByDescending(x => x, comparer)
				.SequenceEqual(new[] { 25, 20, 15 }.Select(x => new ComparableClass2 { Value = x }),
					GenericEqualityComparer<ComparableClass2>.By(x => x)));
		}

		[Test]
		public void Bottom_comparer01()
		{
			var comparer = new ComparableClass2Comparer();
			Assert.True(new[] { 25, 20, 15, 10, 5, 0 }.Select(x => new ComparableClass2 { Value = x })
				.Bottom(3, comparer)
				.OrderBy(x => x, comparer)
				.SequenceEqual(new[] { 0, 5, 10 }.Select(x => new ComparableClass2 { Value = x }),
					GenericEqualityComparer<ComparableClass2>.By(x => x)));
		}

		[Test]
		public void Top_keyselector_comparer01()
		{
			var comparer = new ComparableClass2Comparer();
			Assert.True(new[] { 25, 20, 15, 10, 5, 0 }.Select(x => new SampleClass { ComparableClass2 = new ComparableClass2 { Value = x } })
				.Top(3, x => x.ComparableClass2, comparer)
				.OrderByDescending(x => x.ComparableClass2, comparer)
				.SequenceEqual(new[] { 25, 20, 15 }.Select(x => new SampleClass { ComparableClass2 = new ComparableClass2 { Value = x } }),
					GenericEqualityComparer<SampleClass>.By(x => x.ComparableClass2)));
		}

		[Test]
		public void Bottom_keyselector_comparer01()
		{
			var comparer = new ComparableClass2Comparer();
			Assert.True(new[] { 25, 20, 15, 10, 5, 0 }.Select(x => new SampleClass { ComparableClass2 = new ComparableClass2 { Value = x } })
				.Bottom(3, x => x.ComparableClass2, comparer)
				.OrderBy(x => x.ComparableClass2, comparer)
				.SequenceEqual(new[] { 0, 5, 10 }.Select(x => new SampleClass { ComparableClass2 = new ComparableClass2 { Value = x } }),
					GenericEqualityComparer<SampleClass>.By(x => x.ComparableClass2)));
		}

		[Test]
		[TestCase(new[] { 1, 2, 3, 4, 5 }, new[] { 1, 2, 3 }, new[] { 4, 5 })]
		[TestCase(new[] { 1, 2, 3, 4, 5 }, new[] { 2, 3 }, new[] { 1, 4, 5 })]
		[TestCase(new[] { 1, 2, 3, 4, 5 }, new[] { 1, 2, 3, 4, 5 }, new int[] { })]
		[TestCase(new[] { 2, 3 }, new[] { 1, 2, 3, 4, 5 }, new int[] { })]
		public void ExceptBy01(int[] a, int[] b, int[] expected)
		{
			var source = a.Select(x => new { prop = x });
			var second = b.Select(x => new { prop = x });
			var except = source.ExceptBy(second, x => x.prop);
			Assert.AreEqual(expected.Count(), except.Count());
			foreach (var item in expected)
			{
				Assert.Contains(item, except.Select(x => x.prop).ToList());
			}
		}

		[Test]
		public void ExceptBy02()
		{
			Assert.Throws<ArgumentNullException>(() =>
				new int?[] { 1, 2 }.ExceptBy(new int?[] { 1 }, (Func<int?, int>)null)
				);
		}

		[Test]
		public void ExceptBy03()
		{
			var except = new int?[] { 1, null }.ExceptBy(new int?[] { null }, x => x);
			Assert.IsTrue(except.Count() == 1);
			Assert.IsTrue(except.Contains(1));
		}

		[Test]
		[TestCase(new[] { 1, 2, 3, 4, 5 }, new[] { 1, 2, 3, 4, 5 })]
		[TestCase(new[] { 1, 1, 2 }, new[] { 1, 2 })]
		public void DistinctBy01(int[] a, int[] expected)
		{
			var source = a.Select(x => new { prop = x });
			var distinct = source.DistinctBy(x => x.prop);
			Assert.AreEqual(expected.Count(), distinct.Count());
			foreach (var item in expected)
			{
				Assert.Contains(item, distinct.Select(x => x.prop).ToList());
			}
		}

		[Test]
		public void DistinctBy02()
		{
			Assert.Throws<ArgumentNullException>(() =>
				new int?[] { 1, 2 }.DistinctBy((Func<int?, int>)null)
				);
		}

		[Test]
		public void DistinctBy03()
		{
			var distinct = new int?[] { 1, null, null }.DistinctBy(x => x);
			Assert.IsTrue(distinct.Count() == 2);
			Assert.IsTrue(distinct.Contains(1));
			Assert.IsTrue(distinct.Contains((int?)null));
		}

		[Test]
		public void OrEmpty01()
		{
			Assert.AreEqual(new[] { 1 }, new[] { 1 }.OrEmpty());
			Assert.AreEqual(new int[0], ((int[])null).OrEmpty());
		}

		[Test]
		public void EmptyIfDefault01()
		{
			Assert.AreEqual(new[] { 1 }, new[] { 1 }.EmptyIfDefault());
			Assert.AreEqual(new int[0], ((int[])null).EmptyIfDefault());
		}

		[Test]
		[TestCase(new[] { 1, 4, 5, 2, 3 }, new[] { 1, 2, 3, 4, 5 }, true)]
		[TestCase(new[] { 1, 1, 2 }, new[] { 1, 1, 2 }, true)]
		[TestCase(new int[] { }, new int[] { }, true)]
		[TestCase(new[] { 1, 5, 4 }, new[] { 1, 5, 4 }, false)]
		[TestCase(new int[] { }, new[] { 1 }, false)]
		public void OrderBy01(int[] a, int[] expected, bool result)
		{
			var source = a.Select(x => new { prop = x.ToString() });
			var order = source.OrderBy(x => x.prop, (prop1, prop2) => prop1.CompareTo(prop2));
			Assert.AreEqual(result, expected.Select(x => new { prop = x.ToString() }).SequenceEqual(order));
		}

		[Test]
		[TestCase(new[] { 1, 4, 5, 2, 3 }, new[] { 5, 4, 3, 2, 1 }, true)]
		[TestCase(new[] { 1, 1, 2 }, new[] { 2, 1, 1 }, true)]
		[TestCase(new int[] { }, new int[] { }, true)]
		[TestCase(new[] { 1, 5, 4 }, new[] { 1, 5, 4 }, false)]
		[TestCase(new int[] { }, new[] { 1 }, false)]
		public void OrderByDescending01(int[] a, int[] expected, bool result)
		{
			var source = a.Select(x => new { prop = x.ToString() });
			var order = source.OrderByDescending(x => x.prop, (prop1, prop2) => prop1.CompareTo(prop2));
			Assert.AreEqual(result, expected.Select(x => new { prop = x.ToString() }).SequenceEqual(order));
		}

		[Test]
		[TestCase(new[] { 1, 2 }, 0, false)]
		[TestCase(new[] { 1 }, 1, true)]
		[TestCase(new int[] { }, 0, false)]
		public void TrySingle01(int[] source, int singleTExpected, bool result)
		{
			int singleT;
			Assert.AreEqual(result, source.TrySingle(out singleT));
			if (result)
			{
				Assert.AreEqual(singleTExpected, singleT);
			}
		}

		[Test]
		[TestCase(new[] { 1, 2, 3 }, 0, false)]
		[TestCase(new[] { 1, 2 }, 2, true)]
		[TestCase(new int[] { }, 0, false)]
		public void TrySinglePredicate01(int[] source, int singleTExpected, bool result)
		{
			int singleT;
			Assert.AreEqual(result, source.TrySingle(x => x > 1, out singleT));
			if (result)
			{
				Assert.AreEqual(singleTExpected, singleT);
			}
		}

		[Test]
		[TestCase(new[] { 1, 2 }, 0)]
		[TestCase(new[] { 1 }, 1)]
		[TestCase(new int[] { }, 0)]
		public void OneOrDefault01(int[] source, int oneTExpected)
		{
			Assert.AreEqual(oneTExpected, source.OneOrDefault());
		}

		[Test]
		[TestCase(new[] { 1, 2 }, 2)]
		[TestCase(new[] { 1 }, 0)]
		[TestCase(new int[] { }, 0)]
		[TestCase(new[] { 1, 2, 3 }, 0)]
		public void OneOrDefaultPredicate01(int[] source, int oneTExpected)
		{
			Assert.AreEqual(oneTExpected, source.OneOrDefault(x => x > 1));
		}

		[Test]
		[TestCase(new[] { 1, 2, 3 }, 1, true)]
		[TestCase(new[] { 1, 2, 3 }, 0, false)]
		public void In01(int[] source, int value, bool result)
		{
			Assert.AreEqual(result, value.In(source));
		}

		[Test]
		public void In_comparer01()
		{
			var value = new ComparableClass2() { Value = 1 };
			var source = new[] { 1, 2, 3 }.Select(x => new ComparableClass2() { Value = x });
			Assert.IsTrue(value.In(source, GenericEqualityComparer<ComparableClass2>.By(x => x.Value)));
		}
	}
}

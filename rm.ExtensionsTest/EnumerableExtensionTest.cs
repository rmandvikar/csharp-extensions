using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest
{
    [TestFixture]
    public class EnumerableExtensionTest
    {
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
                var shuffle = items.Shuffle().ToArray();
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
            Console.WriteLine("scrabble count:{0}", words.Where(x => !x.IsNullOrEmpty()).Count().ScrabbleCount());
            Console.WriteLine("result count:{0}", result.Count());
            Assert.AreEqual(result.Count(), result.Distinct().Count());
            Assert.AreEqual(words.Where(x => !x.IsNullOrEmpty()).Count().ScrabbleCount(), result.Count());
        }
        [Test]
        [TestCase("words", "word", "", null)]
        [TestCase("this", "is", "a", "test")]
        public void ToHashSet01(params string[] words)
        {
            var set = words.ToHashSet();
            Assert.NotNull(set);
            foreach (var word in words)
            {
                Assert.IsTrue(set.Contains(word));
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
    }
}

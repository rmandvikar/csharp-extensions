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
    }
}

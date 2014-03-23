using System;
using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest
{
    [TestFixture]
    public class IntExtensionTest
    {
        [Test]
        [TestCase(3, 6)]
        [TestCase(0, 1)]
        [TestCase(10, 3628800)]
        [TestCase(12, 479001600)]
        public void Factorial01(int n, int result)
        {
            Assert.AreEqual(result, n.Factorial());
        }
        [Test]
        public void Factorial02()
        {
            Assert.Throws<OverflowException>(() => { 13.Factorial(); });
        }
        [Test]
        [TestCase(10, 4, 5040)]
        [TestCase(3, 3, 6)]
        [TestCase(0, 0, 1)]
        public void Permutation01(int n, int r, int result)
        {
            Assert.AreEqual(result, n.Permutation(r));
        }
        [Test]
        [TestCase(10, 4, 210)]
        [TestCase(3, 3, 1)]
        [TestCase(0, 0, 1)]
        public void Combination01(int n, int r, int result)
        {
            Assert.AreEqual(result, n.Combination(r));
        }
        [Test]
        [TestCase(2, 4)]
        [TestCase(4, 64)]
        [TestCase(0, 0)]
        public void ScrabbleCount01(int n, int result)
        {
            Assert.AreEqual(result, n.ScrabbleCount());
        }
    }
}

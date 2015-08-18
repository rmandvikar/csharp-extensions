using System;
using System.Linq;
using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest
{
    [TestFixture]
    public class DictionaryExtensionTest
    {
        [Test]
        [TestCase(new[] { 1, 2 }, 3, 0)]
        [TestCase(new[] { 1, 2 }, 1, 1)]
        [TestCase(new[] { 0, 1, 2 }, 0, 0)]
        public void GetValueOrDefault01(int[] a, int key, int expected)
        {
            var dictionary = a.ToDictionary(x => x);
            Assert.AreEqual(expected, dictionary.GetValueOrDefault(key));
        }
        [Test]
        [TestCase(new[] { 1, 2 }, 3, null)]
        [TestCase(new[] { 1, 2 }, 1, "1")]
        [TestCase(new[] { 0, 1, 2 }, 0, "0")]
        public void GetValueOrDefault02(int[] a, int key, string expected)
        {
            var dictionary = a.ToDictionary(x => x, y => y.ToString());
            Assert.AreEqual(expected, dictionary.GetValueOrDefault(key));
        }
        [Test]
        public void AsReadOnly01()
        {
            var dictionary = new[] { 0, 1, 2 }.ToDictionary(x => x, y => y.ToString()).AsReadOnly();
            Assert.Throws<NotSupportedException>(() => dictionary[5] = "5");
        }
    }
}

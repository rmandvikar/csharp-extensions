using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest
{
    [TestFixture]
    public class HelperTest
    {
        [Test]
        public void Swap01()
        {
            var t1 = 1;
            var t2 = 2;
            Assert.AreNotEqual(t1, t2);
            t1.Swap(ref t1, ref t2);
            Assert.AreNotEqual(1, t1);
            Assert.AreNotEqual(2, t2);
            Assert.AreEqual(2, t1);
            Assert.AreEqual(1, t2);
        }
        [Test]
        public void Swap02()
        {
            var c1 = new object(); var c1copy = c1;
            var c2 = new object(); var c2copy = c2;
            Assert.AreNotEqual(c1, c2);
            Helper.Swap(ref c1, ref c2);
            Assert.AreNotEqual(c1copy, c1);
            Assert.AreNotEqual(c2copy, c2);
            Assert.AreEqual(c2copy, c1);
            Assert.AreEqual(c1copy, c2);
        }
        [Test]
        public void Swap03()
        {
            object c1 = null; var c1copy = c1;
            var c2 = new object(); var c2copy = c2;
            Assert.AreNotEqual(c1, c2);
            Helper.Swap(ref c1, ref c2);
            Assert.AreNotEqual(c1copy, c1);
            Assert.AreNotEqual(c2copy, c2);
            Assert.AreEqual(c2copy, c1);
            Assert.AreEqual(c1copy, c2);
        }
    }
}

using System;
using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest
{
    [TestFixture]
    public class CheckExtensionTest
    {
        [Test]
        [TestCase((object)null)]
        public void NullCheck01(object o)
        {
            Assert.Throws<NullReferenceException>(() => { o.NullCheck(); });
        }
        [Test]
        public void NullCheck02()
        {
            Assert.DoesNotThrow(() => { new object().NullCheck(); });
        }
        [Test]
        [TestCase((object)null, "ex message")]
        public void NullCheck03(object o, string m)
        {
            try
            {
                o.NullCheck(m);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(m, ex.Message);
            }
        }
        [Test]
        public void NullCheck04()
        {
            Assert.Throws<NullReferenceException>(() =>
            {
                new[] { new object(), null }.NullCheck();
            });
        }
        [Test]
        public void NullCheck05()
        {
            Assert.DoesNotThrow(() =>
            {
                new[] { new object(), new object() }.NullCheck();
            });
        }
        [Test]
        [TestCase((object)null)]
        public void NullArgumentCheck01(object o)
        {
            Assert.Throws<ArgumentNullException>(() => { o.NullArgumentCheck(); });
        }
        [Test]
        public void NullArgumentCheck02()
        {
            Assert.DoesNotThrow(() => { new object().NullArgumentCheck(); });
        }

        [Test]
        [TestCase((string)null)]
        public void NullOrEmptyCheck01a(string s)
        {
            Assert.Throws<NullReferenceException>(() => { s.NullOrEmptyCheck(); });
        }
        [Test]
        [TestCase("")]
        public void NullOrEmptyCheck01b(string s)
        {
            Assert.Throws<EmptyException>(() => { s.NullOrEmptyCheck(); });
        }
        [Test]
        [TestCase("s")]
        [TestCase("  ")]
        public void NullOrEmptyCheck02(string s)
        {
            Assert.DoesNotThrow(() => { s.NullOrEmptyCheck(); });
        }
        [Test]
        public void NullOrEmptyCheck03()
        {
            Assert.Throws<NullReferenceException>(() =>
            {
                new[] { "s1", null }.NullOrEmptyCheck();
            });
        }
        [Test]
        [TestCase((string)null)]
        public void NullOrEmptyArgumentCheck01a(string s)
        {
            Assert.Throws<ArgumentNullException>(() => { s.NullOrEmptyArgumentCheck(); });
        }
        [Test]
        [TestCase("")]
        public void NullOrEmptyArgumentCheck01b(string s)
        {
            Assert.Throws<EmptyException>(() => { s.NullOrEmptyArgumentCheck(); });
        }
        [Test]
        [TestCase("s")]
        [TestCase("  ")]
        public void NullOrEmptyArgumentCheck02(string s)
        {
            Assert.DoesNotThrow(() => { s.NullOrEmptyArgumentCheck(); });
        }
        [Test]
        public void NullOrEmptyCheckArgument03()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new[] { "s1", null }.NullOrEmptyArgumentCheck();
            });
        }
    }
}

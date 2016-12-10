using System;
using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest
{
	[TestFixture]
	public class ThrowExtensionTest
	{
		[Test]
		[TestCase((object)null)]
		public void ThrowIfNull01(object o)
		{
			Assert.Throws<NullReferenceException>(() => { o.ThrowIfNull(); });
		}
		[Test]
		public void ThrowIfNull02()
		{
			Assert.DoesNotThrow(() => { new object().ThrowIfNull(); });
		}
		[Test]
		[TestCase((object)null, "ex message")]
		public void ThrowIfNull03(object o, string m)
		{
			try
			{
				o.ThrowIfNull(m);
				Assert.Fail();
			}
			catch (NullReferenceException ex)
			{
				Assert.IsTrue(ex.Message.Contains(m));
			}
			catch (Exception)
			{
				Assert.Fail();
			}
		}
		[Test]
		public void ThrowIfNull04()
		{
			Assert.Throws<NullReferenceException>(() =>
			{
				new[] { new object(), null }.ThrowIfAnyNull();
			});
		}
		[Test]
		public void ThrowIfNull05()
		{
			Assert.DoesNotThrow(() =>
			{
				new[] { new object(), new object() }.ThrowIfAnyNull();
			});
		}
		[Test]
		[TestCase((object)null)]
		public void ThrowIfArgumentNull01(object o)
		{
			Assert.Throws<ArgumentNullException>(() => { o.ThrowIfArgumentNull(); });
		}
		[Test]
		public void ThrowIfArgumentNull02()
		{
			Assert.DoesNotThrow(() => { new object().ThrowIfArgumentNull(); });
		}
		[TestCase((object)null, "ex message")]
		public void ThrowIfArgumentNull03(object o, string m)
		{
			try
			{
				o.ThrowIfArgumentNull(m);
				Assert.Fail();
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.Message.Contains(m));
			}
			catch (Exception)
			{
				Assert.Fail();
			}
		}
		[Test]
		public void ThrowIfArgumentNull04()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				new[] { new object(), null }.ThrowIfAnyArgumentNull();
			});
		}
		[Test]
		public void ThrowIfArgumentNull05()
		{
			Assert.DoesNotThrow(() =>
			{
				new[] { new object(), new object() }.ThrowIfAnyArgumentNull();
			});
		}

		[Test]
		[TestCase((string)null)]
		public void ThrowIfNullOrEmpty01a(string s)
		{
			Assert.Throws<NullReferenceException>(() => { s.ThrowIfNullOrEmpty(); });
		}
		[Test]
		[TestCase("")]
		public void ThrowIfNullOrEmpty01b(string s)
		{
			Assert.Throws<EmptyException>(() => { s.ThrowIfNullOrEmpty(); });
		}
		[Test]
		[TestCase("s")]
		[TestCase("  ")]
		public void ThrowIfNullOrEmpty02(string s)
		{
			Assert.DoesNotThrow(() => { s.ThrowIfNullOrEmpty(); });
		}
		[Test]
		public void ThrowIfNullOrEmpty03()
		{
			Assert.Throws<NullReferenceException>(() =>
			{
				new[] { "s1", null }.ThrowIfNullOrEmpty();
			});
		}
		[Test]
		[TestCase((string)null)]
		public void ThrowIfNullOrEmptyArgument01a(string s)
		{
			Assert.Throws<ArgumentNullException>(() => { s.ThrowIfNullOrEmptyArgument(); });
		}
		[Test]
		[TestCase("")]
		public void ThrowIfNullOrEmptyArgument01b(string s)
		{
			Assert.Throws<EmptyException>(() => { s.ThrowIfNullOrEmptyArgument(); });
		}
		[Test]
		[TestCase("s")]
		[TestCase("  ")]
		public void ThrowIfNullOrEmptyArgument02(string s)
		{
			Assert.DoesNotThrow(() => { s.ThrowIfNullOrEmptyArgument(); });
		}
		[Test]
		public void ThrowIfNullOrEmptyArgument03()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				new[] { "s1", null }.ThrowIfNullOrEmptyArgument();
			});
		}

		[Test]
		[TestCase((string)null)]
		public void ThrowIfNullOrWhiteSpace01a(string s)
		{
			Assert.Throws<NullReferenceException>(() => { s.ThrowIfNullOrWhiteSpace(); });
		}
		[Test]
		[TestCase("")]
		[TestCase("  ")]
		public void ThrowIfNullOrWhiteSpace01b(string s)
		{
			Assert.Throws<EmptyException>(() => { s.ThrowIfNullOrWhiteSpace(); });
		}
		[Test]
		[TestCase("s")]
		public void ThrowIfNullOrWhiteSpace02(string s)
		{
			Assert.DoesNotThrow(() => { s.ThrowIfNullOrWhiteSpace(); });
		}

		[Test]
		[TestCase((string)null)]
		public void ThrowIfNullOrWhiteSpaceArgument01a(string s)
		{
			Assert.Throws<ArgumentNullException>(() => { s.ThrowIfNullOrWhiteSpaceArgument(); });
		}
		[Test]
		[TestCase("")]
		[TestCase("  ")]
		public void ThrowIfNullOrWhiteSpaceArgument01b(string s)
		{
			Assert.Throws<EmptyException>(() => { s.ThrowIfNullOrWhiteSpaceArgument(); });
		}
		[Test]
		[TestCase("s")]
		public void ThrowIfNullOrWhiteSpaceArgument02(string s)
		{
			Assert.DoesNotThrow(() => { s.ThrowIfNullOrWhiteSpaceArgument(); });
		}
		[Test]
		public void IsNotVoidReturn()
		{
			Assert.IsNotNull(new object().ThrowIfNull());
			Assert.IsNotNull(new object().ThrowIfArgumentNull());
			Assert.IsNotNull(new[] { new object() }.ThrowIfAnyNull());
			Assert.IsNotNull(new[] { new object() }.ThrowIfAnyArgumentNull());
			Assert.IsNotNull("s".ThrowIfNullOrEmpty());
			Assert.IsNotNull("s".ThrowIfNullOrEmptyArgument());
			Assert.IsNotNull(new[] { "s", "t" }.ThrowIfNullOrEmpty());
			Assert.IsNotNull(new[] { "s", "t" }.ThrowIfNullOrEmptyArgument());
			Assert.IsNotNull("s".ThrowIfNullOrWhiteSpace());
			Assert.IsNotNull("s".ThrowIfNullOrWhiteSpaceArgument());
			Assert.IsNotNull(0.ThrowIfArgumentOutOfRange());
			Assert.IsNotNull(((uint)0).ThrowIfArgumentOutOfRange());
		}
	}
}

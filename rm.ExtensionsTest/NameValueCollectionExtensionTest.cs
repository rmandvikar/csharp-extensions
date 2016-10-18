using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest
{
	[TestFixture]
	public class NameValueCollectionExtensionTest
	{
		private NameValueCollection GetCollection(string[] args)
		{
			if (args == null || args.Length == 0)
			{
				return new NameValueCollection();
			}
			if (args.Length % 2 != 0)
			{
				throw new ApplicationException("args length should be even.");
			}
			var collection = new NameValueCollection();
			for (int i = 0; i < args.Length; i += 2)
			{
				collection.Add(args[i], args[i + 1]);
			}
			return collection;
		}

		[Test]
		[TestCase("?k1=v1&k2=v2", "k1", "v1", "k2", "v2")]
		[TestCase("?k1=v1&k1=v1", "k1", "v1", "k1", "v1")]
		[TestCase("?k1%2C=v1%2C", "k1,", "v1,")]
		[TestCase("")]
		public void ToQueryString01(string expected, params string[] args)
		{
			var nvc = GetCollection(args);
			Assert.AreEqual(expected, nvc.ToQueryString());
			Assert.AreEqual(expected.Replace("?", ""), nvc.ToQueryString(false));
		}
		[Test]
		[TestCase(null, "v")]
		[TestCase("k", null)]
		public void ToQueryString02(params string[] args)
		{
			var nvc = GetCollection(args);
			Assert.Throws<NullReferenceException>(() =>
			{
				var query = nvc.ToQueryString();
			});
		}
	}
}

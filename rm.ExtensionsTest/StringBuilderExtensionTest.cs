using System;
using System.Text;
using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest
{
    [TestFixture]
    public class StringBuilderExtensionTest
    {
        [Test]
        [TestCase("this {0} {1} test.", "is", "a")]
        [TestCase("this test.")]
        [TestCase("this ->{0}<- test.", null, null)]
        public void AppendLine01(string format, params object[] args)
        {
            var result = new StringBuilder().AppendLine(format, args).ToString();
            Console.WriteLine(result);
            Assert.AreEqual(string.Format(format, args) + Environment.NewLine, result);
        }
    }
}

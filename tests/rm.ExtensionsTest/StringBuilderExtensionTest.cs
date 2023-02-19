using System;
using System.Linq;
using System.Text;
using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest;

[TestFixture]
public class StringBuilderExtensionTest
{
	private static readonly string CRLF = Environment.NewLine;
	private static readonly string LFCR = new string(Environment.NewLine.ToCharArray().Reverse().ToArray());

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

	[Test]
	[TestCase("this", "siht")]
	[TestCase("this test", "tset siht")]
	[TestCase("0123456789", "9876543210")]
	[TestCase("t", "t")]
	[TestCase("", "")]
	[TestCase("this{CRLF}test", "tset{LFCR}siht")]
	[TestCase("Les Misérables", "selbarésiM seL")]
	[TestCase("Les Mise\u0301rables", "selbaŕesiM seL")]
	public void Reverse01(string input, string expected)
	{
		var actual = new StringBuilder(input.Replace("{CRLF}", CRLF)).Reverse().ToString();
		Assert.AreEqual(expected.Replace("{LFCR}", LFCR), actual);
	}
}

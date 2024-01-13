using System;
using NUnit.Framework;
using Semver;

namespace rm.ExtensionsTest;

[TestFixture]
public class Semver2Test
{
	[Test]
	[TestCase("1.3.37", 1, 3, 37, "", "")]
	[TestCase("1.3.37+531fd3f3c471n9", 1, 3, 37, "", "531fd3f3c471n9")]
	[TestCase("1.3.37-helenmirren", 1, 3, 37, "helenmirren", "")]
	[TestCase("1.3.37-helenmirren+531fd3f3c471n9", 1, 3, 37, "helenmirren", "531fd3f3c471n9")]
	public void Parse_Semver2(string version,
		int major, int minor, int patch, string prerelease, string metadata)
	{
		var semver = SemVersion.Parse(version, SemVersionStyles.Strict);

		Console.WriteLine(semver.Major);
		Console.WriteLine(semver.Minor);
		Console.WriteLine(semver.Patch);
		Console.WriteLine(semver.Prerelease);
		Console.WriteLine(semver.Metadata);

		Assert.AreEqual(major, semver.Major);
		Assert.AreEqual(minor, semver.Minor);
		Assert.AreEqual(patch, semver.Patch);
		Assert.AreEqual(prerelease, semver.Prerelease);
		Assert.AreEqual(metadata, semver.Metadata);
	}
}

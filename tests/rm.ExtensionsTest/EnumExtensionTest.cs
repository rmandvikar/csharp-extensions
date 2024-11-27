using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using rm.Extensions;
using Sc = System.ComponentModel;

namespace rm.ExtensionsTest;

public enum Temperature
{
	C = 1,
	F
}

public enum Color
{
	[Sc.Description("Red color")]
	Red = 1,
	Green,
	[Sc.Description("Blue color")]
	Blue,
	[Sc.Description("")]
	Black,
	[Sc.Description()]
	White,
}

public enum Grade
{
	Toddler = 1,
	[Sc.Description("Pre-K")]
	PreK,
	Kindergarten,
	[Sc.Description("1")]
	One,
	[Sc.Description("2")]
	Two,
	[Sc.Description("3")]
	Three,
	[Sc.Description("4")]
	Four,
	[Sc.Description("5")]
	Five,
	[Sc.Description("6")]
	Six,
	[Sc.Description("7")]
	Seven,
	[Sc.Description("8")]
	Eight,
	[Sc.Description("9")]
	Nine,
	[Sc.Description("10")]
	Ten,
	[Sc.Description("11")]
	Eleven,
	[Sc.Description("12")]
	Twelve,
	College
}

public enum EmptyEnum { }

[TestFixture]
public class EnumExtensionTest
{
	[Test]
	public void Parse01()
	{
		Assert.AreEqual(Color.Red, "Red".Parse<Color>());
	}

	[Test]
	public void Parse02()
	{
		Assert.Throws<ArgumentException>(() => { "Red".Parse<Temperature>(); });
	}

	[Test]
	public void TryParse01()
	{
		Color color;
		Assert.IsTrue("Red".TryParse<Color>(out color));
		Assert.AreEqual(Color.Red, color);
	}

	[Test]
	public void TryParse02()
	{
		Temperature t;
		Assert.IsFalse("Red".TryParse<Temperature>(out t));
		Assert.AreNotEqual(Color.Red, t);
		Assert.AreEqual(0, (int)t);
	}

	[Test]
	public void GetDescription01()
	{
		Assert.AreEqual("Red color", Color.Red.GetDescription());
		Assert.AreEqual(Color.Green.ToString(), Color.Green.GetDescription());
		Assert.AreEqual(Color.Black.ToString(), Color.Black.GetDescription());
		Assert.AreEqual(Color.White.ToString(), Color.White.GetDescription());
	}

	[Test]
	public void UnsupportedEnumValueException01()
	{
		var enumValue = (Color)0;
		var TEnum = enumValue.GetType();
		var ex = Assert.Throws<UnsupportedEnumValueException<Color>>(() =>
		{
			enumValue.GetEnumName();
		});
		Assert.AreEqual($"Value {enumValue} of enum {TEnum.Name} is not supported.", ex.Message);
	}

	[Test]
	public void GetDescription02()
	{
		Assert.Throws<UnsupportedEnumValueException<Color>>(() =>
		{
			((Color)0).GetDescription();
		});
	}

	[Test]
	public void GetEnumValue01()
	{
		Assert.AreEqual(Color.Red, "Red".GetEnumValue<Color>());
		Assert.Throws<UnsupportedEnumValueException<Color>>(() =>
		{
			"UnsupportedEnumValue".GetEnumValue<Color>();
		});
	}

	[Test]
	public void GetEnumValues01()
	{
		var colors = EnumExtension.GetEnumValues<Color>();
		Assert.AreEqual(5, colors.Count());
		Assert.IsTrue(colors.Contains(Color.Red));
		Assert.IsTrue(colors.Contains(Color.Green));
		Assert.IsTrue(colors.Contains(Color.Blue));
	}

	[Test(Description = "GetEnumValue() v/s Enum.ToString() speed test.")]
	[Category("slow")]
	public void GetEnumValue02()
	{
		Action<Func<bool>, string> speedTest = (testPredicate, testName) =>
		{
			var iterations = 1000000;
			DateTime datetime;
			TimeSpan timespan;
			datetime = DateTime.Now;
			foreach (var item in Enumerable.Range(0, iterations))
			{
				if (testPredicate())
				{ }
			}
			timespan = DateTime.Now - datetime;
			Console.WriteLine("{0}: \t{1}", testName, timespan.TotalSeconds);
		};
		// 15x slow
		speedTest(() => { return Color.Red.ToString() == "Red"; }, "Enum.ToString()");
		// 10x slow
		speedTest(() => { return "Red".Parse<Color>() == Color.Red; }, "Enum.Parse()");
		// 1.5x slow
		speedTest(() => { return "Red".GetEnumValue<Color>() == Color.Red; }, "GetEnumValue()");
		// fastest
		speedTest(() => { return Color.Red.GetEnumName() == "Red"; }, "GetEnumName()");
	}

	[Test]
	public void GetEnumNames01()
	{
		var colors = EnumExtension.GetEnumNames<Color>();
		Assert.AreEqual(5, colors.Count());
		Assert.IsTrue(colors.Contains(Color.Red.ToString()));
		Assert.IsTrue(colors.Contains(Color.Green.ToString()));
		Assert.IsTrue(colors.Contains(Color.Blue.ToString()));
	}

	[Test]
	public void GetEnumNameToDescriptionMap01()
	{
		var colorsMap = EnumExtension.GetEnumNameToDescriptionMap<Color>();
		Assert.AreEqual(5, colorsMap.Count());
		Assert.IsTrue(colorsMap.Contains(
			new KeyValuePair<string, string>(Color.Red.ToString(), Color.Red.GetDescription())
			));
		Assert.IsTrue(colorsMap.Contains(
			new KeyValuePair<string, string>(Color.Green.ToString(), Color.Green.GetDescription())
			));
		Assert.IsTrue(colorsMap.Contains(
			new KeyValuePair<string, string>(Color.Blue.ToString(), Color.Blue.GetDescription())
			));
	}

	[Test]
	public void GetEnumNameFromDescription01()
	{
		Assert.AreEqual(Color.Red.ToString(), "Red color".GetEnumNameFromDescription<Color>());
		Assert.Throws<UnsupportedEnumValueException<Color>>(() =>
		{
			"UnsupportedEnumValue".GetEnumNameFromDescription<Color>();
		});
	}

	[Test]
	public void Sorting01()
	{
		var gradesUnsorted = new[] { "Pre-K", "1", "College", "2", "Toddler" };
		var grades = gradesUnsorted
			.Select(x => x.GetEnumValueFromDescription<Grade>()).ToArray();
		Array.Sort(grades);
		Console.WriteLine("orig: {0}", string.Join(", ", gradesUnsorted));
		var gradesSorted = grades.Select(x => x.GetDescription());
		var gradesSortedFlat = string.Join(", ", gradesSorted);
		Assert.AreEqual("Toddler, Pre-K, 1, 2, College", gradesSortedFlat);
		Console.WriteLine("sort: {0}", gradesSortedFlat);
	}

	[Test]
	public void IsDefined01()
	{
		Assert.IsFalse(0.IsDefined<Color>());
		Assert.IsFalse(100.IsDefined<Color>());
		Assert.IsTrue(((int)Color.Red).IsDefined<Color>());
	}
}

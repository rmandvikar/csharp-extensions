using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using rm.Extensions;
using Sc = System.ComponentModel;

namespace rm.ExtensionsTest
{
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
        Blue
    }
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
        }
        [Test]
        public void GetDescription02()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                ((Color)0).GetDescription();
            });
            Assert.Throws<ArgumentException>(() =>
            {
                "T".Parse<Temperature>().GetDescription();
            });
        }
        [Test]
        public void GetEnumValue01()
        {
            Assert.AreEqual(Color.Red, "Red".GetEnumValue<Color>());
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                "OutOfRange".GetEnumValue<Color>();
            });
        }
        [Test]
        public void GetEnumValues01()
        {
            var colors = EnumExtension.GetEnumValues<Color>();
            Assert.AreEqual(3, colors.Count());
            Assert.IsTrue(colors.Contains(Color.Red));
            Assert.IsTrue(colors.Contains(Color.Green));
            Assert.IsTrue(colors.Contains(Color.Blue));
        }
        [Test(Description = "GetEnumValue() v/s Enum.ToString() speed test.")]
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
            // 10x slow
            speedTest(() => { return Color.Red.ToString() == "Red"; }, "Enum.ToString()");
            // 7x slow
            speedTest(() => { return Color.Red == "Red".Parse<Color>(); }, "Enum.Parse()");
            // fastest
            speedTest(() => { return Color.Red == "Red".GetEnumValue<Color>(); }, "GetEnumValue()");
        }
        [Test]
        public void GetEnumNames01()
        {
            var colors = EnumExtension.GetEnumNames<Color>();
            Assert.AreEqual(3, colors.Count());
            Assert.IsTrue(colors.Contains(Color.Red.ToString()));
            Assert.IsTrue(colors.Contains(Color.Green.ToString()));
            Assert.IsTrue(colors.Contains(Color.Blue.ToString()));
        }
        [Test]
        public void GetEnumNameToDescriptionMap01()
        {
            var colorsMap = EnumExtension.GetEnumNameToDescriptionMap<Color>();
            Assert.AreEqual(3, colorsMap.Count());
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
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                "OutOfRange".GetEnumNameFromDescription<Color>();
            });
        }
    }
}

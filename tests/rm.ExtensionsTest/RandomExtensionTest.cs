using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using rm.Extensions;
using rm.Random2;

namespace rm.ExtensionsTest
{
	[TestFixture]
	public class RandomExtensionTest
	{
		private readonly Random random = RandomFactory.GetThreadStaticRandom();

		private const int iterations = 1_000_000;

		[Explicit]
		[Test]
		[TestCase(3500, 350)]
		public void Verify_NextGaussian(double mu, double sigma)
		{
			var binSize = (int)(mu / 10);
			var delays = new List<int>(iterations);
			var bins = CreateBins(binSize);

			for (int i = 0; i < iterations; i++)
			{
				var delay = random.NextGaussian(mu, sigma);
				delays.Add((int)delay);
				Bucketize((int)delay, binSize, bins);
			}

			PrintBins(bins);
			PrintStats(mu, sigma, delays);
		}

		[Explicit]
		[Test]
		[TestCase(3500, 350)]
		public void Verify_NextGaussian_Positive(double mu, double sigma)
		{
			var binSize = (int)(mu / 10);
			var delays = new List<int>(iterations);
			var bins = CreateBins(binSize);

			for (int i = 0; i < iterations; i++)
			{
				var delay = random.NextGaussian(mu, sigma);
				if (delay < 0)
				{
					delay = mu;
				}
				delays.Add((int)delay);
				Bucketize((int)delay, binSize, bins);
			}

			PrintBins(bins);
			PrintStats(mu, sigma, delays);
		}

		[Explicit]
		[Test]
		[TestCase(10, "0123456789")]
		[TestCase(10, "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_")]
		public void Verify_NextString(int length, string charset)
		{
			var unique = new HashSet<string>();
			for (int i = 0; i < iterations; i++)
			{
				var nextString = random.NextString(length, charset);
				unique.Add(nextString);
			}
			Console.WriteLine(unique.Count);
		}

		[Explicit]
		[Test]
		[TestCase(0.0d, 1.0d)]
		[TestCase(1.0d, 10.0d)]
		[TestCase(-1.0d, 1.0d)]
		[TestCase(0.99d, 1.00d)]
		[TestCase(1.0d, 1.0d)]
		public void Verify_NextDouble(double minValue, double maxValue)
		{
			for (int i = 0; i < 1_000; i++)
			{
				var nextDouble = random.NextDouble(minValue, maxValue);
				Console.WriteLine(nextDouble);
			}
		}

		[Test]
		[TestCase(2.0d, 1.0d)]
		public void Verify_NextDouble_Throws(double minValue, double maxValue)
		{
			Assert.Throws<ArgumentOutOfRangeException>(() =>
				random.NextDouble(minValue, maxValue));
		}

		[Explicit]
		[Test]
		[TestCase(0.0d, 1.0d)]
		[TestCase(1.0d, 10.0d)]
		[TestCase(-1.0d, 1.0d)]
		[TestCase(0.99d, 1.00d)]
		[TestCase(1.0d, 1.0d)]
		public void Verify_NextDecimal(double minValue, double maxValue)
		{
			for (int i = 0; i < 1_000; i++)
			{
				var nextDecimal = random.NextDecimal((decimal)minValue, (decimal)maxValue);
				Console.WriteLine(nextDecimal);
			}
		}

		[Test]
		[TestCase(2.0d, 1.0d)]
		public void Verify_NextDecimal_Throws(double minValue, double maxValue)
		{
			Assert.Throws<ArgumentOutOfRangeException>(() =>
				random.NextDecimal((decimal)minValue, (decimal)maxValue));
		}

		private Dictionary<int, int> CreateBins(int binSize)
		{
			var bins = new Dictionary<int, int>();
			for (int i = 0; i < 50; i++)
			{
				var bucket = binSize * i;
				bins.Add(bucket, 0);
			}

			return bins;
		}

		private void PrintBins(Dictionary<int, int> bins)
		{
			var padding = bins.Max(x => x.Key).ToString().Length;
			foreach (var item in bins.OrderBy(x => x.Key))
			{
				Console.WriteLine($"{item.Key.ToString().PadLeft(padding)}: {item.Value}");
			}
		}

		private void PrintStats(double mu, double sigma, List<int> delays)
		{
			Console.WriteLine($"mu: {mu}, sigma: {sigma}");
			var avg = Average(delays);
			var p95 = Percentile(delays, 0.95);
			var p99 = Percentile(delays, 0.99);
			Console.WriteLine($"avg: {avg}");
			Console.WriteLine($"p95: {p95}");
			Console.WriteLine($"p99: {p99}");
		}

		private void Bucketize(int n, int binSize, IDictionary<int, int> bins)
		{
			var bin = Bin(n, binSize);
			bins[bin]++;
		}

		private int Bin(int n, int binSize)
		{
			return n / binSize * binSize;
		}

		public double Percentile(IEnumerable<int> source, double percentile)
		{
			var elements = source.ToArray();
			Array.Sort(elements);
			double realIndex = percentile * (elements.Length - 1);
			int index = (int)realIndex;
			double indexDelta = realIndex - index;
			if (index + 1 < elements.Length)
				return elements[index] * (1 - indexDelta) + elements[index + 1] * indexDelta;
			else
				return elements[index];
		}

		private double Average(IEnumerable<int> source)
		{
			return source.Average();
		}
	}
}

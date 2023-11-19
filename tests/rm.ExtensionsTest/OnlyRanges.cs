using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using rm.Extensions;
using Ex = rm.Extensions.ExceptionHelper;

namespace rm.ExtensionsTest;

public record class Range(int Start, int End);

public interface IRangeFinder
{
	Range Find(int n, Range[] ranges);
}

public class LinearSearchRangeFinder : IRangeFinder
{
	/// <note>
	/// time: O(n)
	/// </note>
	public Range Find(int n, Range[] ranges)
	{
		Ex.ThrowIfArgumentOutOfRange(n < 0, nameof(n));
		Ex.ThrowIfArgumentNull(ranges == null, nameof(ranges));
		Ex.ThrowIfEmpty(ranges.IsNullOrEmpty(), nameof(ranges));
		// assume ranges are sorted, and not-overlapping

		return FindInner(n, ranges);
	}

	private Range FindInner(int n, Range[] ranges)
	{
		var rangeCandidates = ranges.Where(r => r.Start <= n && n <= r.End);
		// throw or return "null object" Range
		return rangeCandidates.FirstOrDefault() // singleOrDefault will throw on overlapping ranges
			?? throw new ArgumentOutOfRangeException($"n ({n}) is out of ... range (pun-intended!)");
	}
}

public class BinarySearchRangeFinder : IRangeFinder
{
	/// <note>
	/// time: O(logn)
	/// </note>
	public Range Find(int n, Range[] ranges)
	{
		Ex.ThrowIfArgumentOutOfRange(n < 0, nameof(n));
		Ex.ThrowIfArgumentNull(ranges == null, nameof(ranges));
		Ex.ThrowIfEmpty(ranges.IsNullOrEmpty(), nameof(ranges));
		// assume ranges are sorted, and not-overlapping

		return FindInner(n, ranges);
	}

	private Range FindInner(int n, Range[] ranges)
	{
		var start = 0;
		var end = ranges.Count() - 1;
		while (start <= end)
		{
			var mid = start + (end - start) / 2;
			var midRange = ranges[mid];
			if (midRange.Start <= n && n <= midRange.End)
			{
				return midRange;
			}
			else if (n < midRange.Start)
			{
				end = mid - 1;
			}
			else if (n > midRange.End)
			{
				start = mid + 1;
			}
		}
		// throw or return "null object" Range
		throw new ArgumentOutOfRangeException($"n ({n}) is out of ... range (pun-intended!)");
	}
}

public class RangeOverlapFinder
{
	/// <note>
	/// time: O(n)
	/// </note>
	public bool IsOverlapping(Range[] ranges)
	{
		Ex.ThrowIfArgumentNull(ranges == null, nameof(ranges));
		Ex.ThrowIfEmpty(ranges.IsNullOrEmpty(), nameof(ranges));
		// assume ranges are sorted

		for (int i = 1; i < ranges.Count(); i++)
		{
			var range1 = ranges[i - 1];
			var range2 = ranges[i];
			if (range1.End >= range2.Start)
			{
				return true;
			}
		}
		return false;
	}

	/// <note>
	/// time: O(n)
	/// </note>
	public bool IsOverlapping(Range range, Range[] ranges)
	{
		Ex.ThrowIfArgumentNull(ranges == null, nameof(ranges));
		Ex.ThrowIfEmpty(ranges.IsNullOrEmpty(), nameof(ranges));
		// assume ranges are sorted, and not-overlapping

		// see https://stackoverflow.com/questions/3269434/whats-the-most-efficient-way-to-test-if-two-ranges-overlap
		// extension of r.start <= n <= r.end as r.start <= end && start <= r.end
		//return ranges.Any(r => r.Start <= range.End && range.Start <= r.End);
		// max(starts) <= min(ends)
		return ranges.Any(r => Math.Max(range.Start, r.Start) <= Math.Min(range.End, r.End));
	}
}

[TestFixture]
public class OnlyRangesTest
{
	[Test]
	[TestCase(0, 0, 9)]
	[TestCase(5000, 5000, 5009)]
	[TestCase(5010, 5010, 5019)]
	[TestCase(99999, 99990, 99999)]
	public void Find_Tight_Ranges_Linear(int n, int start, int end)
	{
		var ranges = GenTightRanges();
		FindRange(new LinearSearchRangeFinder(), n, ranges, start, end);
	}

	[Test]
	[TestCase(0, 0, 9)]
	[TestCase(5000, 5000, 5009)]
	[TestCase(5010, 5010, 5019)]
	[TestCase(99999, 99990, 99999)]
	public void Find_Tight_Ranges_Binary(int n, int start, int end)
	{
		var ranges = GenTightRanges();
		FindRange(new BinarySearchRangeFinder(), n, ranges, start, end);
	}

	[Test]
	[TestCase(0, 0, 9)]
	[TestCase(5000, 5000, 5009)]
	public void Find_Loose_Ranges_Linear(int n, int start, int end)
	{
		var ranges = GenLooseRanges();
		FindRange(new LinearSearchRangeFinder(), n, ranges, start, end);
	}

	[Test]
	[TestCase(0, 0, 9)]
	[TestCase(5000, 5000, 5009)]
	public void Find_Loose_Ranges_Binary(int n, int start, int end)
	{
		var ranges = GenLooseRanges();
		FindRange(new BinarySearchRangeFinder(), n, ranges, start, end);
	}

	[Test]
	[TestCase(5010, 5010, 5019)]
	[TestCase(99999, 99990, 99999)]
	public void Find_Loose_Ranges_Throws(int n, int start, int end)
	{
		var ranges = GenLooseRanges();
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			FindRange(new LinearSearchRangeFinder(), n, ranges, start, end));
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			FindRange(new BinarySearchRangeFinder(), n, ranges, start, end));
	}

	[Test]
	[TestCase(0, 0, 9)]
	[TestCase(99999, 99990, 99999)]
	public void Find_Adhoc_Ranges_Linear(int n, int start, int end)
	{
		var ranges = GenAdhocRanges();
		FindRange(new LinearSearchRangeFinder(), n, ranges, start, end);
	}

	[Test]
	[TestCase(0, 0, 9)]
	[TestCase(99999, 99990, 99999)]
	public void Find_Adhoc_Ranges_Binary(int n, int start, int end)
	{
		var ranges = GenAdhocRanges();
		FindRange(new BinarySearchRangeFinder(), n, ranges, start, end);
	}

	[Test]
	[TestCase(10, 10, 19)]
	[TestCase(5010, 5010, 5019)]
	[TestCase(5020, 5020, 5029)]
	[TestCase(100000, 100000, 100009)]
	public void Find_Adhoc_Ranges_Throws(int n, int start, int end)
	{
		var ranges = GenAdhocRanges();
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			FindRange(new LinearSearchRangeFinder(), n, ranges, start, end));
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			FindRange(new BinarySearchRangeFinder(), n, ranges, start, end));
	}

	[Test]
	[TestCase(0, 0, 9)]
	[TestCase(99999, 99990, 99999)]
	[TestCase(1000, 1000, 1009)]
	[TestCase(2000, 2000, 2009)]
	[TestCase(3000, 3000, 3009)]
	[TestCase(4000, 4000, 4009)]
	[TestCase(5000, 5000, 5009)]
	[TestCase(6000, 6000, 6009)]
	[TestCase(7000, 7000, 7009)]
	[TestCase(8000, 8000, 8009)]
	[TestCase(9000, 9000, 9009)]
	public void Find_Overlapping_Ranges_Linear(int n, int start, int end)
	{
		var ranges = GenOverlappingRanges();
		FindRange(new LinearSearchRangeFinder(), n, ranges, start, end);
	}

	[Test]
	[TestCase(0, 0, 9)]
	[TestCase(99999, 99990, 99999)]
	[TestCase(1000, 1000, 1009)]
	[TestCase(2000, 2000, 2009)]
	[TestCase(3000, 3000, 3009)]
	[TestCase(4000, 4000, 4009)]
	[TestCase(5000, 5000, 5009)]
	[TestCase(6000, 6000, 6009)]
	[TestCase(7000, 7000, 7009)]
	[TestCase(8000, 8000, 8009)]
	[TestCase(9000, 9000, 9009)]
	public void Find_Overlapping_Ranges_Binary(int n, int start, int end)
	{
		var ranges = GenOverlappingRanges();
		FindRange(new BinarySearchRangeFinder(), n, ranges, start, end);
	}

	private Range FindRange(IRangeFinder rangeFinder, int n, Range[] ranges, int start, int end)
	{
		// warmup call
		rangeFinder.Find(0, ranges);

		Console.WriteLine($"isOverlapping: {new RangeOverlapFinder().IsOverlapping(ranges).ToString().ToLowerInvariant()}");

		var stopwatch = Stopwatch.StartNew();
		var range = rangeFinder.Find(n, ranges);
		stopwatch.Stop();

		Console.WriteLine($"for {n,6}, range found: ({range.Start,6},{range.End,6}) in {stopwatch.ElapsedTicks,7} ticks");
		Assert.AreEqual(start, range.Start);
		Assert.AreEqual(end, range.End);

		return range;
	}

	[Test]
	[TestCase(0)]
	[TestCase(50000)]
	[TestCase(100000)]
	[TestCase(150000)]
	[TestCase(199989)]
	public void Perf_Find_Ranges_Linear(int n)
	{
		var ranges = GenLooseRanges();
		PerfFindRange(new LinearSearchRangeFinder(), n, ranges);
	}

	[Test]
	[TestCase(0)]
	[TestCase(50000)]
	[TestCase(100000)]
	[TestCase(150000)]
	[TestCase(199989)]
	public void Perf_Find_Ranges_Binary(int n)
	{
		var ranges = GenLooseRanges();
		PerfFindRange(new BinarySearchRangeFinder(), n, ranges);
	}

	private Range PerfFindRange(IRangeFinder rangeFinder, int n, Range[] ranges)
	{
		Range range = null;
		const int iterations = 100_000;
		var stopwatch = Stopwatch.StartNew();
		for (int i = 0; i < iterations; i++)
		{
			range = rangeFinder.Find(n, ranges);
		}
		stopwatch.Stop();

		Console.WriteLine($"for {n,6}, range found: ({range.Start,6},{range.End,6}) in {stopwatch.ElapsedMilliseconds,7} ms");

		return range;
	}

	private Range[] GenTightRanges()
	{
		var count = 10_000;
		var ranges = new Range[count];
		for (int i = 0; i < count; i++)
		{
			var rangeValue = 10;
			var start = i * rangeValue;
			var end = start + (rangeValue - 1);
			var range = new Range(start, end);
			ranges[i] = range;
		}
		return ranges;
	}

	private Range[] GenLooseRanges()
	{
		var count = 10_000;
		var ranges = new Range[count];
		for (int i = 0; i < count; i++)
		{
			var rangeValue = 10;
			var start = i * rangeValue * 2;
			var end = start + (rangeValue - 1);
			var range = new Range(start, end);
			ranges[i] = range;
		}
		return ranges;
	}

	private Range[] GenAdhocRanges()
	{
		return
			[
				new Range(0, 9),
				new Range(1000, 1009),
				new Range(2000, 2009),
				new Range(3000, 3009),
				new Range(4000, 4009),
				// exlude 5000, 5009
				new Range(6000, 6009),
				new Range(7000, 7009),
				new Range(8000, 8009),
				new Range(9000, 9009),
				new Range(99990, 99999),
			];
	}

	private Range[] GenOverlappingRanges()
	{
		var count = 10_000;
		var ranges = new Range[count];
		for (int i = 0; i < count; i++)
		{
			var rangeValue = 10;
			var start = i * rangeValue;
			var end = start + (rangeValue - 1);
			var range = new Range(start, end);
			ranges[i] = range;
		}

		return ranges.Concat(GenAdhocRanges()).OrderBy(r => r.Start).ToArray();
	}

	[Test]
	// false
	[TestCase(0, 9, false)]
	[TestCase(26, 74, false)]
	[TestCase(91, 99, false)]
	[TestCase(0, 0, false)]
	[TestCase(50, 50, false)]
	[TestCase(100, 100, false)]
	// true
	[TestCase(10, 25, true)]
	[TestCase(75, 90, true)]
	[TestCase(5, 20, true)]
	[TestCase(70, 90, true)]
	[TestCase(20, 20, true)]
	[TestCase(80, 80, true)]
	[TestCase(0, 50, true)]
	[TestCase(50, 100, true)]
	[TestCase(0, 100, true)]
	[TestCase(0, 10, true)]
	[TestCase(90, 100, true)]
	public void Is_Overlapping(int start, int end, bool isOverlapping)
	{
		var ranges =
			new[]
			{
				new Range(10, 25),
				new Range(75, 90),
			};
		Assert.AreEqual(isOverlapping, new RangeOverlapFinder().IsOverlapping(new Range(start, end), ranges));
	}

	[Test]
	public void Is_Overlapping_Edge()
	{
		var ranges =
			new[]
			{
				new Range(10, 25),
			};
		// ranges
		Assert.AreEqual(false, new RangeOverlapFinder().IsOverlapping(ranges));

		// range in ranges
		Assert.AreEqual(false, new RangeOverlapFinder().IsOverlapping(new Range(0, 0), ranges));
		Assert.AreEqual(true, new RangeOverlapFinder().IsOverlapping(new Range(20, 20), ranges));
		Assert.AreEqual(false, new RangeOverlapFinder().IsOverlapping(new Range(50, 50), ranges));
	}
}

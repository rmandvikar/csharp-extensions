using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using rm.Extensions;
using rm.Random2;

namespace rm.ExtensionsTest;

[TestFixture]
public class Metrics
{
	[Test]
	public void PreAggregate()
	{
		var metricPreAggregator = new MetricPreAggregator();

		var rng = RandomFactory.GetThreadLocalRandom();
		var tag1Values = new object[] { "HelenMirren", "TedLasso", "Shahrukh", "RoyKent", };
		var tag2Values = new object[] { "good", "bad", "?", };

		var iterations = 100;
		for (int i = 0; i < iterations; i++)
		{
			var tag1value = rng.NextItem(tag1Values);
			var tag2value = rng.NextItem(tag2Values);
			var count = 1;
			if (
				(tag1value.Equals("HelenMirren") && tag2value.Equals("bad")) ||
				(tag1value.Equals("Shahrukh") && tag2value.Equals("good")) ||
				(tag1value.Equals("Shahrukh") && tag2value.Equals("?"))
				)
			{
				i--;
				continue;
			}
			metricPreAggregator.Add(count, ("type", tag1value), ("status", tag2value));
		}

		var items = metricPreAggregator.Emit().Values
			.OrderBy(x => x.Tag1)
			.ThenBy(x => x.Tag2);
		foreach (var item in items)
		{
			Console.WriteLine(
				$"count: {item.Count,5}, tag1 {item.Tag1.key}: {item.Tag1.value,-12}, tag2 {item.Tag2.key}: {item.Tag2.value,-6}");
		}
	}

	public class MetricRecordItem
	{
		public long Count { get; set; }
		public readonly (string key, object value) Tag1;
		public readonly (string key, object value) Tag2;

		public MetricRecordItem(
			(string key, object value) tag1,
			(string key, object value) tag2
			)
		{
			Tag1 = tag1;
			Tag2 = tag2;
		}

		public override int GetHashCode()
		{
			return (Tag1.value, Tag2.value).GetHashCode();
		}

		public override bool Equals(object obj)
		{
			var item = obj as MetricRecordItem;
			if (item == null)
			{
				return false;
			}

			return this.Tag1.value.Equals(item.Tag1.value)
				&& this.Tag2.value.Equals(item.Tag2.value);
		}
	}

	public class MetricPreAggregator
	{
		private Dictionary<(object tag1value, object tag2value), MetricRecordItem> items;

		public MetricPreAggregator()
		{
			items = new();
		}

		public void Add(int count,
			(string name, object value) tag1,
			(string name, object value) tag2
			)
		{
			var tagsKey = (tag1.value, tag2.value);
			if (!items.TryGetValue(tagsKey, out var item))
			{
				item = new MetricRecordItem(tag1, tag2);
				items.Add(tagsKey, item);
			}
			checked
			{
				item.Count += count;
			}
		}

		public Dictionary<(object value1, object value2), MetricRecordItem> Emit()
		{
			var current = items;
			items = new();
			return current;
		}
	}
}

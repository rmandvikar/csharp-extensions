using System.Collections.Generic;

namespace rm.ExtensionsTest.Sample;

public class ComparableClass2Comparer : IComparer<ComparableClass2>
{
	public int Compare(ComparableClass2 x, ComparableClass2 y)
	{
		return x.Value.CompareTo(y.Value);
	}
}

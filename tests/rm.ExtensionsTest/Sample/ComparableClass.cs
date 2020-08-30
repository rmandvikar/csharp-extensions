using System;

namespace rm.ExtensionsTest.Sample
{
	public class ComparableClass : IComparable<ComparableClass>
	{
		public int CompareTo(ComparableClass other)
		{
			throw new NotImplementedException();
		}
	}
}

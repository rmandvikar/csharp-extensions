using System;

namespace rm.ExtensionsTest.Sample
{
	public class ComparableClass2 : IComparable<ComparableClass2>, IEquatable<ComparableClass2>
	{
		public int Value { get; set; }

		public int CompareTo(ComparableClass2 other)
		{
			if (other == null)
			{
				return 1;
			}
			return Value.CompareTo(other.Value);
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		public bool Equals(ComparableClass2 other)
		{
			if (other == null)
			{
				return false;
			}
			return Value.Equals(other.Value);
		}
	}
}

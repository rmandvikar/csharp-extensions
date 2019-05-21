using System;
using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest
{
	[TestFixture]
	public class BitSetTest
	{
		[Test]
		public void Add01()
		{
			var bitset = new BitSet(1023);

			Assert.IsFalse(bitset.Has(250));
			bitset.Add(250);
			Assert.IsTrue(bitset.Has(250));

			Assert.IsFalse(bitset.Has(0));
			bitset.Add(0);
			Assert.IsTrue(bitset.Has(0));

			Assert.IsFalse(bitset.Has(1023));
			bitset.Add(1023);
			Assert.IsTrue(bitset.Has(1023));

			bitset.Add(250);
			Assert.IsTrue(bitset.Has(250));
		}

		[Test]
		public void Add01u()
		{
			var bitset = new BitSet((uint)1023);

			Assert.IsFalse(bitset.Has((uint)250));
			bitset.Add((uint)250);
			Assert.IsTrue(bitset.Has((uint)250));

			Assert.IsFalse(bitset.Has((uint)0));
			bitset.Add((uint)0);
			Assert.IsTrue(bitset.Has((uint)0));

			Assert.IsFalse(bitset.Has((uint)1023));
			bitset.Add((uint)1023);
			Assert.IsTrue(bitset.Has((uint)1023));

			bitset.Add((uint)250);
			Assert.IsTrue(bitset.Has((uint)250));
		}

		[Test]
		public void Remove01()
		{
			var bitset = new BitSet(2000);

			bitset.Remove(750);
			Assert.IsFalse(bitset.Has(750));
			bitset.Add(750);
			Assert.IsTrue(bitset.Has(750));
			bitset.Remove(750);
			Assert.IsFalse(bitset.Has(750));

			bitset.Remove(0);
			Assert.IsFalse(bitset.Has(0));
			bitset.Add(0);
			Assert.IsTrue(bitset.Has(0));
			bitset.Remove(0);
			Assert.IsFalse(bitset.Has(0));

			bitset.Remove(1023);
			Assert.IsFalse(bitset.Has(1023));
			bitset.Add(1023);
			Assert.IsTrue(bitset.Has(1023));
			bitset.Remove(1023);
			Assert.IsFalse(bitset.Has(1023));

			bitset.Remove(750);
			Assert.IsFalse(bitset.Has(750));
		}

		[Test]
		public void Remove01u()
		{
			var bitset = new BitSet((uint)2000);

			bitset.Remove((uint)750);
			Assert.IsFalse(bitset.Has((uint)750));
			bitset.Add((uint)750);
			Assert.IsTrue(bitset.Has((uint)750));
			bitset.Remove((uint)750);
			Assert.IsFalse(bitset.Has((uint)750));

			bitset.Remove((uint)0);
			Assert.IsFalse(bitset.Has((uint)0));
			bitset.Add((uint)0);
			Assert.IsTrue(bitset.Has((uint)0));
			bitset.Remove((uint)0);
			Assert.IsFalse(bitset.Has((uint)0));

			bitset.Remove((uint)1023);
			Assert.IsFalse(bitset.Has((uint)1023));
			bitset.Add((uint)1023);
			Assert.IsTrue(bitset.Has((uint)1023));
			bitset.Remove((uint)1023);
			Assert.IsFalse(bitset.Has((uint)1023));

			bitset.Remove((uint)750);
			Assert.IsFalse(bitset.Has((uint)750));
		}

		[Test]
		public void Toggle01()
		{
			var bitset = new BitSet(1500);

			Assert.IsFalse(bitset.Has(500));
			bitset.Toggle(500);
			Assert.IsTrue(bitset.Has(500));
			bitset.Toggle(500);
			Assert.IsFalse(bitset.Has(500));

			Assert.IsFalse(bitset.Has(0));
			bitset.Toggle(0);
			Assert.IsTrue(bitset.Has(0));
			bitset.Toggle(0);
			Assert.IsFalse(bitset.Has(0));

			Assert.IsFalse(bitset.Has(1023));
			bitset.Toggle(1023);
			Assert.IsTrue(bitset.Has(1023));
			bitset.Toggle(1023);
			Assert.IsFalse(bitset.Has(1023));
		}

		[Test]
		public void Toggle01u()
		{
			var bitset = new BitSet((uint)1500);

			Assert.IsFalse(bitset.Has((uint)500));
			bitset.Toggle((uint)500);
			Assert.IsTrue(bitset.Has((uint)500));
			bitset.Toggle((uint)500);
			Assert.IsFalse(bitset.Has((uint)500));

			Assert.IsFalse(bitset.Has((uint)0));
			bitset.Toggle((uint)0);
			Assert.IsTrue(bitset.Has((uint)0));
			bitset.Toggle((uint)0);
			Assert.IsFalse(bitset.Has((uint)0));

			Assert.IsFalse(bitset.Has((uint)1023));
			bitset.Toggle((uint)1023);
			Assert.IsTrue(bitset.Has((uint)1023));
			bitset.Toggle((uint)1023);
			Assert.IsFalse(bitset.Has((uint)1023));
		}

		[Test]
		public void IEnumerable01()
		{
			var bitset = new BitSet(255);
			var inputs = new[] { 0, 30, 31, 32, 33, 100, 200, 255 };
			foreach (var item in inputs)
			{
				bitset.Add(item);
			}
			var size = 0;
			foreach (int item in bitset)
			{
				size++;
			}
			Assert.AreEqual(inputs.Length, size);
		}

		[Test]
		public void IEnumerable01u()
		{
			var bitset = new BitSet((uint)255);
			var inputs = new uint[] { 0, 30, 31, 32, 33, 100, 200, 255 };
			foreach (var item in inputs)
			{
				bitset.Add(item);
			}
			var size = 0;
			foreach (var item in bitset)
			{
				size++;
			}
			Assert.AreEqual(inputs.Length, size);
		}

		[Test]
		public void Ctor01()
		{
			Assert.DoesNotThrow(() => { var bitset = new BitSet(0); });
			Assert.Throws<ArgumentOutOfRangeException>(() => { var bitset = new BitSet(-1); });
		}

		[Test]
		public void Ctor01u()
		{
			Assert.DoesNotThrow(() => { var bitset = new BitSet((uint)0); });
		}

		[Test]
		public void Count01()
		{
			var bitset = new BitSet(15);
			var inputs = new[] { 0, 1, 2, 3, 4 };
			foreach (var item in inputs)
			{
				bitset.Add(item);
				bitset.Add(item);
			}
			Assert.AreEqual(bitset.Count, 5);
			for (int i = 0; i < 2; i++)
			{
				bitset.Remove(inputs[i]);
				bitset.Remove(inputs[i]);
			}
			Assert.AreEqual(bitset.Count, 3);
			foreach (var item in inputs)
			{
				bitset.Toggle(item);
			}
			Assert.AreEqual(bitset.Count, 2);
		}

		[Test]
		public void Count01u()
		{
			var bitset = new BitSet(15);
			var inputs = new uint[] { 0, 1, 2, 3, 4 };
			foreach (var item in inputs)
			{
				bitset.Add(item);
				bitset.Add(item);
			}
			Assert.AreEqual(bitset.Count, 5);
			for (int i = 0; i < 2; i++)
			{
				bitset.Remove(inputs[i]);
				bitset.Remove(inputs[i]);
			}
			Assert.AreEqual(bitset.Count, 3);
			foreach (var item in inputs)
			{
				bitset.Toggle(item);
			}
			Assert.AreEqual(bitset.Count, 2);
		}

		[Test]
		public void Clear01()
		{
			var bitset = new BitSet(20);
			foreach (var item in new[] { 0, 1, 2, 3, 4 })
			{
				bitset.Add(item);
			}
			Assert.AreNotEqual(0, bitset.Count);
			bitset.Clear();
			Assert.AreEqual(0, bitset.Count);
		}

		[Test]
		public void FullCapacity01()
		{
			int max = 50;
			var bitset = new BitSet(max);
			for (int i = 0; i <= max; i++)
			{
				bitset.Add(i);
			}
			Assert.AreEqual(max + 1, bitset.Count);
			for (int i = 0; i <= max; i++)
			{
				Assert.IsTrue(bitset.Has(i));
			}
		}

		[Test]
		public void FullCapacity01u()
		{
			uint max = 50;
			var bitset = new BitSet(max);
			for (uint i = 0; i <= max; i++)
			{
				bitset.Add(i);
			}
			Assert.AreEqual(max + 1, bitset.Count);
			for (uint i = 0; i <= max; i++)
			{
				Assert.IsTrue(bitset.Has(i));
			}
		}

		[Test]
		public void Capacity01()
		{
			Assert.AreEqual(1, new BitSet(30).flags.Length);
			Assert.AreEqual(1, new BitSet(31).flags.Length);
			Assert.AreEqual(2, new BitSet(32).flags.Length);
			Assert.AreEqual(2, new BitSet(62).flags.Length);
			Assert.AreEqual(2, new BitSet(63).flags.Length);
			Assert.AreEqual(3, new BitSet(64).flags.Length);
		}

		[Test]
		public void Capacity01u()
		{
			Assert.AreEqual(1, new BitSet((uint)30).flags.Length);
			Assert.AreEqual(1, new BitSet((uint)31).flags.Length);
			Assert.AreEqual(2, new BitSet((uint)32).flags.Length);
			Assert.AreEqual(2, new BitSet((uint)62).flags.Length);
			Assert.AreEqual(2, new BitSet((uint)63).flags.Length);
			Assert.AreEqual(3, new BitSet((uint)64).flags.Length);
		}

		[Test]
		public void Capacity02()
		{
			Assert.AreEqual((int.MaxValue / 32) + 1, new BitSet(int.MaxValue).flags.Length);
		}

		[Test]
		public void Capacity02u()
		{
			Assert.AreEqual((uint.MaxValue / 32) + 1, new BitSet(uint.MaxValue).flags.Length);
		}

		[Test]
		public void OutOfRange01()
		{
			var bitset = new BitSet(1023);
			Assert.Throws<ArgumentOutOfRangeException>(() => bitset.Has(-1));
			Assert.Throws<ArgumentOutOfRangeException>(() => bitset.Has(1024));
			Assert.Throws<ArgumentOutOfRangeException>(() => bitset.Add(-1));
			Assert.Throws<ArgumentOutOfRangeException>(() => bitset.Add(1024));
			Assert.Throws<ArgumentOutOfRangeException>(() => bitset.Remove(-1));
			Assert.Throws<ArgumentOutOfRangeException>(() => bitset.Remove(1024));
			Assert.Throws<ArgumentOutOfRangeException>(() => bitset.Toggle(-1));
			Assert.Throws<ArgumentOutOfRangeException>(() => bitset.Toggle(1024));
		}

		[Test]
		public void OutOfRange01u()
		{
			var bitset = new BitSet(1023);
			Assert.Throws<ArgumentOutOfRangeException>(() => bitset.Has((uint)1024));
			Assert.Throws<ArgumentOutOfRangeException>(() => bitset.Add((uint)1024));
			Assert.Throws<ArgumentOutOfRangeException>(() => bitset.Remove((uint)1024));
			Assert.Throws<ArgumentOutOfRangeException>(() => bitset.Toggle((uint)1024));
		}
	}
}

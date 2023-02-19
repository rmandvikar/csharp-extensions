using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest;

[TestFixture]
public class LruCacheTest
{
	private const int capacity = 5;
	ILruCache<int, int> cache;

	[SetUp]
	public void Setup()
	{
		cache = new LruCache<int, int>(capacity);
	}

	[Test]
	public void Get_01()
	{
		cache.Insert(-1, -1);
		for (int i = 0; i < capacity; i++)
		{
			cache.Insert(i, i);
		}
		Assert.IsFalse(cache.HasKey(-1));
	}

	[Test]
	public void Get_02()
	{
		Assert.AreEqual(default(int), cache.Get(0));
	}

	[Test]
	public void Get_03()
	{
		Assert.DoesNotThrow(() => cache.Get(0));
	}

	[Test]
	public void Insert_01()
	{
		for (int i = 0; i < capacity; i++)
		{
			cache.Insert(i, i);
			Assert.AreEqual(i + 1, cache.Count());
		}
	}

	[Test]
	public void Lru_01()
	{
		int key = 0;
		for (int i = 0; i < capacity; i++, key++)
		{
			cache.Insert(key, key);
		}
		for (int i = 0; i < capacity; i++, key++)
		{
			cache.Insert(key, key);
		}
		for (int i = 0; i < capacity; i++)
		{
			Assert.IsFalse(cache.HasKey(i));
		}
		for (int i = 0; i < capacity; i++)
		{
			Assert.IsTrue(cache.HasKey(capacity + i));
		}
	}

	[Test]
	public void Lru_02()
	{
		int key = 0;
		for (int i = 0; i < capacity; i++, key++)
		{
			cache.Insert(key, key);
		}
		cache.Get(2);
		for (int i = 0; i < capacity - 1; i++, key++)
		{
			cache.Insert(key, key);
		}
		Assert.IsTrue(cache.HasKey(2));
		cache.Insert(key, key);
		Assert.IsFalse(cache.HasKey(2));
	}

	[Test]
	public void Remove_01()
	{
		for (int i = 0; i < capacity; i++)
		{
			cache.Insert(i, i);
		}
		for (int i = 0; i < capacity; i++)
		{
			cache.Remove(i);
			Assert.AreEqual(capacity - i - 1, cache.Count());
		}
	}

	[Test]
	public void HasKey_01()
	{
		for (int i = 0; i < capacity; i++)
		{
			cache.Insert(i, i);
		}
		cache.Remove(0);
		Assert.IsFalse(cache.HasKey(0));
	}

	[Test]
	public void IsEmpty_01()
	{
		Assert.IsTrue(cache.IsEmpty());
		cache.Insert(0, 0);
		Assert.IsFalse(cache.IsEmpty());
	}

	[Test]
	public void IsFull_01()
	{
		for (int i = 0; i < capacity; i++)
		{
			Assert.IsFalse(cache.IsFull());
			cache.Insert(i, i);
		}
		Assert.IsTrue(cache.IsFull());
	}

	[Test]
	public void IsFull_02()
	{
		for (int i = 0; i < capacity; i++)
		{
			cache.Insert(i, i);
		}
		Assert.IsTrue(cache.IsFull());
		cache.Remove(0);
		Assert.IsFalse(cache.IsFull());
	}

	[Test]
	public void Capacity_01()
	{
		Assert.AreEqual(capacity, cache.Capacity());
	}

	[Test]
	public void Count_01()
	{
		Assert.AreEqual(0, cache.Count());
		cache.Insert(0, 0);
		Assert.AreEqual(1, cache.Count());
	}

	[Test]
	public void Count_02()
	{
		for (int i = 0; i < capacity; i++)
		{
			cache.Insert(i, i);
		}
		Assert.AreEqual(capacity, cache.Count());
		cache.Insert(10, 10);
		Assert.AreEqual(capacity, cache.Count());
	}

	[Test]
	public void Clear_01()
	{
		for (int i = 0; i < capacity; i++)
		{
			cache.Insert(i, i);
		}
		cache.Clear();
		Assert.IsTrue(cache.IsEmpty());
	}

	[Test]
	public void Ctor_01()
	{
		Assert.DoesNotThrow(() =>
		{
			cache = new LruCache<int, int>(0);
			cache.Insert(0, 0);
			Assert.IsFalse(cache.HasKey(0));
		});
	}
}

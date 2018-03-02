using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest
{
	[TestFixture]
	public class HashQueueTest
	{
		[Test]
		public void Enqueue01()
		{
			var hashq = new HashQueue<int>();
			hashq.Enqueue(1);
			hashq.Enqueue(1);
			Assert.AreEqual(2, hashq.Count());
		}

		[Test]
		public void Dequeue01()
		{
			var hashq = new HashQueue<int>();
			hashq.Enqueue(1);
			hashq.Enqueue(1);
			Assert.AreEqual(2, hashq.Count());
			Assert.AreEqual(1, hashq.Dequeue());
			Assert.AreEqual(1, hashq.Count());
			Assert.AreEqual(1, hashq.Dequeue());
			Assert.AreEqual(0, hashq.Count());
		}

		class Person
		{
			public string Name;
			public int Age;
			public override int GetHashCode()
			{
				// only Name
				return Name.GetHashCode();
			}
			public override bool Equals(object obj)
			{
				return obj is Person that
					// Name and Age
					&& (this.Name == that.Name
						&& this.Age == that.Age);
			}
		}

		[Test]
		public void EnqueueDequeue_StableCheck_01()
		{
			var person1 = new Person() { Name = "p1", Age = 1 };
			var person2 = new Person() { Name = "p1", Age = 2 };
			Assert.AreEqual(person1.GetHashCode(), person2.GetHashCode());
			Assert.AreNotEqual(person1, person2);
			var hashq = new HashQueue<Person>();
			hashq.Enqueue(person1);
			hashq.Enqueue(person2);
			Assert.AreEqual(2, hashq.Count());
			Person temp;
			temp = hashq.Dequeue();
			Assert.AreSame(person1, temp);
			Assert.AreNotSame(person2, temp);
			Assert.AreEqual(1, hashq.Count());
			temp = hashq.Dequeue();
			Assert.AreSame(person2, temp);
			Assert.AreNotSame(person1, temp);
			Assert.AreEqual(0, hashq.Count());
		}

		[Test]
		public void EnqueueDequeue_StableCheck_02()
		{
			var hashq = new HashQueue<Person>();
			var person1 = new Person() { Name = "p1", Age = 1 };
			hashq.Enqueue(person1);
			hashq.Enqueue(person1);
			Assert.AreEqual(2, hashq.Count());
			Assert.AreSame(person1, hashq.Dequeue());
			Assert.AreEqual(1, hashq.Count());
			Assert.AreSame(person1, hashq.Dequeue());
			Assert.AreEqual(0, hashq.Count());
		}

		[Test]
		public void PeekPeekTail01()
		{
			var hashq = new HashQueue<int>();
			hashq.Enqueue(1);
			hashq.Enqueue(2);
			hashq.Enqueue(3);
			Assert.AreEqual(1, hashq.Peek());
			Assert.AreEqual(3, hashq.PeekTail());
		}

		[Test]
		public void IsEmpty01()
		{
			var hashq = new HashQueue<int>();
			Assert.IsTrue(hashq.IsEmpty());
			hashq.Enqueue(1);
			Assert.IsFalse(hashq.IsEmpty());
			hashq.Dequeue();
			Assert.IsTrue(hashq.IsEmpty());
			Assert.Throws<EmptyException>(() => hashq.Dequeue());
		}

		[Test]
		public void Clear01()
		{
			var hashq = new HashQueue<int>();
			hashq.Enqueue(1);
			hashq.Enqueue(2);
			Assert.IsFalse(hashq.IsEmpty());
			hashq.Clear();
			Assert.IsTrue(hashq.IsEmpty());
			hashq.Enqueue(1);
			hashq.Enqueue(2);
			Assert.IsFalse(hashq.IsEmpty());
		}

		[Test]
		public void Delete_Between_01()
		{
			var hashq = new HashQueue<int>();
			Assert.AreEqual(0, hashq.Count());
			hashq.Enqueue(1);
			hashq.Enqueue(2);
			hashq.Enqueue(3);
			Assert.AreEqual(1, hashq.Peek());
			Assert.AreEqual(3, hashq.PeekTail());
			hashq.Delete(2);
			Assert.AreEqual(1, hashq.Peek());
			Assert.AreEqual(3, hashq.PeekTail());
		}

		[Test]
		public void Delete_HeadTail_02()
		{
			var hashq = new HashQueue<int>();
			Assert.AreEqual(0, hashq.Count());
			hashq.Enqueue(1);
			Assert.AreEqual(1, hashq.Peek());
			Assert.AreEqual(1, hashq.PeekTail());
			hashq.Delete(1);
			Assert.IsTrue(hashq.IsEmpty());
		}

		[Test]
		public void Delete_ReEnqueue_03()
		{
			var hashq = new HashQueue<int>();
			Assert.AreEqual(0, hashq.Count());
			hashq.Enqueue(1);
			Assert.AreEqual(1, hashq.Count());
			hashq.Delete(1);
			Assert.AreEqual(0, hashq.Count());
			hashq.Enqueue(1);
			Assert.AreEqual(1, hashq.Count());
		}

		[Test]
		public void Delete_DuplicateHashCode_04()
		{
			var person1 = new Person() { Name = "p1", Age = 1 };
			var person2 = new Person() { Name = "p1", Age = 2 };
			Assert.AreEqual(person1.GetHashCode(), person2.GetHashCode());
			Assert.AreNotEqual(person1, person2);
			var hashq = new HashQueue<Person>();
			Assert.AreEqual(0, hashq.Count());
			hashq.Enqueue(person1);
			hashq.Enqueue(person2);
			Assert.AreEqual(2, hashq.Count());
			Assert.IsTrue(hashq.Delete(person2));
			Assert.AreEqual(1, hashq.Count());
			Assert.AreSame(person1, hashq.Peek());
		}

		[Test(Description = "HashQueue<T> v/s Queue<T> find speed test.")]
		[Category("slow")]
		public void Find01()
		{
			var count = 1000000;
			var sw = new Stopwatch();

			var hashq = new HashQueue<int>();
			for (int i = 0; i < count; i++)
			{ hashq.Enqueue(i); }
			sw.Start();
			Assert.IsTrue(hashq.Delete(count - 1));
			sw.Stop();
			var hashqTime = sw.ElapsedMilliseconds;
			Console.WriteLine(hashqTime);

			sw.Reset();
			var q = new Queue<int>();
			for (int i = 0; i < count; i++)
			{ q.Enqueue(i); }
			sw.Start();
			// Queue<T>.Remove(x) and Where(x) are O(n).
			q.Where(x => x == count - 1).ToList();
			sw.Stop();
			var qTime = sw.ElapsedMilliseconds;
			Console.WriteLine(qTime);

			Assert.Less(hashqTime, qTime);
			Assert.Less(hashqTime, 3);
		}

		[Test(Description = "HashQueue<T> v/s Queue<T> enumerate speed test.")]
		[Category("slow")]
		public void Enumerate01()
		{
			var count = 1000000;
			var sw = new Stopwatch();

			var hashq = new HashQueue<int>();
			for (int i = 0; i < count; i++)
			{ hashq.Enqueue(i); }
			sw.Start();
			foreach (var item in hashq) { }
			sw.Stop();
			var hashqTime = sw.ElapsedMilliseconds;
			Console.WriteLine(hashqTime);

			sw.Reset();
			var q = new Queue<int>();
			for (int i = 0; i < count; i++)
			{ q.Enqueue(i); }
			sw.Start();
			foreach (var item in q) { }
			sw.Stop();
			var qTime = sw.ElapsedMilliseconds;
			Console.WriteLine(qTime);

			Assert.Less(hashqTime, qTime * 4);
			Assert.Less(hashqTime, 50);
		}
	}
}

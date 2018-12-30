using System;
using System.Collections;
using System.Collections.Generic;

namespace rm.Extensions
{
	/// <summary>
	/// Defines circular stack methods.
	/// </summary>
	public interface ICircularStack<T>
	{
		/// <summary>
		/// Pushes <paramref name="x"/> into stack.
		/// </summary>
		void Push(T x);

		/// <summary>
		/// Pops top from stack.
		/// </summary>
		T Pop();

		/// <summary>
		/// Peeks top of stack.
		/// </summary>
		T Peek();

		/// <summary>
		/// Peeks bottom of stack.
		/// </summary>
		T PeekBottom();

		/// <summary>
		/// Returns true if stack is empty.
		/// </summary>
		bool IsEmpty();

		/// <summary>
		/// Returns count of stack.
		/// </summary>
		int Count();

		/// <summary>
		/// Returns capacity of stack.
		/// </summary>
		int Capacity();

		/// <summary>
		/// Clears stack.
		/// </summary>
		void Clear();
	}

	/// <summary>
	/// Circular stack.
	/// </summary>
	/// <remarks>Uses array as backing store.</remarks>
	public class CircularStack<T> : ICircularStack<T>, IEnumerable<T>
	{
		#region members

		private readonly T[] store;
		private readonly int capacity;
		private int count;
		private int top;

		#endregion

		#region ctors

		public CircularStack(int capacity = 8)
		{
			capacity.ThrowIfArgumentOutOfRange(minRange: 1, exMessage: nameof(capacity));
			store = new T[capacity];
			this.capacity = capacity;
			top = count = 0;
		}

		#endregion

		#region ICircularStack<T> methods

		/// <summary>
		/// Pushes <paramref name="x"/> into stack.
		/// </summary>
		public void Push(T x)
		{
			store[top] = x;
			top = WrapIndex(top + 1);
			if (count < capacity)
			{
				count++;
			}
		}

		/// <summary>
		/// Pops top from stack.
		/// </summary>
		public T Pop()
		{
			if (IsEmpty())
			{
				throw new InvalidOperationException("Stack is empty.");
			}
			top = WrapIndex(top - 1);
			var item = store[top];
			count--;
			return item;
		}

		/// <summary>
		/// Peeks top of stack.
		/// </summary>
		public T Peek()
		{
			if (IsEmpty())
			{
				throw new InvalidOperationException("Stack is empty.");
			}
			return store[WrapIndex(top - 1)];
		}

		/// <summary>
		/// Peeks bottom of stack.
		/// </summary>
		public T PeekBottom()
		{
			if (IsEmpty())
			{
				throw new InvalidOperationException("Stack is empty.");
			}
			return store[WrapIndex(top - count)];
		}

		/// <summary>
		/// Returns true if stack is empty.
		/// </summary>
		public bool IsEmpty()
		{
			return count == 0;
		}

		/// <summary>
		/// Returns count of stack.
		/// </summary>
		public int Count()
		{
			return count;
		}

		/// <summary>
		/// Returns capacity of stack.
		/// </summary>
		public int Capacity()
		{
			return capacity;
		}

		/// <summary>
		/// Clears stack.
		/// </summary>
		public void Clear()
		{
			top = count = 0;
		}

		#endregion

		private int WrapIndex(int index)
		{
			var wIndex = index % capacity;
			if (wIndex < 0)
			{
				wIndex += capacity;
			}
			return wIndex;
		}

		#region IEnumerable<T> methods

		public IEnumerator<T> GetEnumerator()
		{
			var index = top;
			for (int c = 0; c < count; c++)
			{
				index = WrapIndex(index - 1);
				yield return store[index];
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}

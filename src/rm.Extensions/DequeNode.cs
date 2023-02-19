namespace rm.Extensions.Deque;

/// <summary>
/// Deque node.
/// </summary>
public class Node<T>
{
	public readonly T Value;
	internal Node<T> prev;
	internal Node<T> next;
	internal Deque<T> owner;

	internal Node(T value, Deque<T> owner)
	{
		this.Value = value;
		this.owner = owner;
	}
}

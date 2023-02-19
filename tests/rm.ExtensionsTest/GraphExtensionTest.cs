using System.Collections.Generic;
using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest;

[TestFixture]
public class GraphExtensionTest
{
	[Test(Description = " 1->2->3 ")]
	public void IsCyclic_False_01()
	{
		var n1 = new GraphNode("1");
		var n2 = new GraphNode("2");
		var n3 = new GraphNode("3");
		n1.Neighbors = new[] { n2 };
		n2.Neighbors = new[] { n3 };
		var graph = new Graph(new[] { n1, n2, n3 });

		Assert.IsFalse(graph.IsCyclic());
	}

	[Test(Description = " 1<-2<-3 ")]
	public void IsCyclic_False_02()
	{
		var n1 = new GraphNode("1");
		var n2 = new GraphNode("2");
		var n3 = new GraphNode("3");
		n3.Neighbors = new[] { n2 };
		n2.Neighbors = new[] { n1 };
		var graph = new Graph(new[] { n1, n2, n3 });

		Assert.IsFalse(graph.IsCyclic());
	}

	[Test(Description = " 1->2->3, 1->3 ")]
	public void IsCyclic_False_03()
	{
		var n1 = new GraphNode("1");
		var n2 = new GraphNode("2");
		var n3 = new GraphNode("3");
		n1.Neighbors = new[] { n2, n3 };
		n2.Neighbors = new[] { n3 };
		var graph = new Graph(new[] { n1, n2, n3 });

		Assert.IsFalse(graph.IsCyclic());
	}

	[Test(Description = " 1<->2 ")]
	public void IsCyclic_True_01()
	{
		var n1 = new GraphNode("1");
		var n2 = new GraphNode("2");
		n1.Neighbors = new[] { n2 };
		n2.Neighbors = new[] { n1 };
		var graph = new Graph(new[] { n1, n2 });

		Assert.IsTrue(graph.IsCyclic());
	}

	[Test(Description = " 1->2->3, 3->1 ")]
	public void IsCyclic_True_02()
	{
		var n1 = new GraphNode("1");
		var n2 = new GraphNode("2");
		var n3 = new GraphNode("3");
		n1.Neighbors = new[] { n2 };
		n2.Neighbors = new[] { n3 };
		n3.Neighbors = new[] { n1 };
		var graph = new Graph(new[] { n1, n2, n3 });

		Assert.IsTrue(graph.IsCyclic());
	}

	[Test(Description = " 1->2<->3, 2->4, 4->3 ")]
	public void IsCyclic_True_03()
	{
		var n1 = new GraphNode("1");
		var n2 = new GraphNode("2");
		var n3 = new GraphNode("3");
		var n4 = new GraphNode("4");
		n1.Neighbors = new[] { n2 };
		n2.Neighbors = new[] { n3, n4 };
		n3.Neighbors = new[] { n2 };
		n4.Neighbors = new[] { n3 };
		var graph = new Graph(new[] { n1, n2, n3, n4 });

		Assert.IsTrue(graph.IsCyclic());
	}

	[Test(Description = " 1->2, 3->4, 4->3 ")]
	public void IsCyclic_True_04()
	{
		var n1 = new GraphNode("1");
		var n2 = new GraphNode("2");
		var n3 = new GraphNode("3");
		var n4 = new GraphNode("4");
		n1.Neighbors = new[] { n2 };
		n3.Neighbors = new[] { n4 };
		n4.Neighbors = new[] { n3 };
		var graph = new Graph(new[] { n1, n2, n3, n4 });

		Assert.IsTrue(graph.IsCyclic());
	}
}

#region Graph interfaces

/// <summary>
/// Graph.
/// </summary>
public class Graph : IGraph
{
	#region IGraph methods
	public IEnumerable<IGraphNode> Nodes { get; private set; }
	#endregion

	/// <summary>
	/// ctor.
	/// </summary>
	public Graph(IEnumerable<IGraphNode> nodes)
	{
		this.Nodes = nodes;
	}
}

/// <summary>
/// Graph node.
/// </summary>
public class GraphNode : IGraphNode
{
	#region IGraphNode methods

	public string Id { get; private set; }

	public IEnumerable<IGraphNode> Neighbors { get; set; }

	#endregion

	private static readonly IEnumerable<IGraphNode> emptyGraphNodes = new GraphNode[0];

	/// <summary>
	/// ctor.
	/// </summary>
	public GraphNode(string id, IEnumerable<IGraphNode> neighbors = null)
	{
		this.Id = id;
		this.Neighbors = neighbors ?? emptyGraphNodes;
	}

	public override string ToString()
	{
		return Id;
	}
}

#endregion

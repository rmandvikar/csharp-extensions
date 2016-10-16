using System.Collections.Generic;

namespace rm.Extensions
{
    /// <summary>
    /// Graph extensions.
    /// </summary>
    public static class GraphExtension
    {
        /// <summary>
        /// Returns true if graph is cyclic.
        /// </summary>
        public static bool IsCyclic(this IGraph graph)
        {
            graph.ThrowIfArgumentNull(nameof(graph));
            graph.Nodes.ThrowIfArgumentNull(nameof(graph.Nodes));
            var acyclicNodes = new HashSet<string>();
            var path = new HashSet<string>();
            foreach (var node in graph.Nodes)
            {
                if (IsCyclic(node, path, acyclicNodes))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Returns true if graph node is cyclic.
        /// </summary>
        /// <param name="node">Graph node.</param>
        /// <param name="path">Path from a graph node to this <paramref name="graph node"/>.</param>
        /// <param name="acyclicNodes">Nodes from which a cycle does not exist.</param>
        /// <returns>Returns true if graph node is cyclic.</returns>
        private static bool IsCyclic(IGraphNode node, ISet<string> path, ISet<string> acyclicNodes)
        {
            node.ThrowIfArgumentNull(nameof(node));
            node.Id.ThrowIfArgumentNull(nameof(node.Id));
            node.Neighbors.ThrowIfArgumentNull(nameof(node.Neighbors));
            if (acyclicNodes.Contains(node.Id))
            {
                return false;
            }
            if (path.Contains(node.Id))
            {
                return true;
            }
            path.Add(node.Id);
            foreach (var neighbor in node.Neighbors)
            {
                if (IsCyclic(neighbor, path, acyclicNodes))
                {
                    return true;
                }
            }
            path.Remove(node.Id);
            acyclicNodes.Add(node.Id);
            return false;
        }
    }

    #region Graph interfaces

    /// <summary>
    /// Defines Graph.
    /// </summary>
    public interface IGraph
    {
        /// <summary>
        /// Nodes in the graph.
        /// </summary>
        IEnumerable<IGraphNode> Nodes { get; }
    }
    /// <summary>
    /// Defines Graph node.
    /// </summary>
    public interface IGraphNode
    {
        /// <summary>
        /// Node's Id.
        /// </summary>
        string Id { get; }
        /// <summary>
        /// Node's neighbors.
        /// </summary>
        IEnumerable<IGraphNode> Neighbors { get; }
    }

    #endregion
}

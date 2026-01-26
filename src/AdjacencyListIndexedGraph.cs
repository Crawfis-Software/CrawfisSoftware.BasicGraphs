using CrawfisSoftware.Collections.Graph;

using System;
using System.Collections.Generic;

namespace CrawfisSoftware.Collections.BasicGraphs
{
    /// <summary>
    /// A simple (immutable) implementation of an indexed graph using an adjacency list.
    /// </summary>
    /// <typeparam name="N">The type of the nodes in the graph.</typeparam>
    /// <typeparam name="E">The type of the data on an edge.</typeparam>
    public class AdjacencyListIndexedGraph<N, E> : IIndexedGraph<N, E>
    {
        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="adjacencies">A list of nodes connected to each node.</param>
        /// <param name="nodeLabels">The labels for each node.</param>
        /// <param name="isUndirected">Indicates whether the graph is undirected. If undirected, assumes the adjacencies parameter has duplicate edges (one reversed).</param>
        public AdjacencyListIndexedGraph(in IList<ICollection<IIndexedEdge<E>>> adjacencies, IList<N> nodeLabels = null, bool isUndirected = false)
        {
            _adjacencyList = adjacencies;
            _nodeLabels = nodeLabels;
            _isUndirected = isUndirected;
        }
        #endregion

        #region Implicit IIndexedGraph<N, E> Implementation
        /// <inheritdoc/>
        public int NumberOfEdges
        {
            get
            {
                int sum = 0;
                foreach (var node in _adjacencyList)
                {
                    sum += node.Count;
                }
                sum /= _isUndirected ? 2 : 1;
                return sum;
            }
        }
        /// <inheritdoc/>
        public int NumberOfNodes { get { return _adjacencyList.Count; } }
        /// <summary>
        /// Iterator for the nodes in the graph.
        /// </summary>
        public IEnumerable<int> Nodes
        {
            get
            {
                for (int index = 0; index < _adjacencyList.Count; index++)
                {
                    yield return index;
                }
            }
        }

        /// <summary>
        /// Iterator for the children or neighbors of the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>An enumerator of nodes.</returns>
        public IEnumerable<int> Neighbors(int node)
        {
            foreach (var edge in _adjacencyList[node])
            {
                int index = edge.To;
                yield return index;
            }
        }

        /// <summary>
        /// Iterator over the emanating edges from a node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>An enumerator of nodes.</returns>
        public IEnumerable<IIndexedEdge<E>> OutEdges(int node)
        {
            foreach (var edge in _adjacencyList[node])
            {
                yield return edge;
            }
        }

        /// <summary>
        /// Iterator for the edges in the graph, yielding IEdge's
        /// </summary>
        public IEnumerable<IIndexedEdge<E>> Edges
        {
            get
            {
                for (int node = 0; node < _adjacencyList.Count; node++)
                {
                    foreach (var edge in _adjacencyList[node])
                    {
                        yield return edge;
                    }
                }
            }
        }

        /// <summary>
        /// Tests whether an edge exists between two nodes.
        /// </summary>
        /// <param name="fromNode">The node that the edge emanates from.</param>
        /// <param name="toNode">The node that the edge terminates at.</param>
        /// <returns>True if the edge exists in the graph. False otherwise.</returns>
        public bool ContainsEdge(int fromNode, int toNode)
        {
            foreach (var edge in _adjacencyList[fromNode])
            {
                if (edge.To == toNode)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the label on an edge.
        /// </summary>
        /// <param name="fromNode">The node that the edge emanates from.</param>
        /// <param name="toNode">The node that the edge terminates at.</param>
        /// <returns>The edge.</returns>
        public E GetEdgeLabel(int fromNode, int toNode)
        {
            E result;
            if (!TryGetEdgeLabel(fromNode, toNode, out result))
                throw new ArgumentException("The specified edge does not exist.");

            return result;
        }

        /// <summary>
        /// Exception safe routine to get the label on an edge.
        /// </summary>
        /// <param name="fromNode">The node that the edge emanates from.</param>
        /// <param name="toNode">The node that the edge terminates at.</param>
        /// <param name="edgeLabel">The resulting edge label if the method was successful. A default
        /// value for the type if the edge could not be found.</param>
        /// <returns>True if the edge was found. False otherwise.</returns>
        public bool TryGetEdgeLabel(int fromNode, int toNode, out E edgeLabel)
        {
            foreach (var edge in _adjacencyList[fromNode])
            {
                if (edge.To == toNode)
                {
                    edgeLabel = edge.Value;
                    return true;
                }
            }
            edgeLabel = default(E);
            return false;
        }
        #endregion

        #region Explicit IIndexedGraph implementation
        /// <summary>
        /// Iterator over the parents or immediate ancestors of a node.
        /// </summary>
        /// <remarks>May not be supported by all graphs.</remarks>
        /// <param name="node">The node.</param>
        /// <returns>An enumerator of nodes.</returns>
        IEnumerable<int> IIndexedGraph<N, E>.Parents(int node)
        {
            if (!_isUndirected)
                throw new ApplicationException("AdjacencyListIndexedGraph does not keep track of parents and in-edges for directed graphs.");
            return Neighbors(node);
        }

        /// <summary>
        /// Iterator over the in-coming edges of a node.
        /// </summary>
        /// <remarks>May not be supported by all graphs.</remarks>
        /// <param name="node">The node.</param>
        /// <returns>An enumerator of edges.</returns>
        IEnumerable<IIndexedEdge<E>> IIndexedGraph<N, E>.InEdges(int node)
        {
            if (!_isUndirected)
                throw new ApplicationException("AdjacencyListIndexedGraph does not keep track of parents and in-edges for directed graphs.");
            return OutEdges(node);
        }

        /// <inheritdoc/>
        public N GetNodeLabel(int nodeIndex)
        {
            if (_nodeLabels == null)
                return default(N);
            return _nodeLabels[nodeIndex];
        }
        #endregion

        #region
        private IList<ICollection<IIndexedEdge<E>>> _adjacencyList;
        private IList<N> _nodeLabels;
        private bool _isUndirected;
        #endregion
    }
}
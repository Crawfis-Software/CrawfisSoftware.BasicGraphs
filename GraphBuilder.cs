using CrawfisSoftware.Collections.BasicGraphs;

using System;
using System.Collections.Generic;

namespace CrawfisSoftware.Collections.Graph
{
    /// <summary>
    /// Helper class used to construct a GraphBuilder where we do not care what the node or edge types are.
    /// </summary>
    public class GraphBuilder : GraphBuilder<object, object>
    {
        /// <summary>
        /// Add an edge from node index <paramref name="from"/> to node index <paramref name="to"/>.
        /// </summary>
        /// <param name="from">Existing node index for the directed edge.</param>
        /// <param name="to">Existing node index for the directed edge.</param>
        /// <param name="undirected">If true (default) an edge is also created in the reverse direction.</param>
        public void AddEdge(int from, int to, bool undirected = true)
        {
            AddEdge(from, to, null, undirected);
        }

    }
    /// <summary>
    /// Explicit graph building creating a SimpleGraph (which uses an adjacency list).
    /// </summary>
    /// <typeparam name="N">The node label type.</typeparam>
    /// <typeparam name="E">The edge label type.</typeparam>
    public class GraphBuilder<N, E>
    {
        private IList<ICollection<IIndexedEdge<E>>> _adjacencyLists = new List<ICollection<IIndexedEdge<E>>>(DefaultSize);
        private bool _isUndirected = true;
        private int _defaultDegree = 2;
        /// <summary>
        /// Specifies the default size of the Adjacency lists for each node.
        /// </summary>
        public int DefaultDegree
        {
            get { return _defaultDegree; }
            set { _defaultDegree = value; }
        }

        private static int _defaultSize = 30;
        /// <summary>
        /// Specifies the default number of nodes in the resulting graph.
        /// </summary>
        public static int DefaultSize
        {
            get { return _defaultSize; }
            set { _defaultSize = value; }
        }

        /// <summary>
        /// Set or get a default value for all new nodes.
        /// </summary>
        public N DefaultNodeValue { get; set; } = default(N);

        /// <summary>
        /// A a new node with a default node value.
        /// </summary>
        public int AddNode()
        {
            return AddNode(DefaultNodeValue);
        }

        private IList<N> _nodeValues = new List<N>();
        /// <summary>
        /// Add a new node with the specified node value.
        /// </summary>
        /// <param name="nodeValue"></param>
        public int AddNode(N nodeValue)
        {
            _adjacencyLists.Add(new List<IIndexedEdge<E>>(DefaultDegree));
            _nodeValues.Add(nodeValue);
            return _adjacencyLists.Count - 1;
        }

        /// <summary>
        /// Add an edge from the 
        /// </summary>
        /// <param name="from">The node index of the starting edge node (directional).</param>
        /// <param name="to">The node index of the ending edge node.</param>
        /// <param name="edgeValue">An edge vale to assign to the resulting edge.</param>
        /// <param name="undirected">If true (default) an edge is also created in the reverse direction.</param>
        public void AddEdge(int from, int to, E edgeValue, bool undirected = true)
        {
            ValidateNode(from);
            ValidateNode(to);

            var forwardEdge = new IndexedEdge<E>(from, to, edgeValue);
            _adjacencyLists[from].Add(forwardEdge);
            if (!undirected) { _isUndirected = false; return; }

            var backwardEdge = new IndexedEdge<E>(to, from, edgeValue);
            _adjacencyLists[to].Add(backwardEdge);
        }

        internal void ValidateNode(int node)
        {
            if (node < 0 || node > _adjacencyLists.Count)
                throw new ArgumentOutOfRangeException("The specified node does not exist.");
        }

        /// <summary>
        /// Extract an <c>IIndexedGraph</c> from the builder.
        /// </summary>
        /// <returns>An <c>IIndexedGraph</c>.</returns>
        public IIndexedGraph<N, E> GetGraph()
        {
            return new AdjacencyListIndexedGraph<N, E>(_adjacencyLists, _nodeValues, _isUndirected);
        }

        /// <summary>
        /// Reduce the storage needed to represent the current state of the graph.
        /// </summary>
        public void Optimize()
        {
            foreach (ICollection<IIndexedEdge<E>> list in _adjacencyLists)
            {
                ((List<IIndexedEdge<E>>)list).TrimExcess();
            }
            ((List<ICollection<IIndexedEdge<E>>>)_adjacencyLists).TrimExcess();
        }

        /// <summary>
        /// EdgeCapacity is used only to test the Optimize routine in the Unit Testing.
        /// </summary>
        internal int EdgeCapacity
        {
            get
            {
                int size = 0;
                for (int node = 0; node < _adjacencyLists.Count; node++)
                {
                    size += ((List<int>)_adjacencyLists[node]).Capacity;
                }
                return size;
            }
        }
    }
}
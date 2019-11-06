using System;
using System.Collections.Generic;

namespace OhioState.Collections.Graph
{
    /// <summary>
    /// SimpleGraph takes as input an Adjacency List and
    /// provides the IGraph wrapper around it.
    /// </summary>
    public class SimpleGraph : IGraph<int,int>
    {

        internal struct Connection : IEdge<int, int>
        {
            private int _from, _to;
            public Connection( int from, int to )
            {
                _from = from;
                _to = to;
            }

            #region IEdge<int,int> Members
            public int From
            {
                get { return _from; }
            }

            public int To
            {
                get { return _to; }
            }

            public int Value
            {
                get { return EdgeValue; }
            }
            #endregion
        }

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="adjacencies">A list of nodes connected to each node.</param>
        public SimpleGraph( IList<ICollection<int>> adjacencies )
        {
            _adjacencyList = adjacencies;
        }
        #endregion

        #region Implicit IGraph<int,int> Implementation
        /// <summary>
        /// Iterator for the nodes in the graoh.
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
            foreach (int index in _adjacencyList[node])
            {
                yield return index;
            }
        }

        /// <summary>
        /// Iterator over the emanating edges from a node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>An enumerator of nodes.</returns>
        public IEnumerable<IEdge<int, int>> OutEdges(int node)
        {
            foreach (int index in _adjacencyList[node])
            {
                yield return new Connection(node, index);
            }
        }

        /// <summary>
        /// Iterator for the edges in the graph, yielding IEdge's
        /// </summary>
        public IEnumerable<IEdge<int, int>> Edges
        {
            get
            {
                for (int node = 0; node < _adjacencyList.Count; node++ )
                {
                    foreach (int index in _adjacencyList[node])
                    {
                        yield return new Connection(node, index);
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
            return _adjacencyList[fromNode].Contains(toNode);
        }

        /// <summary>
        /// Gets the label on an edge.
        /// </summary>
        /// <param name="fromNode">The node that the edge emanates from.</param>
        /// <param name="toNode">The node that the edge terminates at.</param>
        /// <returns>The edge.</returns>
        public int GetEdgeLabel(int fromNode, int toNode)
        {
            int result;
            if (!TryGetEdge(fromNode, toNode, out result))
                throw new ArgumentException("The specified edge does not exist.");

            return result;
        }

        /// <summary>
        /// Exception safe routine to get the label on an edge.
        /// </summary>
        /// <param name="fromNode">The node that the edge emanates from.</param>
        /// <param name="toNode">The node that the edge terminates at.</param>
        /// <param name="edge">The resulting edge if the method was successful. A default
        /// value for the type if the edge could not be found.</param>
        /// <returns>True if the edge was found. False otherwise.</returns>
        public bool TryGetEdge(int fromNode, int toNode, out int edge)
        {
            if (ContainsEdge(fromNode, toNode))
            {
                edge = EdgeValue;
                return true;
            }
            edge = -1;
            return false;
        }
        #endregion

        #region Explicit IGraph implementation
        /// <summary>
        /// Iterator over the parents or immediate ancestors of a node.
        /// </summary>
        /// <remarks>May not be supported by all graphs.</remarks>
        /// <param name="node">The node.</param>
        /// <returns>An enumerator of nodes.</returns>
        IEnumerable<int> IGraph<int, int>.Parents(int node)
        {
            throw new ApplicationException("SimpleGraph does not keep track of parents and in-edges.");
        }

        /// <summary>
        /// Iterator over the in-coming edges of a node.
        /// </summary>
        /// <remarks>May not be supported by all graphs.</remarks>
        /// <param name="node">The node.</param>
        /// <returns>An enumerator of edges.</returns>
        IEnumerable<IEdge<int, int>> IGraph<int, int>.InEdges(int node)
        {
            throw new ApplicationException("SimpleGraph does not keep track of parents and in-edges.");
        }
        #endregion

        #region
        private const int EdgeValue = 1;
        private IList<ICollection<int>> _adjacencyList;
        #endregion
    }
}

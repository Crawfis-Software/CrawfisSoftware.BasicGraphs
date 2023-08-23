using System;
using System.Collections.Generic;

namespace CrawfisSoftware.Collections.Graph
{
    /// <summary>
    /// Light weight implementation of a complete graph using functions 
    /// for the edge weights and node labels.
    /// </summary>
    /// <typeparam name="N">The type used for node labels or data.</typeparam>
    /// <typeparam name="E">The type used for edge weights or data.</typeparam>
    public class CompleteIndexedGraph<N, E> : IIndexedGraph<N, E>
    {
        private Func<int,N> nodeFunction;
        private Func<int,int,E> edgeFunction;

        /// <inheritdoc/>
        public int NumberOfEdges { get { return NumberOfNodes * (NumberOfNodes + 1) / 2; } }
        /// <inheritdoc/>
        public int NumberOfNodes { get; private set; }
        /// <inheritdoc/>
        public IEnumerable<int> Nodes
        {
            get
            {
                for (int i = 0; i < NumberOfNodes; i++)
                    yield return i;
            }
        }
        /// <inheritdoc/>
        public IEnumerable<IIndexedEdge<E>> Edges {
            get
            {
                for (int i = 0; i < NumberOfNodes; i++)
                    for (int j = i + 1; j < NumberOfNodes; j++)
                    {
                        E label = GetEdgeLabel(i, j);
                        yield return new IndexedEdge<E>(i, j, label);
                    }
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="numberOfNodes">The number of nodes in this complete graph.</param>
        /// <param name="nodeLabelFunc">A delegate (Func) that takes a node index (int) and returns
        /// a node label (N).</param>
        /// <param name="edgeLabelFunc">A delegate (Func) that takes two node indices (int's) and 
        /// returns an edge label (E).</param>
        public CompleteIndexedGraph(int numberOfNodes, in Func<int, N> nodeLabelFunc,in Func<int, int, E> edgeLabelFunc)
        {
            this.NumberOfNodes = numberOfNodes;
            nodeFunction = nodeLabelFunc;
            edgeFunction = edgeLabelFunc;
        }

        /// <inheritdoc/>
        public bool ContainsEdge(int fromNode, int toNode)
        {
            if (fromNode >= 0 && fromNode < NumberOfNodes && toNode >= 0 && toNode < NumberOfNodes && fromNode != toNode)
                return true;
            return false;
        }

        /// <inheritdoc/>
        public E GetEdgeLabel(int fromNode, int toNode)
        {
            if (fromNode < 0 || fromNode >= NumberOfNodes || toNode < 0 || toNode >= NumberOfNodes)
                throw new ArgumentOutOfRangeException("The specified node index is out of range for the given Grid<N,E>");
            return edgeFunction(fromNode, toNode);
        }

        /// <inheritdoc/>
        public N GetNodeLabel(int nodeIndex)
        {
            if (nodeIndex < 0 || nodeIndex >= NumberOfNodes)
                throw new ArgumentOutOfRangeException("The specified node index is out of range for the given Grid<N,E>");
            return nodeFunction(nodeIndex);
        }

        /// <inheritdoc/>
        public IEnumerable<IIndexedEdge<E>> InEdges(int nodeIndex)
        {
            for (int j = 0; j < NumberOfNodes; j++)
            {
                if (nodeIndex == j) continue;
                E label = GetEdgeLabel(j, nodeIndex);
                yield return new IndexedEdge<E>(j, nodeIndex, label);
            }
        }

        /// <inheritdoc/>
        public IEnumerable<int> Neighbors(int nodeIndex)
        {
            return Nodes;
        }

        /// <inheritdoc/>
        public IEnumerable<IIndexedEdge<E>> OutEdges(int nodeIndex)
        {
            for (int j = 0; j < NumberOfNodes; j++)
            {
                if (nodeIndex == j) continue;
                E label = GetEdgeLabel(nodeIndex, j);
                yield return new IndexedEdge<E>(nodeIndex, j, label);
            }
        }

        /// <inheritdoc/>
        public IEnumerable<int> Parents(int nodeIndex)
        {
            return Nodes;
        }

        /// <inheritdoc/>
        public bool TryGetEdgeLabel(int fromNode, int toNode, out E edge)
        {
            edge = default(E);
            if (fromNode < 0 || fromNode >= NumberOfNodes || toNode < 0 || toNode >= NumberOfNodes)
                return false;
            if(fromNode == toNode) return false;
            edge = edgeFunction(fromNode, toNode);
            return true;
        }
    }
}

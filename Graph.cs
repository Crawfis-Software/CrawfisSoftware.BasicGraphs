using System;
using System.Collections.Generic;

//[assembly: SecurityPermission(SecurityAction.RequestMinimum, Execution = true)]
//[assembly: PermissionSet(SecurityAction.RequestOptional, Name = "Nothing")]
namespace CrawfisSoftware.Collections.Graph
{
    /// <summary>
    /// A standard graph implementation of <typeparamref name="IGraph{N,E}"/>.
    /// </summary>
    /// <typeparam name="N">The type associated at each node. Called a node or node label</typeparam>
    /// <typeparam name="E">The type associated at each edge. Also called the edge label.</typeparam>   
    public class Graph<N, E> : IGraph<N, E>
    {
        #region Properties
        /// <summary>
        /// Get the number of nodes in the graph.
        /// </summary>
        public int NumberOfNodes
        {
            get { return vertexList.Count; }
        }
        #endregion

        /// <summary>
        /// Factory method to create a GraphNode. This is the only way to
        /// create a GraphNode and aids in the management of the nodes.
        /// </summary>
        /// <param name="node">The internal object that the node contains.</param>
        /// <returns>A new instance of a GraphNode."/></returns>
        public void AddNode(N node)
        {
            if (vertexList.Contains(node))
                throw new ArgumentException("Only Simple graphs are supported. You can not add the same vertex twice.");

            GraphNode<N, E> newNode = new GraphNode<N, E>(node);
            newNode.Owner = this;
            vertexList.Add(node);
            nodeList.Add(newNode);
        }

        /// <summary>
        /// Add an edge to the graph.
        /// </summary>
        /// <param name="from">The node label for the originating node.</param>
        /// <param name="to">The node label for the destination node.</param>
        /// <param name="edgeData">An edge label to associate with this edge.</param>
        public void AddEdge(N from, N to, E edgeData)
        {
            int fromIndex = vertexList.IndexOf(from);
            if (fromIndex == -1)
                throw new ArgumentException("The specified node or vertex was not found in the graph.");

            int toIndex = vertexList.IndexOf(to);
            if (toIndex == -1)
                throw new ArgumentException("The specified node or vertex was not found in the graph.");

            AddEdge(edgeData, fromIndex, toIndex);
        }

        #region Implementation
        private void AddEdge(E edgeData, int fromIndex, int toIndex)
        {
            GraphNode<N, E> fromNode = nodeList[fromIndex];
            GraphNode<N, E> toNode = nodeList[toIndex];

            AddEdge(edgeData, fromNode, toNode);
        }

        internal void AddEdge(in E edgeData, GraphNode<N, E> fromNode, GraphNode<N, E> toNode)
        {
            GraphEdge<N, E> newEdge = new GraphEdge<N, E>(fromNode, toNode, edgeData);
            fromNode.AddEdge(newEdge);
            toNode.AddEdge(newEdge);
        }
        #endregion

        #region IGraph<VertexDataType, EdgeDataType>
        /// <summary>
        /// Iterator for the nodes in the graoh.
        /// </summary>
        public IEnumerable<N> Nodes
        {
            get
            {
                foreach (N node in vertexList)
                {
                    yield return node;
                }
            }
        }
        /// <summary>
        /// Iterator for the children or neighbors of the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>An enumerator of nodes.</returns>
        public IEnumerable<N> Neighbors(N node)
        {
            GraphNode<N, E> gNode;
            int index = vertexList.IndexOf(node);
            if (index == -1)
                throw new ArgumentException("The specified node or vertex was not found in the graph.");

            gNode = nodeList[index];
            foreach (GraphEdge<N, E> edge in gNode.EdgesOut)
            {
                yield return edge.To;
            }
        }
        /// <summary>
        /// Iterator over the parents or immediate ancestors of a node.
        /// </summary>
        /// <remarks>May not be supported by all graphs.</remarks>
        /// <param name="node">The node.</param>
        /// <returns>An enumerator of nodes.</returns>
        public IEnumerable<N> Parents(N node)
        {
            GraphNode<N, E> gNode;
            int index = vertexList.IndexOf(node);
            if (index == -1)
                throw new ArgumentException("The specified node or vertex was not found in the graph.");

            gNode = nodeList[index];
            foreach (GraphEdge<N, E> edge in gNode.EdgesIn)
            {
                yield return edge.From;
            }
        }

        /// <summary>
        /// Iterator over the emanating edges from a node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>An enumerator of nodes.</returns>
        public IEnumerable<IEdge<N, E>> OutEdges(N node)
        {
            GraphNode<N, E> gNode;
            int index = vertexList.IndexOf(node);
            if (index == -1)
                throw new ArgumentException("The specified node or vertex was not found in the graph.");

            gNode = nodeList[index];
            foreach (GraphEdge<N, E> edge in gNode.EdgesOut)
            {
                yield return edge;
            }
        }

        /// <summary>
        /// Iterator over the in-coming edges of a node.
        /// </summary>
        /// <remarks>May not be supported by all graphs.</remarks>
        /// <param name="node">The node.</param>
        /// <returns>An enumerator of edges.</returns>
        public IEnumerable<IEdge<N, E>> InEdges(N node)
        {
            GraphNode<N, E> gNode;
            int index = vertexList.IndexOf(node);
            if (index == -1)
                throw new ArgumentException("The specified node or vertex was not found in the graph.");

            gNode = nodeList[index];
            foreach (GraphEdge<N, E> edge in gNode.EdgesIn)
            {
                yield return edge;
            }
        }

        /// <summary>
        /// Iterator for the edges in the graph, yielding IEdge's
        /// </summary>
        public IEnumerable<IEdge<N, E>> Edges
        {
            get
            {
                foreach (GraphNode<N, E> node in nodeList)
                {
                    foreach (GraphEdge<N, E> edge in node.EdgesOut)
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
        public bool ContainsEdge(in N fromNode, in N toNode)
        {
            bool contains = false;
            foreach (N node in Neighbors(fromNode))
            {
                if (node.Equals(toNode))
                {
                    contains = true;
                    break;
                }
            }
            return contains;
        }

        /// <summary>
        /// Gets the label on an edge.
        /// </summary>
        /// <param name="fromNode">The node that the edge emanates from.</param>
        /// <param name="toNode">The node that the edge terminates at.</param>
        /// <returns>The edge.</returns>
        public E GetEdgeLabel(in N fromNode, in N toNode)
        {
            E edge = default(E);
            TryGetEdge(fromNode, toNode, out edge);
            return edge;
        }

        /// <summary>
        /// Exception safe routine to get the label on an edge.
        /// </summary>
        /// <param name="fromNode">The node that the edge emanates from.</param>
        /// <param name="toNode">The node that the edge terminates at.</param>
        /// <param name="edge">The resulting edge if the method was successful. A default
        /// value for the type if the edge could not be found.</param>
        /// <returns>True if the edge was found. False otherwise.</returns>
        public bool TryGetEdge(in N fromNode, in N toNode, out E edge)
        {
            GraphNode<N, E> node;
            int index = vertexList.IndexOf(fromNode);
            if (index == -1)
                throw new ArgumentException("The specified node or vertex was not found in the graph.");

            node = nodeList[index];
            bool contains = false;
            edge = default(E);
            foreach (GraphEdge<N, E> currentEdge in node.EdgesOut)
            {
                if (currentEdge.To.Equals(toNode))
                {
                    contains = true;
                    edge = currentEdge.Value;
                    break;
                }
            }
            return contains;
        }
        #endregion

        #region Member variables
        private static readonly int InitialGraphSize = 30;
        private readonly IList<N> vertexList = new List<N>(InitialGraphSize);
        private readonly IList<GraphNode<N, E>> nodeList = new List<GraphNode<N, E>>(InitialGraphSize);
        #endregion
    }
}
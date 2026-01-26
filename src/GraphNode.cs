using System.Collections.Generic;

namespace CrawfisSoftware.Collections.Graph
{
    internal sealed class GraphNode<VertexDataType, EdgeDataType>
    {
        internal GraphNode(VertexDataType data)
        {
            nodeValue = data;
        }

        internal void AddEdge(GraphEdge<VertexDataType, EdgeDataType> edge)
        {
            if (edge.FromNode == this)
            {
                if (!edgesOut.Contains(edge))
                {
                    edgesOut.Add(edge);
                }
            }
            //
            // Rather than an else-if clause we use a separate if, that way
            //    a node can have an edge to itself.
            //
            if (edge.ToNode == this)
            {
                if (!edgesIn.Contains(edge))
                {
                    edgesIn.Add(edge);
                }
            }
        }

        //internal void RemoveEdge( GraphNode<T> child )
        //{
        //    _children.Remove(child);
        //}

        private VertexDataType nodeValue;
        internal VertexDataType Value
        {
            get { return nodeValue; }
            set { nodeValue = value; }
        }
        // TODO Make this an Abstract Factory.
        private ICollection<GraphEdge<VertexDataType, EdgeDataType>> edgesIn = new List<GraphEdge<VertexDataType, EdgeDataType>>();
        internal IEnumerable<GraphEdge<VertexDataType, EdgeDataType>> EdgesIn
        {
            get { return edgesIn; }
        }
        private ICollection<GraphEdge<VertexDataType, EdgeDataType>> edgesOut = new List<GraphEdge<VertexDataType, EdgeDataType>>();
        internal IEnumerable<GraphEdge<VertexDataType, EdgeDataType>> EdgesOut
        {
            get { return edgesOut; }
        }
        private Graph<VertexDataType, EdgeDataType> owner;
        internal Graph<VertexDataType, EdgeDataType> Owner
        {
            get { return owner; }
            set { owner = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace OhioState.Collections.Graph
{
    internal class GraphEdge<N, E> : IEdge<N,E>
    {
        public GraphEdge( GraphNode<N, E> fromVertex, GraphNode<N, E> toVertex, E edgeData )
        {
            fromNode = fromVertex;
            toNode = toVertex;
            edgeValue = edgeData;
        }
        private GraphNode<N, E> fromNode;
        internal GraphNode<N, E> FromNode
        {
            get { return fromNode; }
        }
        private GraphNode<N, E> toNode;
        internal GraphNode<N, E> ToNode
        {
            get { return toNode; }
        }
        #region IEdge
        public N From
        {
            get { return fromNode.Value; }
        }
        public N To
        {
            get { return toNode.Value; }
        }
        private E edgeValue;
        public E Value
        {
            get { return edgeValue; }
            set { edgeValue = value; }
        }
        #endregion
    }
}

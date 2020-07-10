using System;
using System.Collections.Generic;

namespace CrawfisSoftware.Collections.Graph
{
    public class GraphBuilder : GraphBuilder<object, object>
    {
        public void AddEdge(int from, int to)
        {
            AddEdge(from, to, null);
        }

    }
    public class GraphBuilder<N, E>
    {
        private int _defaultDegree = 2;
        public int DefaultDegree
        {
            get { return _defaultDegree; }
            set { _defaultDegree = value; }
        }

        private static int _defaultSize = 30;
        public static int DefaultSize
        {
            get { return _defaultSize; }
            set { _defaultSize = value; }
        }

        private N _defaultNodeValue;
        public N DefaultNodeValue
        {
            get { return _defaultNodeValue; }
            set { _defaultNodeValue = value; }
        }
        private IList<ICollection<int>> _adjacencyLists = new List<ICollection<int>>(DefaultSize);
        public void AddNode()
        {
            AddNode(_defaultNodeValue);
        }

        // TODO: Switch this over to a null collection object for graphs that do not want this data.
        private IList<N> _nodeValues = new List<N>();
        public void AddNode(N nodeValue)
        {
            _adjacencyLists.Add(new List<int>(DefaultDegree));
            _nodeValues.Add(nodeValue);
        }

        public void AddEdge(int from, int to, E edgeValue)
        {
            ValidateNode(from);
            ValidateNode(to);

            if (_adjacencyLists[from].Contains(to))
                throw new ArgumentException("The specified edge already exists.");

            _adjacencyLists[from].Add(to);
        }

        internal void ValidateNode(int node)
        {
            if (node < 0 || node > _adjacencyLists.Count)
                throw new ArgumentOutOfRangeException("The specified node does not exist.");
        }

        /// <summary>
        /// Extract an <typeparamref name="IGraph{int,int}"/> from the builder.
        /// </summary>
        /// <returns>An <typeparamref name="IGraph{int,int}"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public IGraph<int, int> GetGraph()
        {
            return new SimpleGraph(_adjacencyLists);
        }

        /// <summary>
        /// Reduce the storage needed to represent the current state of the graph.
        /// </summary>
        public void Optimize()
        {
            foreach (ICollection<int> list in _adjacencyLists)
            {
                ((List<int>)list).TrimExcess();
            }
            ((List<ICollection<int>>)_adjacencyLists).TrimExcess();
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

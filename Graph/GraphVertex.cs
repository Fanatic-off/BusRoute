using System.Collections.Generic;

namespace BusRoute
{
    internal class GraphVertex
    {
        public int Name { get; }
        public List<GraphEdge> Edges { get; }

        public GraphVertex(int vertexName)
        {
            Name = vertexName;
            Edges = new List<GraphEdge>();
        }

        public void AddEdge(GraphEdge newEdge)
        {
            Edges.Add(newEdge);
        }

        public void AddEdge(GraphVertex vertex, int edgeWeight)
        {
            AddEdge(new GraphEdge(vertex, edgeWeight));
        }

        public override string ToString() => Name.ToString();
    }
}

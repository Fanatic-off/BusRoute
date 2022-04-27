using System.Collections.Generic;

namespace BusRoute
{
    internal class Vertex
    {
        public int Name { get; }
        public List<Edge> Edges { get; }

        public Vertex(int vertexName)
        {
            Name = vertexName;
            Edges = new List<Edge>();
        }

        public void AddEdge(Edge newEdge)
        {
            Edges.Add(newEdge);
        }

        public void AddEdge(Vertex vertex, int edgeWeight)
        {
            AddEdge(new Edge(vertex, edgeWeight));
        }

        public override string ToString() => Name.ToString();
    }
}

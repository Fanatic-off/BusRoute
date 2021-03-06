using System.Collections.Generic;

namespace BusRoute
{
    internal class Graph
    {
        public List<Vertex> Vertices { get; }

        public Graph()
        {
            Vertices = new List<Vertex>();
        }

        public void AddVertex(int vertexName)
        {
            Vertices.Add(new Vertex(vertexName));
        }

        public Vertex? FindVertexByName(int vertexName)
        {
            foreach (var v in Vertices)
            {
                if (v.Name.Equals(vertexName))
                {
                    return v;
                }
            }

            return null;
        }

        public void AddEdge(int firstName, int secondName, int weight)
        {
            var v1 = FindVertexByName(firstName);
            var v2 = FindVertexByName(secondName);
            if (v2 != null && v1 != null)
                v1.AddEdge(v2, weight);
        }
    }
}

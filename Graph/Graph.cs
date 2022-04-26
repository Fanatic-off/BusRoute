using System.Collections.Generic;

namespace BusRoute
{
    internal class Graph
    {
        public List<GraphVertex> Vertices { get; }

        public Graph()
        {
            Vertices = new List<GraphVertex>();
        }

        public void AddVertex(int vertexName)
        {
            Vertices.Add(new GraphVertex(vertexName));
        }

        public GraphVertex? FindVertex(int vertexName)
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
            var v1 = FindVertex(firstName);
            var v2 = FindVertex(secondName);
            if (v2 != null && v1 != null)
                v1.AddEdge(v2, weight);
        }
    }
}

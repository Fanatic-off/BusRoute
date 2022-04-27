namespace BusRoute
{
    internal class Edge
    {
        public Vertex ConnectedVertex { get; }
        public int EdgeWeight { get; }

        public Edge(Vertex connectedVertex, int weight)
        {
            ConnectedVertex = connectedVertex;
            EdgeWeight = weight;
        }
    }
}

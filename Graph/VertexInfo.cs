namespace BusRoute
{
    internal class VertexInfo
    {
        public Vertex Vertex { get; set; }
        public bool IsUnvisited { get; set; }
        public int EdgesWeightSum { get; set; }
        public Vertex? PreviousVertex { get; set; }

        public VertexInfo(Vertex vertex)
        {
            Vertex = vertex;
            IsUnvisited = true;
            EdgesWeightSum = int.MaxValue;
            PreviousVertex = null;
        }
    }
}

using System.Collections.Generic;

namespace BusRoute
{
    internal class Dijkstra
    {
        private readonly Graph _graph;
        List<VertexInfo> Infos { get; set; } = new List<VertexInfo>();

        public Dijkstra(Graph graph)
        {
            _graph = graph;
        }

        void Init()
        {
            Infos = new List<VertexInfo>();
            foreach (var v in _graph.Vertices)
            {
                Infos.Add(new VertexInfo(v));
            }
        }

        VertexInfo GetVertexInfo(Vertex v)
        {
            foreach (var i in Infos)
                if (i.Vertex.Equals(v))
                    return i;

            return null;
        }

        public VertexInfo? FindUnvisitedVertexWithMinSum()
        {
            var minValue = int.MaxValue;
            VertexInfo? minVertexInfo = null;
            foreach (var i in Infos)
            {
                if (i.IsUnvisited && i.EdgesWeightSum < minValue)
                {
                    minVertexInfo = i;
                    minValue = i.EdgesWeightSum;
                }
            }

            return minVertexInfo;
        }

        public string FindShortestPath(int startName, int finishName)
        {
            return FindShortestPath(_graph.FindVertex(startName), _graph.FindVertex(finishName));
        }

        public string FindShortestPath(Vertex startVertex, Vertex finishVertex)
        {
            Init();
            var first = GetVertexInfo(startVertex);
            first.EdgesWeightSum = 0;
            while (true)
            {
                var current = FindUnvisitedVertexWithMinSum();
                if (current == null)
                    break;

                SetSumToNextVertex(current);
            }

            return GetPath(startVertex, finishVertex);
        }

        void SetSumToNextVertex(VertexInfo info)
        {
            info.IsUnvisited = false;
            foreach (var e in info.Vertex.Edges)
            {
                var nextInfo = GetVertexInfo(e.ConnectedVertex);
                var sum = info.EdgesWeightSum + e.EdgeWeight;
                if (sum < nextInfo.EdgesWeightSum)
                {
                    nextInfo.EdgesWeightSum = sum;
                    nextInfo.PreviousVertex = info.Vertex;
                }
            }
        }

        string GetPath(Vertex startVertex, Vertex endVertex)
        {
            var path = endVertex.ToString() + "= " + GetVertexInfo(endVertex).EdgesWeightSum;
            while (startVertex != endVertex)
            {
                endVertex = GetVertexInfo(endVertex).PreviousVertex;
                if (endVertex is null) return "";
                path = endVertex.ToString() + "⇒ " + path;
            }

            return path;
        }
    }
}

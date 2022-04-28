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
            return FindShortestPath(_graph.FindVertexByName(startName), _graph.FindVertexByName(finishName));
        }

        public string FindShortestPath(Vertex start, Vertex finish)
        {
            Init();
            var first = GetVertexInfo(start);
            first.EdgesWeightSum = 0;
            while (true)
            {
                var current = FindUnvisitedVertexWithMinSum();
                if (current == null)
                    break;

                SetSumToNextVertex(current);
            }

            return GetPath(start, finish);
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

        string GetPath(Vertex start, Vertex end)
        {
            var path = end.ToString() + "= " + GetVertexInfo(end).EdgesWeightSum;
            while (start != end)
            {
                end = GetVertexInfo(end).PreviousVertex;
                if (end is null) return "";
                path = end.ToString() + "⇒ " + path;
            }

            return path;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace BusRoute
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Graph Graph = new Graph();
        public int StartPoint { get; set; }
        public int EndPoint { get; set; }
        public int StartTime { get; set; }
        public int BusCount { get; set; }
        public int StationCount { get; set; }
        public List<Bus> Buses = new List<Bus>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ButtonClick(object sender, RoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
                using (var stream = new StreamReader(ofd.OpenFile()))
                {
                    var data = new List<string>();

                    while (true)
                    {
                        var line = stream.ReadLine();
                        if (line == null) break;
                        data.Add(line);
                    }

                    BusCount = Convert.ToInt32(data[0]);
                    StationCount = Convert.ToInt32(data[1]);

                    var busesStarts = data[2].Split(' ')
                        .Select(x => Convert.ToInt32(Convert.ToDateTime(x).Hour * Consts.MinutesInHour + Convert.ToDateTime(x).Minute))
                        .ToList();
                    var costs = data[3].Split(' ').Select(x => Convert.ToInt32(x)).ToList();

                    for (var i = 0; i < BusCount; i++)
                    {
                        Buses.Add(new Bus(busesStarts[i], costs[i]));
                    }

                    for (var i = Consts.MinRowInFile; i < BusCount + Consts.MinRowInFile; i++)
                    {
                        var moves = data[i].Split(' ');
                        var count = Convert.ToInt32(moves[0]);
                        var busStops = new List<int>();
                        var travelTime = new List<int>();

                        for (var j = 1; j < count + 1; j++)
                        {
                            busStops.Add(Convert.ToInt32(moves[j]));
                        }
                        for (var j = count + 1; j < moves.Count(); j++)
                        {
                            travelTime.Add(Convert.ToInt32(moves[j]));
                        }
                        for (var j = 0; j < count - 1; j++)
                        {
                            Buses.ElementAt(i - Consts.MinRowInFile).Slings.Add(new Sprint(busStops[j], busStops[j + 1], travelTime[j]));
                        }
                        Buses.ElementAt(i - Consts.MinRowInFile).Slings.Add(new Sprint(busStops.Last(), busStops[0], travelTime.Last()));

                        Buses.ElementAt(i - Consts.MinRowInFile).Stops = moves.Skip(1).Take(int.Parse(moves[0])).Select(s => int.Parse(s)).ToArray();
                        Buses.ElementAt(i - Consts.MinRowInFile).Times = moves.Skip(1 + int.Parse(moves[0])).Select(s => int.Parse(s)).ToArray();
                    }
                }
            }
        }

        private async void CalculateButtonClick(object sender, RoutedEventArgs e)
        {
            var cost = 0;
            Graph = new();
            var startPoint = Convert.ToInt32(StartText.Text);
            var endPoint = Convert.ToInt32(EndText.Text);
            var startTime = Convert.ToInt32(Convert.ToDateTime(StartTimeText.Text).Hour * Consts.MinutesInHour + Convert.ToDateTime(StartTimeText.Text).Minute);


            var buses = Buses.Where(b => b.CheckBusRoute(startPoint, endPoint) == true).ToList();

            if (buses.Any())
            {
                cost = buses.Where(b => b.IsHasDirectRoute(startPoint, endPoint) == true).FirstOrDefault() is not null ?
                    buses.Where(b => b.IsHasDirectRoute(startPoint, endPoint) == true).Min(b => b.Costs) : 0;

                int StartStop = startPoint, FinalStop = endPoint;

                for (int i = 1; i < 5; i++)
                    Graph.AddVertex(i);
                AddTimeEdge(startPoint, startTime);

                var dijkstra = new Dijkstra(Graph).FindShortestPath(StartStop, FinalStop);
                if (dijkstra == "")
                {
                    MessageBox.Show("Пути не существует");
                }
                else
                {
                    var s = dijkstra.Replace("=", "\nВремя в пути: ");
                    MessageBox.Show("Cамый быстрый путь: \n" + s);
                }

                Graph = new();
                for (int i = 1; i < 5; i++)
                    Graph.AddVertex(i);
                AddMoneyEdge(StartStop, startTime);

                dijkstra = new Dijkstra(Graph).FindShortestPath(StartStop, FinalStop);
                if (dijkstra == "")
                    MessageBox.Show("Пути не существует");
                else
                {
                    var s = dijkstra.Replace("=", "\nСтоимость: ");
                    MessageBox.Show("Cамый дешевый путь: \n" + s);
                }
            }
        }

        void AddTimeEdge(int stop, int time)
        {
            var list = Buses.Where(x => x.Stops.Contains(stop)).ToList();
            foreach (var x in list)
            {
                int nextstop = x.GetNextStop(stop);
                var tmp = Graph.Vertices.FirstOrDefault(y => y.Name == stop);

                int nexttime = x.GetTime(time, stop, nextstop);
                if (nexttime == -1) continue;
                if (tmp is not null
                 && tmp.Edges.Any(y => (y.ConnectedVertex.Name == nextstop) && (y.EdgeWeight <= nexttime)))
                    return;
                Graph.AddEdge(stop, nextstop, nexttime);
                AddTimeEdge(nextstop, time + nexttime);
            }
        }

        void AddMoneyEdge(int stop, int time)
        {
            var list = Buses.Where(x => x.Stops.Contains(stop)).ToList();
            foreach (var x in list)
            {
                var tmp = Graph.Vertices.FirstOrDefault(y => y.Name == stop);
                List<int> temptime = new();
                foreach (var s in x.Stops)
                {
                    int tm = x.GetTime(time, stop, s);
                    if (tmp is not null && tmp.Edges.Any(y => y.ConnectedVertex.Name == s))
                        tm = -1;
                    temptime.Add(tm);
                    if (s != stop && tm != -1)
                        Graph.AddEdge(stop, s, x.Costs);
                }
                for (int i = 0; i < x.Stops.Length; i++)
                {
                    if (x.Stops[i] != stop && temptime[i] != -1)
                        AddMoneyEdge(x.Stops[i], time + temptime[i]);
                }
            }
        }
    }
}

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
        Graph Graph = new();
        public List<Bus> Buses { get; set; } = new List<Bus>();
        public int StartPoint { get; set; }
        public int EndPoint { get; set; }
        public int StartTime { get; set; }
        public int BusCount { get; set; }
        public int StationCount { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
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

        private void CalculateButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ParseValue();
                CalculateGraph();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"SomeThings going wrong:\n{ex.Message}");
            }
        }

        private void CalculateGraph()
        {
            var buses = Buses.Where(b => b.CheckBusRoute(StartPoint, EndPoint) == true).ToList();

            if (buses.Where(b => b.Stops.Contains(StartPoint) == true).Any()
                && buses.Where(b => b.Stops.Contains(EndPoint) == true).Any())
            {
                CreateGraphs();
            }
            else
            {
                MessageBox.Show("Пути не существует");
            }
        }

        private void ParseValue()
        {
            StartPoint = Convert.ToInt32(StartText.Text);
            EndPoint = Convert.ToInt32(EndText.Text);

            if(!string.IsNullOrWhiteSpace( StartTimeText.Text))
            {
                StartTime = Convert.ToInt32(Convert.ToDateTime(StartTimeText.Text).Hour * Consts.MinutesInHour + Convert.ToDateTime(StartTimeText.Text).Minute);
            }
            else
            {
                StartTime = Convert.ToInt32(Convert.ToDateTime(DateTime.Now).Hour * Consts.MinutesInHour + Convert.ToDateTime(DateTime.Now).Minute);
            }
        }

        private void CreateGraphs()
        {
            for (int i = 1; i < 5; i++)
                Graph.AddVertex(i);
            AddTimeEdge(StartPoint, StartTime);

            var dijkstra = new Dijkstra(Graph).FindShortestPath(StartPoint, EndPoint);
            if (dijkstra != "")
            {
                var s = dijkstra.Replace("=", "\nВремя в пути: ");
                MessageBox.Show("Cамый быстрый путь: \n" + s);
            }

            Graph = new Graph();
            for (int i = 1; i < 5; i++)
                Graph.AddVertex(i);
            AddMoneyEdge(StartPoint, StartTime);

            dijkstra = new Dijkstra(Graph).FindShortestPath(StartPoint, EndPoint);
            if (dijkstra != "")
            {
                var s = dijkstra.Replace("=", "\nСтоимость: ");
                MessageBox.Show("Cамый дешевый путь: \n" + s);
            }
        }

        void AddTimeEdge(int stop, int time)
        {
            var buses = Buses.Where(b => b.Stops.Contains(stop)).ToList();
            foreach (var bus in buses)
            {
                int nextStop = bus.GetNextStop(stop);
                var vertex = Graph.Vertices.FirstOrDefault(v => v.Name == stop);

                int nextTime = bus.GetTime(time, stop, nextStop);
                if (nextTime == -1) continue;
                if (vertex is not null
                 && vertex.Edges.Any(e => (e.ConnectedVertex.Name == nextStop) && (e.EdgeWeight <= nextTime)))
                    return;
                Graph.AddEdge(stop, nextStop, nextTime);
                AddTimeEdge(nextStop, time + nextTime);
            }
        }

        void AddMoneyEdge(int stop, int time)
        {
            var buses = Buses.Where(x => x.Stops.Contains(stop)).ToList();
            foreach (var bus in buses)
            {
                var vertex = Graph.Vertices.FirstOrDefault(v => v.Name == stop);
                List<int> tempTime = new();
                foreach (var s in bus.Stops)
                {
                    int timeBusOnStop = bus.GetTime(time, stop, s);
                    if (vertex is not null && vertex.Edges.Any(e => e.ConnectedVertex.Name == s))
                        timeBusOnStop = -1;
                    tempTime.Add(timeBusOnStop);
                    if (s != stop && timeBusOnStop != -1)
                        Graph.AddEdge(stop, s, bus.Costs);
                }
                for (var i = 0; i < bus.Stops.Length; i++)
                {
                    if (bus.Stops[i] != stop && tempTime[i] != -1)
                        AddMoneyEdge(bus.Stops[i], time + tempTime[i]);
                }
            }
        }
    }
}

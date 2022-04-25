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
                        Buses.Add(new Bus(i, busesStarts[i], costs[i]));
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

                    }
                }
            }
        }

        public class Bus
        {
            public int Id { get; set; }
            public int Costs { get; set; }
            public int StartTime { get; set; }
            public List<Sprint> Slings { get; set; } = new List<Sprint>();

            public Bus(int id, int startTime, int costs)
            {
                Id = id;
                Costs = costs;
                StartTime = startTime;
            }
        }

        public class Sprint
        {
            public int StationFrom { get; set; }
            public int StationTo { get; set; }
            public int TravelTime { get; set; }

            public Sprint(int stationFrom, int stationTo, int travelTime)
            {
                StationFrom = stationFrom;
                StationTo = stationTo;
                TravelTime = travelTime;
            }
        }
    }
}

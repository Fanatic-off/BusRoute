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
                }
            }
        }

        public class Bus
        {
            public int Id { get; set; }
            public int Costs { get; set; }
            public int StartTime { get; set; }

            public Bus(int id, int startTime, int costs)
            {
                Id = id;
                Costs = costs;
                StartTime = startTime;
            }
        }
    }
}

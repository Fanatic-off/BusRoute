using System.Collections.Generic;
using System.Linq;
using static BusRoute.MainWindow;

namespace BusRoute
{
    public class Bus
    {
        public int Costs { get; set; }
        public int StartTime { get; set; }
        public int[] Times { get; set; }
        public int[] Stops { get; set; }
        public List<Sprint> Slings { get; set; } = new List<Sprint>();

        public Bus(int startTime, int costs)
        {
            Costs = costs;
            StartTime = startTime;
        }

        public bool CheckBusRoute(int start, int end)
        {
            return Slings.Where(s => s.FromStation == start).Any() 
                || Slings.Where(s => s.ToStation == end).Any();
        }

        public bool IsHasDirectRoute(int start, int end)
        {
            return Slings.Where(s => s.FromStation == start).Any() 
                && Slings.Where(s => s.ToStation == end).Any();
        }

        public int GetTime(int currentTime, int start, int destination)
        {
            if (!Stops.Contains(destination) || !Stops.Contains(start)) return -1;
            int time = StartTime;
            int idx = Stops.ToList().IndexOf(destination);
            int count = Stops.Length;
            bool visited = false;
            for (int i = 0; time < 1440 && (!visited || i % count != idx || currentTime > time); i++)
            {
                if (Stops[i % count] == start && time >= currentTime)
                    visited = true;
                time += Times[i % count];
            }
            if (time >= 1440) return -1;
            return time - currentTime;
        }

        public int GetNextStop(int current) => Stops[(Stops.ToList().IndexOf(current) + 1) % Stops.Length];
    }
}

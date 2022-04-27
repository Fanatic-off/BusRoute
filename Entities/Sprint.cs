namespace BusRoute
{
    public class Sprint
    {
        public int FromStation { get; set; }
        public int ToStation { get; set; }
        public int TravelTime { get; set; }

        public Sprint(int stationFrom, int stationTo, int travelTime)
        {
            FromStation = stationFrom;
            ToStation = stationTo;
            TravelTime = travelTime;
        }
    }
}

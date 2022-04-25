namespace BusRoute
{
    internal class Consts
    {
        public const int MinutesInHour = 60;
        public const int HoursInDay = 24;
        public static string GetTimeFromNimutes(int minutes)
        {
            return minutes / MinutesInHour + ":" + minutes % MinutesInHour;
        }
    }
}

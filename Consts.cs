namespace BusRoute
{
    internal class Consts
    {
        public const int MinRowInFile = 4;
        public const int MinutesInHour = 60;
        public const int HoursInDay = 24;
        public static string GetTimeFromMimutes(int minutes)
        {
            return minutes / MinutesInHour + ":" + minutes % MinutesInHour;
        }
    }
}

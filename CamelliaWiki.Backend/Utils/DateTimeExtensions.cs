namespace CamelliaWiki.Backend.Utils;

public static class DateTimeExtensions
{
    public static DateTime StartOfDay(this DateTimeOffset date) => date.Date.Date;
}

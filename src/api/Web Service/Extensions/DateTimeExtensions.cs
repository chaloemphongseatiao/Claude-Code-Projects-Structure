namespace TemplateWebService.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(
            this DateTime dt,
            DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        public static int MonthDifference(
            this DateTime lValue,
            DateTime rValue)
        {
            return Math.Abs((lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year));
        }
    }
}
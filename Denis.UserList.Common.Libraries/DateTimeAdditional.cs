namespace Denis.UserList.Common.Libraries
{
    public class DateTimeAdditional
    {
        public static int CompleteYearDifference(DateTime startDate, DateTime endDate)
        {
            if (endDate.DayOfYear < startDate.DayOfYear)
            {
                return endDate.Year - startDate.Year - 1;
            }
            return endDate.Year - startDate.Year;
        }
    }
}
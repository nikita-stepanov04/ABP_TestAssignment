namespace ABP_TestAssignment.Application.BusinessServices.Reports
{
    public class ReportBaseBS
    {
        protected static IEnumerable<DateOnly> EnumerateDays(DateOnly periodStart, DateOnly periodEnd)
        {
            for (var day = periodStart; day <= periodEnd; day = day.AddDays(1))
                yield return day;
        }
    }
}

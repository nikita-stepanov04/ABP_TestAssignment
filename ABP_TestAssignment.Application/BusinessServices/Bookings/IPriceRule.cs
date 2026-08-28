using ABP_TestAssignment.Domain.Entities.Rooms;

namespace ABP_TestAssignment.Application.BusinessServices.Bookings
{
    public interface IPriceRule
    {
        int Priority { get; }
        int RuleCode { get; }
        string Message { get; }
        bool IsApplicable(Room room, DateTime slotStart, DateTime slotEnd);
        decimal Apply(Room room, DateTime slotStart, DateTime slotEnd);
    }
}

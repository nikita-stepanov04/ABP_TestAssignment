using ABP_TestAssignment.Domain.Entities.Rooms;

namespace ABP_TestAssignment.Application.BusinessServices.Bookings.PricingRules
{
    public class StandardPriceRule : IPriceRule
    {
        public int Priority => 0;
        public int RuleCode => 1;
        public string Message => "Standard hours (09:00–18:00): base price";

        public bool IsApplicable(Room room, DateTime slotStart, DateTime slotEnd)
            => slotStart.TimeOfDay >= TimeSpan.FromHours(9)
            && slotEnd.TimeOfDay <= TimeSpan.FromHours(18);

        public decimal Apply(Room room, DateTime slotStart, DateTime slotEnd)
            => room.BasePricePerHour * (decimal)(slotEnd - slotStart).TotalHours;
    }
}

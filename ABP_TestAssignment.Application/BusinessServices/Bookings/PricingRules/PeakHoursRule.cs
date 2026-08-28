using ABP_TestAssignment.Domain.Entities.Rooms;

namespace ABP_TestAssignment.Application.BusinessServices.Bookings.PricingRules
{
    public class PeakSurchargeRule : IPriceRule
    {
        public int Priority => 10;
        public int RuleCode => 4;
        public string Message => "Peak hours (12:00–14:00): 15% surcharge";

        public bool IsApplicable(Room room, DateTime slotStart, DateTime slotEnd)
            => slotStart.TimeOfDay >= TimeSpan.FromHours(12)
            && slotEnd.TimeOfDay <= TimeSpan.FromHours(14);

        public decimal Apply(Room room, DateTime slotStart, DateTime slotEnd)
            => room.BasePricePerHour * 1.15m * (decimal)(slotEnd - slotStart).TotalHours;
    }
}

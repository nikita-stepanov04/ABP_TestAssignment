using ABP_TestAssignment.Domain.Entities.Rooms;

namespace ABP_TestAssignment.Application.BusinessServices.Bookings.PricingRules
{
    public class MorningDiscountRule : IPriceRule
    {
        public int Priority => 1;
        public int RuleCode => 3;
        public string Message => "Morning hours (06:00–09:00): 10% discount";

        public bool IsApplicable(Room room, DateTime slotStart, DateTime slotEnd)
            => slotStart.TimeOfDay >= TimeSpan.FromHours(6)
            && slotEnd.TimeOfDay <= TimeSpan.FromHours(9);

        public decimal Apply(Room room, DateTime slotStart, DateTime slotEnd)
            => room.BasePricePerHour * 0.9m * (decimal)(slotEnd - slotStart).TotalHours;
    }
}

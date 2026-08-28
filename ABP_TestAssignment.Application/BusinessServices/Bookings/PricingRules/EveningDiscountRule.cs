using ABP_TestAssignment.Domain.Entities.Rooms;

namespace ABP_TestAssignment.Application.BusinessServices.Bookings.PricingRules
{
    public class EveningDiscountRule : IPriceRule
    {
        public int Priority => 1;
        public int RuleCode => 2;
        public string Message => "Evening hours (18:00–23:00): 20% discount";

        public bool IsApplicable(Room room, DateTime slotStart, DateTime slotEnd)
            => slotStart.TimeOfDay >= TimeSpan.FromHours(18)
            && slotEnd.TimeOfDay <= TimeSpan.FromHours(23);

        public decimal Apply(Room room, DateTime slotStart, DateTime slotEnd)
            => room.BasePricePerHour * 0.8m * (decimal)(slotEnd - slotStart).TotalHours;
    }
}

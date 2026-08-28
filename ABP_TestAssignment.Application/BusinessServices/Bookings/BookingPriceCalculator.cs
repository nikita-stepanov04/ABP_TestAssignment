using ABP_TestAssignment.Application.DTOs.Bookings.Pricing;
using ABP_TestAssignment.Domain.Entities.Bookings;
using ABP_TestAssignment.Domain.Entities.Rooms;

namespace ABP_TestAssignment.Application.BusinessServices.Bookings
{
    public class BookingPriceCalculator
    {
        private readonly List<IPriceRule> _rules;

        public BookingPriceCalculator(IEnumerable<IPriceRule> rules)
        {
            _rules = rules.OrderByDescending(r => r.Priority).ToList();
        }
        
        public PriceCalculationDTO Calculate(Room room, Booking booking)
        {
            decimal roomRentTotal = 0;
            decimal servicesTotal = 0;

            var slices = SplitIntoSlices(booking.BookingStartTime, booking.BookingEndTime);
            var appliedRules = new List<PricingRuleDTO>();

            // Calculate the price for each time slice
            foreach (var (start, end) in slices)
            {
                var rule = _rules.FirstOrDefault(r => r.IsApplicable(room, start, end));

                if (rule is null)
                    throw new InvalidOperationException(
                        $"No price rule covers interval {start:t}-{end:t}");

                var cost = rule.Apply(room, start, end);
                roomRentTotal += cost;

                if (!appliedRules.Any(r => r.RuleCode == rule.RuleCode))
                {
                    appliedRules.Add(new PricingRuleDTO
                    {
                        RuleCode = rule.RuleCode,
                        Message = rule.Message
                    });
                }
            }

            if (booking.Services is not null && booking.Services.Count > 0)
            {
                servicesTotal += booking.Services.Sum(s => s.Price);
            }

            return new PriceCalculationDTO
            {
                RoomRentalPriceTotal = roomRentTotal,
                ServicesPriceTotal = servicesTotal,
                Total = roomRentTotal + servicesTotal,
                AppliedPricingRules = appliedRules.ToList()
            };
        }

        // Split the chosen booking time span to parts for price calculation for each
        private List<(DateTime start, DateTime end)> SplitIntoSlices(DateTime start, DateTime end)
        {
            var slices = new List<(DateTime, DateTime)>();
            var cursor = start;

            while (cursor < end)
            {
                var nextHourMark = cursor.Date.AddHours(cursor.Hour + 1);

                var sliceEnd = nextHourMark < end ? nextHourMark : end;

                slices.Add((cursor, sliceEnd));
                cursor = sliceEnd;
            }

            return slices;
        }
    }
}

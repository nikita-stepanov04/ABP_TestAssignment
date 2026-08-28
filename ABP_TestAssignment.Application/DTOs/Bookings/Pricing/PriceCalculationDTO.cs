namespace ABP_TestAssignment.Application.DTOs.Bookings.Pricing
{
    public class PriceCalculationDTO
    {
        public decimal RoomRentalPriceTotal { get; set; }
        public decimal ServicesPriceTotal { get; set; }
        public decimal Total { get; set; }
        public List<PricingRuleDTO> AppliedPricingRules { get; set; } = [];
    }
}

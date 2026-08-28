using ABP_TestAssignment.Application.DTOs.Bookings;
using ABP_TestAssignment.Application.DTOs.Bookings.Pricing;

namespace ABP_TestAssignment.Application.IBusinessServices.Bookings
{
    public interface IBookingBS
    {
        Task<OpRes<PriceCalculationDTO>> BookRoomAsync(BookRoomDTO dto, long companyID);
        Task<List<BookingDTO>> GetAllBookingsForCompanyAsync(long companyID);
    }
}

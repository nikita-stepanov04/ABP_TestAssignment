using ABP_TestAssignment.Application.DTOs.Bookings;
using ABP_TestAssignment.Application.DTOs.Bookings.Pricing;
using ABP_TestAssignment.Application.IBusinessServices.Bookings;
using ABP_TestAssignment.Domain.Entities.Bookings;
using ABP_TestAssignment.Domain.Entities.Services;
using ABP_TestAssignment.Infrastructure.IRepositories.Bookings;
using ABP_TestAssignment.Infrastructure.IRepositories.Companies;
using ABP_TestAssignment.Infrastructure.IRepositories.Rooms;
using AutoMapper;

namespace ABP_TestAssignment.Application.BusinessServices.Bookings
{
    public class BookingBS : IBookingBS
    {
        private readonly IMapper _mapper;
        private readonly IRoomRepository _roomRep;
        private readonly ICompanyRepository _companyRep;
        private readonly IBookingRepository _bookingRep;
        private readonly BookingPriceCalculator _pricingCalculator;

        public BookingBS(
            IMapper mapper,
            IRoomRepository roomRep,
            ICompanyRepository companyRep,
            IBookingRepository bookingRep,
            BookingPriceCalculator pricingCalculator)
        {
            _mapper = mapper;
            _roomRep = roomRep;
            _companyRep = companyRep;
            _bookingRep = bookingRep;
            _pricingCalculator = pricingCalculator;
        }

        public async Task<OpRes<PriceCalculationDTO>> BookRoomAsync(BookRoomDTO dto, long companyID)
        {
            // Check company
            var company = await _companyRep.GetByIDAsync(companyID);
            if (company is null)
            {
                return OpRes.Err<PriceCalculationDTO>("Company was not found");
            }

            await using var transaction = await _roomRep.BeginTransactionAsync();
            try
            {
                // Check room
                var room = await _roomRep.GetByIDForUpdateAsync(dto.RoomID);
                if (room is null)
                {
                    await transaction.RollbackAsync();
                    return OpRes.Err<PriceCalculationDTO>("Room not found");
                }

                // Check if selected services are available for selection
                var selectedServices = new List<Service>();
                var availableServiceIDs = room.AvailableServices.Select(s => s.ID).ToHashSet();
                var unavailableIDs = dto.SelectedServicesIDs
                    .Where(id => !availableServiceIDs.Contains(id))
                    .ToList();

                if (unavailableIDs.Count > 0)
                {
                    await transaction.RollbackAsync();
                    return OpRes.Err<PriceCalculationDTO>(
                        $"The following services are not available for this room: {string.Join(", ", unavailableIDs)}");
                }

                selectedServices = room.AvailableServices
                    .Where(s => dto.SelectedServicesIDs.Contains(s.ID))
                    .ToList();

                // Check if selected time does not overlap the existing books
                if (await _bookingRep.HasOverlapAsync(dto.RoomID, dto.StartTime, dto.EndTime))
                {
                    await transaction.RollbackAsync();
                    return OpRes.Err<PriceCalculationDTO>("Room is already booked for this time period");
                }

                var booking = _mapper.Map<Booking>(dto);
                booking.Company = company;
                booking.Services = selectedServices;
                booking.BookingDate = DateTime.UtcNow;

                // Calculate the price of booking
                var pricingReport = _pricingCalculator.Calculate(room, booking);
                booking.CalculatedTotalPrice = pricingReport.Total;

                await _bookingRep.AddAsync(booking);
                await _bookingRep.SaveChangesAsync();

                await transaction.CommitAsync();
                return OpRes.Success(pricingReport);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}

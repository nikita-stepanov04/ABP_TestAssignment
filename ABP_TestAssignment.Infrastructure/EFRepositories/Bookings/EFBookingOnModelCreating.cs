using ABP_TestAssignment.Domain.Entities.Bookings;
using ABP_TestAssignment.Domain.Entities.Services;
using ABP_TestAssignment.Infrastructure.EFRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABP_TestAssignment.Infrastructure.EFRepositories.Companies
{
    public class EFBookingOnModelCreating : EFOnModelCreatingBase<Booking>
    {
        protected override void OnModelCreating(EntityTypeBuilder<Booking> model)
        {
            model.HasIndex(e => e.BookingStartTime);
            model.Property(e => e.BookingEndTime);

            model.Property(r => r.CalculatedTotalPrice)
                .HasColumnType("numeric(10,2)");

            model.HasMany(b => b.Services)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "BookingService",
                    j => j.HasOne<Service>()
                          .WithMany()
                          .OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne<Booking>()
                          .WithMany()
                          .OnDelete(DeleteBehavior.Cascade)
                );
        }
    }
}

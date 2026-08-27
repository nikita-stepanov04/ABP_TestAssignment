using ABP_TestAssignment.Domain.Entities.Rooms;
using ABP_TestAssignment.Infrastructure.EFRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABP_TestAssignment.Infrastructure.EFRepositories.Services
{
    public class EFRoomOnModelCreating : EFOnModelCreatingBase<Room>
    {
        protected override void OnModelCreating(EntityTypeBuilder<Room> model)
        {
            model.Property(e => e.Name)
                .HasColumnType("varchar(50)");

            model.HasIndex(e => e.Name)
                .IsUnique();
        }
    }
}

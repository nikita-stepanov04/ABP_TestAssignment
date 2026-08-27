using Microsoft.EntityFrameworkCore;

namespace ABP_TestAssignment.Infrastructure.EFRepositories
{
    public class EFDataContext : DbContext
    {
        public EFDataContext(DbContextOptions options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EFDataContext).Assembly);
        }
    }
}

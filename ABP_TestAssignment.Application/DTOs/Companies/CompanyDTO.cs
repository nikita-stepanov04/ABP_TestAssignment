using ABP_TestAssignment.Domain.Entities.Companies;

namespace ABP_TestAssignment.Application.DTOs.Companies
{
    public class CompanyDTO
    {
        public long ID { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public Role Role { get; set; }
    }
}

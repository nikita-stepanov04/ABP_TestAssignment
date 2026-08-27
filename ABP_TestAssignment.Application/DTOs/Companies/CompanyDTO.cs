namespace ABP_TestAssignment.Application.DTOs.Companies
{
    public class CompanyDTO
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsAdmin { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

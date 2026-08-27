namespace ABP_TestAssignment.Domain.Entities.Companies
{
    public class Company : EntityBase
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = null!;
        public bool IsAdmin { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

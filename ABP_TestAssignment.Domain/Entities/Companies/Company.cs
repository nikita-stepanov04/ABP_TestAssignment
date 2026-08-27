namespace ABP_TestAssignment.Domain.Entities.Companies
{
    public class Company : EntityBase
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
        public Role Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

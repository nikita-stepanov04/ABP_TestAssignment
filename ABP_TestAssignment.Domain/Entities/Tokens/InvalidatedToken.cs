namespace ABP_TestAssignment.Domain.Entities.Tokens
{
    public class InvalidatedToken : EntityBase
    {
        public string TokenID { get; set; } = null!;
        public DateTime DateExpiration { get; set; }
    }
}

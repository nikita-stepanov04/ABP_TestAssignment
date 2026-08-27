namespace ABP_TestAssignment.Web.Models
{
    public class RefreshTokenRequest
    {
        public RefreshTokenRequest(string refreshToken)
        {
            RefreshToken = refreshToken;
        }

        public string RefreshToken { get; set; }
    }
}

namespace ABP_TestAssignment.Web.Models
{
    public class RefreshResponse
    {
        public RefreshResponse(string accessToken)
        {
            AccessToken = accessToken;
        }

        public string AccessToken { get; set; }
    }
}

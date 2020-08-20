namespace PerformanceReview.BusinessLogic.Auth.Entities.Models
{
    public class AuthenticateResponse
    {
        public string RoleName { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
    }
}

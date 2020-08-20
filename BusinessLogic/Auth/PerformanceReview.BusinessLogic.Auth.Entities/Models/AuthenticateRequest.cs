using System.ComponentModel.DataAnnotations;

namespace PerformanceReview.BusinessLogic.Auth.Entities.Models
{
    public class AuthenticateRequest
    {
        [Required]
        public string Password { get; set; }

        [Required]
        public string Username { get; set; }
    }
}

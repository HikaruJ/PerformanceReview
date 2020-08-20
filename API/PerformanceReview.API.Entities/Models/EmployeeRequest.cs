using System.ComponentModel.DataAnnotations;

namespace PerformanceReview.API.Entities.Models
{
    public class EmployeeRequest
    {
        [EmailAddress]
        [MaxLength(100)]
        [Required]
        public string Email { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [MaxLength(35)]
        [Required]
        public string FirstName { get; set; }

        [MaxLength(35)]
        [Required]
        public string LastName { get; set; }
    }
}

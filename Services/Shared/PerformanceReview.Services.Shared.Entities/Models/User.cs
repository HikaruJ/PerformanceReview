using Newtonsoft.Json;
using System;

namespace PerformanceReview.Services.Shared.Entities.Models
{
    public class User
    {
        [JsonIgnore]
        public string EncodedKey { get; set; }

        [JsonIgnore]
        public string EncodedSalt { get; set; }

        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace Growtify.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public required string DisplayName { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        [JsonIgnore]
        public Member Member { get; set; } = null!;
    }
}

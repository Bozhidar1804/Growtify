using System.Text.Json.Serialization;

namespace Growtify.Domain.Entities
{
    public class AppUser
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public required byte[] PasswordHash { get; set; }
        public required byte[] PasswordSalt { get; set; }
        [JsonIgnore]
        public Member Member { get; set; } = null!;
    }
}

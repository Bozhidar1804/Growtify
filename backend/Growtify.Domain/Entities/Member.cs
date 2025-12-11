using System.ComponentModel.DataAnnotations.Schema;

namespace Growtify.Domain.Entities
{
    public class Member
    {
        public string Id { get; set; } = null!;
        public required string UserName { get; set; }
        public string? ImageUrl { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public required string Gender { get; set; }
        public string? Description { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }

        [ForeignKey(nameof(Id))]
        public AppUser User { get; set; } = null!;
        public List<Photo> Photos { get; set; } = [];
    }
}

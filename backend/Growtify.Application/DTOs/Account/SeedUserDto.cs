namespace Growtify.Application.DTOs.Account
{
    public class SeedUserDto
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string Gender { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string? Description { get; set; }
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
    }
}

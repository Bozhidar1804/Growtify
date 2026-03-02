using System.ComponentModel.DataAnnotations;

namespace Growtify.Application.DTOs.Account
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MinLength(4)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Gender {  get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string Country { get; set; } = string.Empty;

        [Required]
        public DateOnly DateOfBirth { get; set; }
    }
}

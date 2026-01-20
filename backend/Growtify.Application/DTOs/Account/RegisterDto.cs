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
    }
}

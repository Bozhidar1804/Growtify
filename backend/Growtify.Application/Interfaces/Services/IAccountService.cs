using Growtify.Application.DTOs.Account;

namespace Growtify.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<(UserDto user, string refreshToken)?> RegisterAsync(RegisterDto dto);
        Task<(UserDto user, string refreshToken)?> LoginAsync(LoginDto dto);
        Task<(UserDto user, string refreshToken)?> RefreshTokenAsync(string refreshToken);
    }
}

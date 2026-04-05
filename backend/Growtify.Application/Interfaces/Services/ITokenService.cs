using Growtify.Domain.Entities;

namespace Growtify.Application.Interfaces.Services
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
        string GenerateRefreshToken();
    }
}

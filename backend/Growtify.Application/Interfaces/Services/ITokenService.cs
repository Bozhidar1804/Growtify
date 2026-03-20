using Growtify.Domain.Entities;

namespace Growtify.Application.Interfaces.Services
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}

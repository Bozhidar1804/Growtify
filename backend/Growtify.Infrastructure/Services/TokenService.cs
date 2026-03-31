using Growtify.Application.DTOs.Account;
using Growtify.Application.Interfaces.Services;
using Growtify.Domain.Entities;
using Growtify.Infrastructure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Growtify.Infrastructure.Services
{
    // TokenService is implemented in Infrastructure layer because it uses Microsoft.IdentityModel.Tokens and System.IdentityModel.Tokens.Jwt, which are not needed in the Application layer. This way, we keep the Application layer clean and focused on business logic, while the Infrastructure layer handles the implementation details of token generation.
    public class TokenService(IConfiguration config) : ITokenService
    {
        public string CreateToken(UserDto user)
        {
            var tokenKey = config["TokenKey"] ?? throw new Exception("Cannot get token key");
            if (tokenKey.Length < 64)
            {
                throw new Exception("Your token key needs to be >= 64 characters");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, user.DisplayName),
                new(ClaimTypes.NameIdentifier, user.Id)
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}

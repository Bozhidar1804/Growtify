using Growtify.API.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Growtify.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
                options.AddPolicy("ModeratePhotoRole", policy => policy.RequireRole("Admin", "Moderator"));
            });
            return services;
        }
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddScoped<LogUserActivity>();
            return services;
        }
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
        {
            var tokenKey = config["TokenKey"] ?? throw new Exception("Token key not found - Program.cs");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(tokenKey)),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };
                    });

            return services;
        }

    }
}

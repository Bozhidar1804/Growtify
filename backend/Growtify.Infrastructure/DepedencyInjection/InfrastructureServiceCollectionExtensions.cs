using Growtify.Application.Common.Settings;
using Growtify.Application.Interfaces.Repositories;
using Growtify.Application.Interfaces.Services;
using Growtify.Infrastructure.Repositories;
using Growtify.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Growtify.Infrastructure.DepedencyInjection
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ILikesRepository, LikesRepository>();

            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<ITokenService, TokenService>();

            services.Configure<CloudinarySettings>(
                config.GetSection("CloudinarySettings"));

            return services;
        }
    }
}

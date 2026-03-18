using Growtify.Application.Interfaces.Services;
using Growtify.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Growtify.Application.Common.DependencyInjection
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IAccountService, AccountService>();

            return services;
        }
    }
}

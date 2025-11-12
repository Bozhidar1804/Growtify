using Microsoft.EntityFrameworkCore;
using Growtify.Application.Interfaces;
using Growtify.Infrastructure.Data;
using Growtify.Infrastructure.Services;
using Growtify.API.Extensions;

namespace Growtify.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            string? connectionString = builder.Configuration.GetConnectionString("GrowtifyConnection") ?? throw new InvalidOperationException("Connection string 'GrowtifyConnection' not found.");

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddJwtAuthentication(builder.Configuration);


            // TODO: Register Application layer services (example, add later)
            // builder.Services.AddScoped<IUserProfileService, UserProfileService>();
            // !!! OR create an AddServices() method that registers all services in the Application layer

            builder.Services
                .AddDbContext<GrowtifyDbContext>(options =>
            {                 
                options.UseSqlServer(connectionString);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(policy =>
            {
                policy.AllowAnyHeader()
                      .AllowAnyMethod()
                      .WithOrigins("http://localhost:4200", "https://localhost:4200");
            });

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

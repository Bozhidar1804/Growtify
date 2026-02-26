using Growtify.API.Extensions;
using Growtify.API.Middlewares;
using Growtify.Application.Interfaces;
using Growtify.Application.Interfaces.Repositories;
using Growtify.Infrastructure.Data;
using Growtify.Infrastructure.Helpers;
using Growtify.Infrastructure.Services;
using Growtify.Infrastructure.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Growtify.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            string? connectionString = builder.Configuration.GetConnectionString("GrowtifyConnection") ?? throw new InvalidOperationException("Connection string 'GrowtifyConnection' not found.");

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<IPhotoService, PhotoService>();
            builder.Services.AddScoped<IMemberRepository, MemberRepository>();
            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
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

            app.UseMiddleware<ExceptionMiddleware>();
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

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<GrowtifyDbContext>();
                await context.Database.MigrateAsync();
                await Seed.SeedUsers(context);
            } catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occured during migration/seeding the database!");
            };

            app.Run();
        }
    }
}

using Growtify.API.Extensions;
using Growtify.API.Middlewares;
using Growtify.Infrastructure.Data;
using Growtify.Infrastructure.DepedencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Growtify.Application.Common.DependencyInjection;
using Growtify.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Growtify.API.SignalR;

namespace Growtify.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string? connectionString = builder.Configuration.GetConnectionString("GrowtifyConnection") ?? throw new InvalidOperationException("Connection string 'GrowtifyConnection' not found.");

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            builder.Services.AddDbContext<GrowtifyDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors();

            builder.Services.AddApiServices();
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);

            builder.Services.AddJwtAuthentication(builder.Configuration);
            builder.Services.AddCustomAuthorization();
            builder.Services.AddSignalR();

            builder.Services.AddIdentityCore<AppUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.User.RequireUniqueEmail = true;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<GrowtifyDbContext>();


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
                      .AllowCredentials()
                      .WithOrigins("http://localhost:4200", "https://localhost:4200");
            });

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapHub<PresenceHub>("hubs/presence");

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<GrowtifyDbContext>();
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                await context.Database.MigrateAsync();
                await Seed.SeedUsers(userManager);
            } catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occured during migration/seeding the database!");
            };

            app.Run();
        }
    }
}

using Growtify.Application.DTOs.Account;
using Growtify.Domain.Entities;
using Growtify.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Growtify.Infrastructure.Data
{
    public static class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager)
        {
            if (userManager.Users.Any()) return;

            var json = await File.ReadAllTextAsync("../Growtify.Infrastructure/Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<SeedUserDto>>(json);

            if (users == null)
            {
                Console.WriteLine("No members in seed data.");
                return;
            }

            foreach (var u in users)
            {
                AppUser appUser = new AppUser
                {
                    Id = u.Id,
                    UserName = u.Email,
                    Email = u.Email,
                    DisplayName = u.UserName,
                    ImageUrl = u.ImageUrl,
                    CreatedAt = u.Created
                };

                Member member = new Member
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    ImageUrl = u.ImageUrl,
                    DateOfBirth = u.DateOfBirth,
                    Created = u.Created,
                    LastActive = u.LastActive,
                    Gender = u.Gender,
                    Description = u.Description,
                    City = u.City,
                    Country = u.Country
                };

                appUser.Member = member;
                appUser.Member.Photos.Add(new Photo
                {
                    Url = member.ImageUrl!,
                    MemberId = member.Id
                });

                IdentityResult? result = await userManager.CreateAsync(appUser, "Pa$$w0rd");
                if (!result.Succeeded)
                {
                    Console.WriteLine(result.Errors.First().Description);
                }

                await userManager.AddToRoleAsync(appUser, "Member");
            }

            AppUser admin = new AppUser
            {
                UserName = "admin@test.com",
                Email = "admin@test.com",
                DisplayName = "Admin"
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, ["Admin", "Moderator"]);
        }
    }
}
